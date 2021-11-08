using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DataModels;
using DataModels.Models;
using NLog;

namespace CatchChangesREST
{
    public static class RequestBuilder
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void InitData(out string key, out string token, out string telegramToken)
        {
            if (CredentialsManager.TryInit())
            {
                key = CredentialsManager.Credentials.TrelloKey;
                token = CredentialsManager.Credentials.TrelloToken;
                telegramToken = CredentialsManager.Credentials.TelegramToken;
            }
            else
            {
                key = string.Empty;
                token = string.Empty;
                telegramToken = string.Empty;
            }
        }

        public static async Task<HttpResponseMessage> GetAsync(string url)
        {
            InitData(out var key, out var token, out _);
            return await new HttpClient().GetAsync(url + $"&key={key}&token={token}");
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(string uri, T httpContentModel)
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

        public static async Task<HttpResponseMessage> PutAsync<T>(string uri, T httpContentModel)
        {
            return await PutAsync(uri, httpContentModel, CancellationToken.None);
        }

        public static async Task<HttpResponseMessage> PutAsync<T>(string uri, T httpContentModel,
            CancellationToken cancellationToken)
        {
            var jsonString = JsonSerializer.Serialize(httpContentModel);
            var content = new StringContent(jsonString);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var reader = new StreamReader(await content.ReadAsStreamAsync(cancellationToken));
            var logInfo = await reader.ReadToEndAsync();
            Logger.Trace($"New put request: URI: {uri} Length: {logInfo.Length} Content: {logInfo}");

            return await new HttpClient().PutAsync(uri, content, cancellationToken);
        }
    }
}