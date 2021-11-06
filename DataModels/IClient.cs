using System;

namespace DataModels
{
    public interface IClient
    {
        public void OnTableChanged(object sender, TableChangedEventArgs args);
        public void OnListChanged(object sender, ListChangedEventArgs args);
        public void OnCardChanged(object sender, CardChangedEventArgs args);

        public event EventHandler<CommandExecutedEventArgs> CommandExecuted; 
        public void OnCommandExecuted(object sender, CommandExecutedEventArgs args);
    }

    public class CommandExecutedEventArgs { }
}