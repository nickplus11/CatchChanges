using System;
using System.Threading;
using System.Threading.Tasks;
using DataModels;
using DataModels.Models;

namespace CatchChangesREST.DataSources
{
    public class Trello : IDataSource
    {
        public event EventHandler<TableChangedEventArgs> TableChanged;
        public event EventHandler<ListChangedEventArgs> ListChanged;
        public event EventHandler<CardChangedEventArgs> CardChanged;
        public Task<bool> TryChangeTableAsync(Table targetTable, Table newTable)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryChangeTableAsync(Table targetTable, Table newTable, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryChangeListAsync(List targetList, List newList)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryChangeListAsync(List targetList, List newList, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryChangeCardAsync(Card targetCard, Card newCard)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryChangeCardAsync(Card targetCard, Card newCard, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}