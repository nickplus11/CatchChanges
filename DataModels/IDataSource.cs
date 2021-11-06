using System;
using System.Threading;
using System.Threading.Tasks;
using DataModels.Models;

namespace DataModels
{
    public interface IDataSource
    {
        public event EventHandler<TableChangedEventArgs> TableChanged;
        public event EventHandler<ListChangedEventArgs> ListChanged;
        public event EventHandler<CardChangedEventArgs> CardChanged;

        public Task<bool> TryChangeTableAsync(Table targetTable, Table newTable);
        public Task<bool> TryChangeTableAsync(Table targetTable, Table newTable, CancellationToken cancellationToken);

        public Task<bool> TryChangeListAsync(List targetList, List newList);
        public Task<bool> TryChangeListAsync(List targetList, List newList, CancellationToken cancellationToken);

        public Task<bool> TryChangeCardAsync(Card targetCard, Card newCard);
        public Task<bool> TryChangeCardAsync(Card targetCard, Card newCard, CancellationToken cancellationToken);
    }

    public class TableChangedEventArgs : EventArgs, IHaveChanges
    {
        public Table Table { get; set; }
        public bool HaveChanges { get; }
    }

    public class ListChangedEventArgs : EventArgs, IHaveChanges
    {
        public List List { get; set; }
        public bool HaveChanges { get; }
    }

    public class CardChangedEventArgs : EventArgs, IHaveChanges
    {
        public Card Card { get; set; }
        public bool HaveChanges { get; }
    }

    public interface IHaveChanges
    {
        public bool HaveChanges { get; }
    }
}