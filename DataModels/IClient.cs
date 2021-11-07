using System;
using System.Threading;
using System.Threading.Tasks;
using DataModels.Models;

namespace DataModels
{
    public interface IClient
    {
        public void OnTableChanged(object sender, TableChangedEventArgs args);
        public void OnListChanged(object sender, ListChangedEventArgs args);
        public void OnCardChanged(object sender, CardChangedEventArgs args);
    }
}