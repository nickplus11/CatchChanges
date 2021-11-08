using System.Threading.Tasks;

namespace DataModels
{
    public interface IClient
    {
        string Name { get; }
        public void OnTableChanged(object sender, TableChangedEventArgs args);
        public void OnListChanged(object sender, ListChangedEventArgs args);
        public void OnCardChanged(object sender, CardChangedEventArgs args);
        public void OnUpdateReceived(object sender, UpdateReceivedEventArgs args);
        Task SendAsync(int chatId, string info);
    }
}