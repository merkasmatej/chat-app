namespace App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class ChatApp : DbContext
    {
       
        public ChatApp()
            : base("name=ChatApp")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
    }

    public class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IsActive { get; set; }

        public virtual List<Conversation> Conversations { get; set; }
        public virtual List<Message> Messages { get; set; }
        public string UserName { get; internal set; }
        public string Password { get; internal set; }

        public User()
        {
            this.Conversations = new List<Conversation>();
            this.Messages = new List<Message>();
        }

    }

    public class Conversation
    {
        public int Id { get; set; }
        public string Name { get; internal set; }
        public virtual List<User> Members { get; internal set; }
        public virtual List<Message> Messages { get; internal set; }
        public DateTime CreatedDate { get; internal set; }
    }

    public class Message
    {
        public int MessageID { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public DateTime CreatingDate { get; set; }

        public virtual User User { get; set; }
        public string Text { get; internal set; }
    }

}