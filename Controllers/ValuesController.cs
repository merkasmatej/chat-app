using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace App.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public class LoginModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        [ResponseType(typeof(int))]
        [Route("api/Login")]
        public int Login([FromBody]LoginModel model)
        {
            using (var ctx = new ChatApp())
            {
                var user = ctx.Users.FirstOrDefault(p => p.UserName == model.UserName && p.Password == model.Password);
                if (user == null)
                {
                    user = ctx.Users.Add(new User
                    {
                        UserName = model.UserName,
                        Password = model.Password
                    });
                    ctx.SaveChanges();
                }

                return user.UserID;
            }
        }


        public class ConversationModel
        {
            public string Name { get; set; }
            public int UserId { get; set; }
            public string Members { get; set; }
        }

        [HttpPost]
        [Route("api/Conversation")]
        public int Conversation([FromBody]ConversationModel model)
        {
            using (var ctx = new ChatApp())
            {
                var memberNames = model.Members.Split(';');
                var newConversation = new Conversation
                {
                    Name = model.Name,
                    Members = new List<User>(),
                    CreatedDate = DateTime.Now
                };
                var members = ctx.Users.Where(p => memberNames.Contains(p.UserName)).ToList();
                var user = ctx.Users.Find(model.UserId);
                members.Add(user);
                newConversation.Members = members;

                ctx.Conversations.Add(newConversation);
                ctx.SaveChanges();

                return newConversation.Id;
            }
        }

        [HttpGet]
        [Route("api/Conversations/{userId}")]
        public IEnumerable<object> Conversation(int userId)
        {
            using (var ctx = new ChatApp())
            {
                var user = ctx.Users.Include("Conversations").FirstOrDefault(p => p.UserID == userId);

                return user.Conversations.Select(p => new
                {
                    id = p.Id,
                    name = p.Name
                }).ToList();
            }
        }

        [HttpGet]
        [Route("api/Messages/{conversationId}")]
        public IEnumerable<object> Message(int conversationId)
        {
            using (var ctx = new ChatApp())
            {
                var conversation = ctx.Conversations.Include("Messages").Include("Messages.User").FirstOrDefault(p => p.Id == conversationId);

                return conversation.Messages.Select(p => new
                {
                    id = p.MessageID,
                    text = p.Text,
                    user = p.User.UserName
                }).ToList();
            }
        }
    }
}
