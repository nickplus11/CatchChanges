using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using DataModels;
using DataModels.Models;
using NLog;

namespace CatchChangesREST.DataSources
{
    public static class RequestBuilder
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void InitData(out string key, out string token)
        {
            if (CredentialsManager.TryInit())
            {
                key = CredentialsManager.Credentials.Key;
                token = CredentialsManager.Credentials.Token;
            }
            else
            {
                key = string.Empty;
                token = string.Empty;
            }
        }

        private static async Task<HttpResponseMessage> GetAsync(string url)
        {
            InitData(out var key, out var token);
            return await new HttpClient().GetAsync(url + $"&key={key}&token={token}");
        }

        private static async Task<HttpResponseMessage> PostAsync<T>(string uri, T httpContentModel)
        {
            //InitData(out var key, out var token);
            var jsonString = JsonSerializer.Serialize(httpContentModel);
            var content = new StringContent(jsonString);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var reader = new StreamReader(await content.ReadAsStreamAsync());
            var logInfo = await reader.ReadToEndAsync();
            Logger.Trace($"New posting: URI: {uri} Length: {logInfo.Length} Content: {logInfo}");

            return await new HttpClient().PostAsync(uri, content);
        }

        public static async Task<IReadOnlyList<Table>> GetAllTablesAsync()
        {
            try
            {
                var url = "https://api.trello.com/1/members/me/boards?fields=name,url";
                var response = await GetAsync(url);
                var stream = await response.Content.ReadAsStreamAsync();
                var tables = await JsonSerializer.DeserializeAsync<List<Table>>(stream)
                             ?? throw new Exception("Tables have not been received");

                foreach (var table in tables)
                {
                    Logger.Trace($"Received table. Name: {table.Name} Id: {table.Id}");
                }

                return tables;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ImmutableList<Table>.Empty;
            }
        }

        public static async Task<Table> GetTableAsync(string tableId)
        {
            try
            {
                var tables = await GetAllTablesAsync();
                var table = tables.FirstOrDefault(t => t.Id == tableId) ??
                            throw new Exception("Table hasn't been found");
                Logger.Trace($"Received table. Name: {table.Name} Id: {table.Id}");
                return table;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        /// <param name="idModel">The id of a model to watch. This can be the id of a member, card, board, or anything that actions apply to. Any event involving this model will trigger the webhook.</param>
        public static async Task<HttpResponseMessage> CreateWebhookAsync(string idModel)
        {
            InitData(out var key, out var token);
            var uri = $"https://api.trello.com/1/tokens/{token}/webhooks/?key={key}";
            var webhookCreatingPostJson = new WebRequests.WebhookCreatingPostJson
            {
                CallbackUrl = "https://catchchangesrest.azurewebsites.net/webhook/receive",
                IdModel = idModel,
                Description = "New webhook: " + DateTime.Now
            };
            Logger.Trace($"Starting webhook creation. URI: {uri}");
            return await PostAsync(uri, webhookCreatingPostJson);
        }

        public static async Task ReceiveWebhookAsync(string requestBody)
        {
            try
            {
                ReceivedChanges.Add(requestBody);
                Logger.Trace(requestBody);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private static readonly List<string> ReceivedChanges = new List<string>();

        public static IReadOnlyList<string> GetReceivedChangesAsync()
        {
            return ReceivedChanges;
        }
    }
}