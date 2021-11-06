using System;
using DataModels;

namespace CatchChangesREST.Clients
{
    public class TelegramBot : IClient
    {
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

        public event EventHandler<CommandExecutedEventArgs> CommandExecuted;
        public void OnCommandExecuted(object sender, CommandExecutedEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}