using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsSocialNetwork.Attributes;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Controllers
{
    [Route("api/Chat")]
    public class MessagesController: BaseController
    {
        private readonly IMessagesService _service;

        public MessagesController(IMessagesService service) 
        {
            _service = service;
        }

        #region GetAllChats 
        /// <summary>
        /// Get all user`s chats
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [SwaggerResponse200(typeof(List<ChatViewModel>))]
        public async Task<IActionResult> Get()
        {
            return await GetResultAsync(() => _service.GetAllChatsAsync(GetCurrentUserId()));
        }
        #endregion

        #region GetChat
        /// <summary>
        /// Get Chat with user
        /// </summary>
        /// <param name="chatWithUserId">chatWithUserId. Example 2</param>
        /// <param name="skip">number of skipped records. Example 20</param>
        /// <param name="take">number of taken records. Example 20</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{chatWithUserId}")]
        [SwaggerResponse200(typeof(ChatViewModel))]
        public async Task<IActionResult> GetById(string chatWithUserId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            return await GetResultAsync(() => _service.GetChatAsync(GetCurrentUserId(), chatWithUserId, skip, take));
        }
        #endregion

        #region Create Message
        /// <summary>
        /// Create message
        /// </summary>
        /// <param name="model">Message Model</param>
        /// <param name="receiverId">Id of reseiver user. Example: a86402ee-6fb5-4140-a0c9-3941d79604c0</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("{receiverId}")]
        [SwaggerResponse200(typeof(AppointmentViewModel))]
        public async Task<IActionResult> Create([FromBody] MessageDtoModel model, string receiverId)
        {
            string userId = GetCurrentUserId();
            return await GetResultAsync(() => _service.AddMessageAsync(model, receiverId, GetCurrentUserId(), GetCurrentDate()));
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Message
        /// </summary>
        /// <param name="id">Message id. Example: 5</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerResponseNoContent]
        public async Task<IActionResult> Delete(long id)
        {
            var mesage = await _service.GetMessageAsync(id);
            var userId = GetCurrentUserId();

            if (mesage.SenderId != userId && mesage.ReceiverId != userId)
                return GenerateErrorResponse(409, "DeletionProhibited", "У вас нет прав для удаления этой записи");
            await _service.DeleteAsync(id);
            return NoContent();
        }
        #endregion
    }
}
