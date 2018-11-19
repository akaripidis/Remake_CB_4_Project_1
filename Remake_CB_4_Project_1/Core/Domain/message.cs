using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remake_CB_4_Project_1.Core.Domain
{
    public class Message
    {
        public int Id { get; set; }
        public User SenterId { get; set; }
        public User ReceiverId { get; set; }
        public bool IsRead { get; set; }
        public string Title { get; set; }
        public string ActualMessage { get; set; }
        public DateTime MessageDate { get; set; }




    }

}
