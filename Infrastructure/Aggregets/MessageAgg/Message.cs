using Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Aggregets.MessageAgg
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string SenderUsername { get; set; }
        public ApplicationUser Sender { get; set; }
        public string RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public ApplicationUser Recipient { get; set; }
        public string Content { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.Now;
    }
}
