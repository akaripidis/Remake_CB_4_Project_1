using Remake_CB_4_Project_1.Core.Domain;
using System.Data.Entity;

namespace Remake_CB_4_Project_1.Persistance
{
    public class MessageAppContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

        public MessageAppContext() : base("name=DefaultConnection")
        {

        }

       
    }
}