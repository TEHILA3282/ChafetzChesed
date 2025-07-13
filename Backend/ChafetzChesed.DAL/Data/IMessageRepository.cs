using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChafetzChesed.DAL.Entities;

namespace ChafetzChesed.DAL.Data
{
    public interface IMessageRepository
    {
        List<Message> GetMessagesByClientId(string clientId);
        void AddMessage(Message message);
    }
}

