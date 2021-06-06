using Infrastructure.Aggregets.MessageAgg;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public MessageService()
        {
           
        }
   

        public Message GetMessage(int id)
        {
            return unitOfWork.MessageRepo.GetByID(id);
        }
        
    
        public IEnumerable<Message> GetMessageThread(string currentUserId, string RecipientId)
        {
            
            var messages = unitOfWork.MessageRepo.Get(
               (e => e.RecipientId == currentUserId && e.SenderId == RecipientId ||
               e.RecipientId == RecipientId && e.SenderId == currentUserId),null,
               "Sender,Recipient").ToList();



            return messages;

        
        }
        public IEnumerable<Message> GetMessageThreadForDesktop(string senderUserName, string RecipientUserName)
        {

            var messages = unitOfWork.MessageRepo.Get(
               (e => e.RecipientUsername == senderUserName && e.SenderUsername == RecipientUserName ||
               e.RecipientUsername == RecipientUserName && e.SenderUsername == senderUserName), null,
               "Sender,Recipient").ToList();



            return messages;


        }
    }
}
