using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using App.Models;

namespace App.Hubs
{
    public class ConnectedUsers {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public List<User> Recipients { get; set; }
        public int UserId { get; internal set; }
    }

    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);  
        }

        public void NewMessage(int userId, int conversationId, string message)
        {
            using (var ctx = new ChatApp())
            {
                var user = ctx.Users.Find(userId);
                var conversation = ctx.Conversations.Include("Members").Include("Messages").FirstOrDefault(p => p.Id == conversationId);

                if (conversation == null)
                    return;

                conversation.Messages.Add(new Message {
                    CreatingDate = DateTime.Now,
                    Text = message,
                    User = user 
                });
                ctx.SaveChanges();

                Clients.Group(conversation.Name).broadcastMessage(user.UserName, message);
            }
        }

        public void Connect(int userId, int conversationId) {
            using (var ctx = new ChatApp()) {
                var conversation = ctx.Conversations.Where(p => p.Members.Any(d => d.UserID == userId)).FirstOrDefault(p => p.Id == conversationId);

                if (conversation == null)
                    return;

                Groups.Add(Context.ConnectionId, conversation.Name);
            }
        }
    }
}