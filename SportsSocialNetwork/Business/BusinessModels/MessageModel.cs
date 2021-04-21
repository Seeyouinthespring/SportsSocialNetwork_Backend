using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Business.BusinessModels
{
    public class MessageDtoModel
    {
        public string Text { get; set; }
    }

    public class MessageViewModel : MessageDtoModel 
    { 
        public long Id { get; set; }

        public string ReceiverId { get; set; }

        public string SenderId { get; set; }

        public DateTime Date { get; set; }
    }

    public class ChatViewModel 
    {
        public ApplicationUserMessageViewModel CurrentUser { get; set; }

        public ApplicationUserMessageViewModel ChatWithUser { get; set; }

        public List<MessageViewModel> Messages { get; set; }
    }
}
