using Microsoft.AspNet.SignalR;

namespace Naftan.Maintenance.WebApplication.SignalR
{
    public class ChatHub : Hub
    {
        public void Send(string message)
        {
            Clients.Others.showMessage(message);
        }
    }
}