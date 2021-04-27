using SportsSocialNetwork.Business.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface IMessagesService
    {
        Task<ChatViewModel> GetChatAsync(string currentUserId, string chatWithUserId, int skip, int take);
        Task<List<ChatViewModel>> GetAllChatsAsync(string currentUserId);
        Task<ChatViewModel> AddMessageAsync(MessageDtoModel model, string receiverId, string currentUserId, DateTime currentDate);
        Task DeleteAsync(long id);
        Task<MessageViewModel> GetMessageAsync(long id);
    }
}
