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

        public SubscriptionService(IEnumerable<IClient> clients, IEnumerable<IDataSource> dataSources)
        {
            Clients = clients.ToList();
            DataSources = dataSources.ToList();

            foreach (var dataSource in DataSources)
            {
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