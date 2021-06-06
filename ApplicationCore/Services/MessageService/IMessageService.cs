using Infrastructure.Aggregets.MessageAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services.MessageService
{
    public interface IMessageService
    {

        Message GetMessage(int id);
        IEnumerable<Message> GetMessageThread(string currentUserId, string RecipientId);

    }
}
