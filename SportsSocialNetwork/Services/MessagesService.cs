using Microsoft.EntityFrameworkCore;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Helpers;
using SportsSocialNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly ICommonRepository _commonRepository;

        public MessagesService(ICommonRepository commonRepository) 
        {
            _commonRepository = commonRepository;
        }

        public async Task<ChatViewModel> GetChatAsync(string currentUserId, string chatWithUserId, int skip, int take) 
        {
            List<Message> messages = await _commonRepository.FindByCondition<Message>(x =>
                    x.ReceiverId == chatWithUserId && x.SenderId == currentUserId ||
                    x.SenderId == chatWithUserId && x.ReceiverId == currentUserId)
                .OrderByDescending(x => x.Date)
                .Skip(skip)
                .Take(take).ToListAsync();

            var users = await _commonRepository.FindByCondition<ApplicationUser>(x => x.Id == currentUserId || x.Id == chatWithUserId)
                .Select(x => new ApplicationUser
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Id = x.Id
                    //Photo = x.Photo
                }).ToListAsync();

            ChatViewModel result = new ChatViewModel
            {
                CurrentUser = users.FirstOrDefault(x => x.Id == currentUserId).MapTo<ApplicationUserMessageViewModel>(),
                ChatWithUser = users.FirstOrDefault(x => x.Id == chatWithUserId).MapTo<ApplicationUserMessageViewModel>(),
                Messages = messages.MapTo<List<MessageViewModel>>()
            };
            return result;
        }

        public async Task<List<ChatViewModel>> GetAllChatsAsync(string currentUserId) 
        {
            var currentUser = await _commonRepository.FindByCondition<ApplicationUser>(x => x.Id == currentUserId)
                .Select(x => new ApplicationUser
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Id = x.Id
                    //Photo = x.Photo

                }).FirstOrDefaultAsync();

            var usersList = await _commonRepository.FindByCondition<ApplicationUser>(x =>
                    x.ReceivedMessages.Any(m => m.SenderId == currentUserId) ||
                    x.SentMessages.Any(m => m.ReceiverId == currentUserId)).Include(x => x.ReceivedMessages).Include(x => x.SentMessages)
                .ToListAsync();
                
            List<ApplicationUser> users = usersList
                .Select(x => new ApplicationUser
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Id = x.Id,
                    //Photo = x.Photo
                    SentMessages = new List<Message>() { x.SentMessages.Where(x => x.ReceiverId == currentUserId).OrderByDescending(x => x.Date).First() },
                    ReceivedMessages = new List<Message>() { x.ReceivedMessages.Where(x => x.SenderId == currentUserId).OrderByDescending(x => x.Date).First() },
                }).ToList();

            List<ChatViewModel> chats = new List<ChatViewModel>();
            for (int i = 0; i < users.Count(); i++) 
            {
                List<Message> lastMessages = new List<Message>();
                if (users[i].SentMessages == null || !users[i].SentMessages.Any())
                    lastMessages.Add(users[i].ReceivedMessages.First());
                else if (users[i].ReceivedMessages == null || !users[i].ReceivedMessages.Any())
                    lastMessages.Add(users[i].SentMessages.First());
                else if (users[i].SentMessages.First().Date > users[i].ReceivedMessages.First().Date)
                    lastMessages.Add(users[i].SentMessages.First());
                else
                    lastMessages.Add(users[i].ReceivedMessages.First());
                ChatViewModel chat;
                if (i == 0)
                    chat = new ChatViewModel
                    {
                        CurrentUser = currentUser.MapTo<ApplicationUserMessageViewModel>(),
                        ChatWithUser = users[i].MapTo<ApplicationUserMessageViewModel>(),
                        Messages = lastMessages.MapTo<List<MessageViewModel>>()
                    };
                else
                    chat = new ChatViewModel
                    {
                        CurrentUser = null,
                        ChatWithUser = users[i].MapTo<ApplicationUserMessageViewModel>(),
                        Messages = lastMessages.MapTo<List<MessageViewModel>>()
                    };
                chats.Add(chat);
            }
            return chats;
        }

        public async Task<ChatViewModel> AddMessageAsync(MessageDtoModel model, string receiverId, string currentUserId, DateTime currentDate) 
        {
            Message entity = model.MapTo<Message>();
            entity.ReceiverId = receiverId;
            entity.SenderId = currentUserId;
            entity.Date = currentDate;

            await _commonRepository.AddAsync(entity);
            await _commonRepository.SaveAsync();

            var message = await _commonRepository.FindByCondition<Message>(x => x.Id == entity.Id)
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .FirstOrDefaultAsync();

            ChatViewModel result = new ChatViewModel
            {
                ChatWithUser = message.Receiver.MapTo<ApplicationUserMessageViewModel>(),
                CurrentUser = message.Sender.MapTo<ApplicationUserMessageViewModel>(),
                Messages = new List<MessageViewModel>() { message.MapTo<MessageViewModel>() }
            };
            return result;
        }

        public async Task DeleteAsync(long id) 
        {
            await _commonRepository.DeleteAsync<Message>(id);
            await _commonRepository.SaveAsync();
        }

        public async Task<MessageViewModel> GetMessageAsync(long id)
        {
            Message entity = await _commonRepository.FindByIdAsync<Message>(id);
            if (entity == null) return null;
            return entity.MapTo<MessageViewModel>();
        }
    }
}
