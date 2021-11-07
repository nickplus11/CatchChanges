using System;
using System.Threading;
using System.Threading.Tasks;
using DataModels;
using DataModels.Models;
using JetBrains.Annotations;
using NLog;
using static CatchChangesREST.RequestBuilder;

namespace CatchChangesREST.Clients
{
    [UsedImplicitly]
    public class TelegramBot : IClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public TelegramBot()
        {
            Logger.Trace("Telegram bot client has been created");
        }

        public void OnTableChanged(object sender, TableChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnListChanged(object sender, ListChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnCardChanged(object sender, CardChangedEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}