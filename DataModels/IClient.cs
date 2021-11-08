using System.Threading.Tasks;

namespace DataModels
{
    public interface IClient
    {
        public string Name { get; }
        public void OnTableChanged(object sender, TableChangedEventArgs args);
        public void OnListChanged(object sender, ListChangedEventArgs args);
        public void OnCardChanged(object sender, CardChangedEventArgs args);
        public void OnUpdateReceived(object sender, UpdateReceivedEventArgs args);
        public Task SendAsync(int chatId, string info);
        public void Subscribe(object userObj, object chatObj);
    }
}