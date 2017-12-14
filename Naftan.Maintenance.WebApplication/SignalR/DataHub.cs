using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;

namespace Naftan.Maintenance.WebApplication.SignalR
{
    public class DataHub:Hub
    {
        public void DataChange(string collection, int id, string operation, JObject data)
        {
            Clients.Others.dataChangeHandler(collection, id, operation, data);
        }
    }
}