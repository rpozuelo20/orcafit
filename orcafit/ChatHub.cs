using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit
{
    public class ChatHub:Hub
    {
        public async Task SendMessage(string room, string user, string message, string image)
        {
            DateTime time = DateTime.UtcNow;
            await Clients.Group(room).SendAsync("ReceiveMessage", user, message, image, time.AddHours(2).ToShortTimeString().ToString());
        }
        public async Task AddToGroup(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
        }
    }
}
