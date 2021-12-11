using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DataModels;
using DataModels.Models;
using JetBrains.Annotations;
using NLog;
using static CatchChangesREST.RequestBuilder;

namespace CatchChangesREST.DataSources
{
    [UsedImplicitly]
    public class Trello : IDataSource
    {
        private const string BaseUrl = "https://api.trello.com";
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public Trello()
        {
            _logger.Trace("Trello data source has been created");
        }

        public string Name => "trello";
        public event EventHandler<TableChangedEventArgs> TableChanged;
        public event EventHandler<ListChangedEventArgs> ListChanged;
        public event EventHandler<CardChangedEventArgs> CardChanged;

        public void ReceiveChangedTable(Table oldTable, Table newTable)
        {
            TableChanged?.Invoke(this, new TableChangedEventArgs { OldTable = oldTable, NewTable = newTable });
        }

        public void ReceiveChangedList(Card oldCard, Card newList)
        {
            ListChanged?.Invoke(this, new ListChangedEventArgs { });
        }

        public void ReceiveChangedCard(ListOfCards oldListOfCards, ListOfCards newCard)
        {
            CardChanged?.Invoke(this, new CardChangedEventArgs { });
        }

        public async Task<bool> TryChangeTableAsync(Table targetTable, Table newTable)
        {
            return await TryChangeTableAsync(targetTable, newTable, CancellationToken.None);
        }

        public async Task<bool> TryChangeTableAsync(Table targetTable, Table newTable,
            CancellationToken cancellationToken)
        {
            try
            {
                InitData(out var key, out var token, out _);
                var url = $"{BaseUrl}/1/boards/{targetTable.Id}";
                var model = new
                {
                    key = key,
                    token = token,
                    name = newTable.Name
                };

                await PutAsync(url, model, cancellationToken);
                return true;
            }
            catch (OperationCanceledException operationCanceledException)
            {
                _logger.Error("Operation has been cancelled. " + operationCanceledException.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return false;
        }

        public async Task<bool> TryChangeListAsync(ListOfCards targetListOfCards, ListOfCards newListOfCards)
        {
            return await TryChangeListAsync(targetListOfCards, newListOfCards, CancellationToken.None);
        }

        public async Task<bool> TryChangeListAsync(ListOfCards targetListOfCards, ListOfCards newListOfCards,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> TryChangeCardAsync(Card targetCard, Card newCard)
        {
            return await TryChangeCardAsync(targetCard, newCard, CancellationToken.None);
        }

        public async Task<bool> TryChangeCardAsync(Card targetCard, Card newCard, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Table>> GetAllTablesAsync()
        {
            try
            {
                var url = $"{BaseUrl}/1/members/me/boards?fields=name,url";
                var response = await GetAsync(url);
                var stream = await response.Content.ReadAsStreamAsync();
                var tables = await JsonSerializer.DeserializeAsync<List<Table>>(stream)
                             ?? throw new Exception("Tables have not been received");

                foreach (var table in tables)
                {
                    _logger.Trace($"Received table. Name: {table.Name} Id: {table.Id}");
                }

                return tables;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ImmutableList<Table>.Empty;
            }
        }

        public async Task<Table> GetTableAsync(string tableId)
        {
            try
            {
                var tables = await GetAllTablesAsync();
                var table = tables.FirstOrDefault(t => t.Id == tableId) ??
                            throw new Exception("Table hasn't been found");
                _logger.Trace($"Received table. Name: {table.Name} Id: {table.Id}");
                return table;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        // NOTE API can not process this
        // TODO find out what the problem is
        public async Task<IReadOnlyList<ListOfCards>> GetAllListsAsync(string tableId)
        {
            try
            {
                var url = $"{BaseUrl}/1/boards/{tableId}/lists";
                var response = await GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new Exception(await new StreamReader(await response.Content.ReadAsStreamAsync())
                        .ReadToEndAsync());

                var stream = await response.Content.ReadAsStreamAsync();
                var lists = await JsonSerializer.DeserializeAsync<List<ListOfCards>>(stream)
                            ?? throw new Exception("Lists have not been received");

                _logger.Trace($"Received some lists of cards. Count: {lists.Count}. Names: " +
                              string.Join(", ", lists.Select(l => l.Name)));
                return lists;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        public async Task<ListOfCards> GetListAsync(string listId)
        {
            try
            {
                var url = $"{BaseUrl}/1/lists/{listId}";
                var response = await GetAsync(url);
                var stream = await response.Content.ReadAsStreamAsync();
                var list = await JsonSerializer.DeserializeAsync<ListOfCards>(stream)
                           ?? throw new Exception("List hasn't been received");

                _logger.Trace($"Received list of cards. Name: {list.Name} Id: {list.Id}");
                return list;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        public Task<IReadOnlyList<Card>> GetAllCardsAsync(string listId)
        {
            throw new NotImplementedException();
        }

        public Task<Card> GetCardAsync(string cardId)
        {
            throw new NotImplementedException();
        }

        /// <param name="idModel">The id of a model to watch. This can be the id of a member, card, board, or anything that actions apply to. Any event involving this model will trigger the webhook.</param>
        public async Task<HttpResponseMessage> CreateWebhookAsync(string idModel)
        {
            InitData(out var key, out var token, out _);
            var uri = $"https://api.trello.com/1/tokens/{token}/webhooks/?key={key}";
            var webhookCreatingPostJson = new WebhookCreatingPostJson
            {
                CallbackUrl = "https://catchchangesrest.azurewebsites.net/trello/webhook/receive",
                IdModel = idModel,
                Description = "New webhook: " + DateTime.Now
            };
            _logger.Trace($"Starting webhook creation. URI: {uri}");
            return await PostAsync(uri, webhookCreatingPostJson);
        }

        public async Task<HttpResponseMessage> ReceiveWebhookAsync(string requestBody)
        {
            try
            {
                _logger.Trace(requestBody);
                ReceiveChangedTable(new Table(), new Table
                {
                    Id = "invalid Id",
                    Name = requestBody
                });
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}