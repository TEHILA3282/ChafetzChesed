using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChafetzChesed.DAL.Entities
{
    public class Message
    {
        public int ID { get; set; }
        public string ClientID { get; set; } = null!; // קישור ל-Registration
        public string MessageType { get; set; } = null!; // 'מערכת' או 'גביה'
        public string MessageText { get; set; } = null!;
        public DateTime DateSent { get; set; }
        public bool IsRead { get; set; }
    }
}
