using System.Collections.Generic;

namespace Remake_CB_4_Project_1.Core.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Saltword { get; set; }
        public string Password { get; set; }
        public int AccessLevel { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public User()
        {
            Messages = new HashSet<Message>();
        }
        //public User(int id,string name,int accessLevel):base()
        //{
        //    Id = id;
        //    Name = name;
        //    AccessLevel = accessLevel;
        //}







        
        
    }
}
