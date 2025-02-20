﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DataModels.Models;

namespace DataModels
{
    public interface IDataSource
    {
        public string Name { get; }
        public event EventHandler<TableChangedEventArgs> TableChanged;
        public event EventHandler<ListChangedEventArgs> ListChanged;
        public event EventHandler<CardChangedEventArgs> CardChanged;

        public void ReceiveChangedTable(Table oldTable, Table newTable);
        public void ReceiveChangedList(Card oldCard, Card newList);
        public void ReceiveChangedCard(ListOfCards oldListOfCards, ListOfCards newCard);

        public Task<bool> TryChangeTableAsync(Table targetTable, Table newTable);
        public Task<bool> TryChangeTableAsync(Table targetTable, Table newTable, CancellationToken cancellationToken);

        public Task<bool> TryChangeListAsync(ListOfCards targetListOfCards, ListOfCards newListOfCards);

        public Task<bool> TryChangeListAsync(ListOfCards targetListOfCards, ListOfCards newListOfCards,
            CancellationToken cancellationToken);

        public Task<bool> TryChangeCardAsync(Card targetCard, Card newCard);
        public Task<bool> TryChangeCardAsync(Card targetCard, Card newCard, CancellationToken cancellationToken);

        public Task<IReadOnlyList<Table>> GetAllTablesAsync();
        public Task<Table> GetTableAsync(string tableId);

        public Task<IReadOnlyList<ListOfCards>> GetAllListsAsync(string tableId);
        public Task<ListOfCards> GetListAsync(string listId);

        public Task<IReadOnlyList<Card>> GetAllCardsAsync(string listId);
        public Task<Card> GetCardAsync(string cardId);

        public Task<HttpResponseMessage> CreateWebhookAsync(string idModel);
        public Task<HttpResponseMessage> ReceiveWebhookAsync(string idModel);
    }

    public class TableChangedEventArgs : EventArgs, IHaveChanges
    {
        public Table OldTable { get; set; }
        public Table NewTable { get; set; }
        public bool HaveChanges => OldTable != NewTable;
    }

    public class ListChangedEventArgs : EventArgs, IHaveChanges
    {
        public ListOfCards ListOfCards { get; set; }
        public bool HaveChanges { get; }
    }

    public class CardChangedEventArgs : EventArgs, IHaveChanges
    {
        public Card Card { get; set; }
        public bool HaveChanges { get; }
    }

    public class UpdateReceivedEventArgs : EventArgs, IHaveChanges
    {
        public ClientUpdate ClientUpdate { get; set; }
        public bool HaveChanges { get; }
    }

    public interface IHaveChanges
    {
        public bool HaveChanges { get; }
    }
}