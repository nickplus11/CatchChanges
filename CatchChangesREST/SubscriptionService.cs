using System.Collections.Generic;
using System.Linq;
using DataModels;
using NLog;

namespace CatchChangesREST
{
    public class SubscriptionService
    {
        public readonly IReadOnlyList<IClient> Clients;
        public readonly IReadOnlyList<IDataSource> DataSources;
        public readonly Dictionary<string, IDataSource> DataSourceByName;

        public SubscriptionService(IEnumerable<IClient> clients, IEnumerable<IDataSource> dataSources)
        {
            Clients = clients.ToList();
            DataSources = dataSources.ToList();
            DataSourceByName = new();

            foreach (var dataSource in DataSources)
            {
                DataSourceByName.Add(dataSource.Name, dataSource);
                foreach (var client in Clients)
                {
                    dataSource.TableChanged += client.OnTableChanged;
                    dataSource.ListChanged += client.OnListChanged;
                    dataSource.CardChanged += client.OnCardChanged;
                }
            }

            LogManager.GetCurrentClassLogger().Trace("Subscription service has been created");
        }
    }
}