using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsSocialNetwork.Attributes;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Controllers
{
    [Route("api/Comment")]
    public class CommentsController : BaseController
    {
        private readonly ICommentsService _service;

        public CommentsController(ICommentsService service)
        {
            _service = service;
        }

        #region GetAll 
        /// <summary>
        /// Get all Comments
        /// </summary>
        /// <param name="playgroundId">PlaygroundId. Example 2</param>
        /// <param name="skip">number of skipped records. Example 20</param>
        /// <param name="take">number of taken records. Example 20</param>
        /// <returns></returns>
        [HttpGet("{playgroundId}")]
        [SwaggerResponse200(typeof(List<CommentViewModel>))]
        public async Task<IActionResult> Get(long playgroundId, int skip = 0, int take = 20)
        {
            return await GetResultAsync(() => _service.GetAllAsync(playgroundId, skip, take));
        }
        #endregion

        #region Create 
        /// <summary>
        /// Create Comment
        /// </summary>
        /// <param name="model">Comment Model</param>
        /// <param name="playgroundId">PlaygroundId. Example 2</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("{playgroundId}")]
        [SwaggerResponse200(typeof(CommentViewModel))]
        public async Task<IActionResult> Create([FromBody] CommentDtoModel model, long playgroundId)
        {
            return await GetResultAsync(() => _service.CreateAsync(model, GetCurrentUserId(), playgroundId, GetCurrentDate()));
        }
        #endregion

        #region Update 
        /// <summary>
        /// Update Comment
        /// </summary>
        /// <param name="model">Comment Model</param>
        /// <param name="id">CommentId. Example 2</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        [SwaggerResponse200(typeof(List<CommentViewModel>))]
        public async Task<IActionResult> Update([FromBody] CommentDtoModel model, long id)
        {
            var comment = await _service.GetAsync(id);
            if (comment == null) 
                return NotFound();
            var userId = GetCurrentUserId();
            if (userId != comment.AuthorId)
                return GenerateErrorResponse(409, "UpdatingProhibited", "У вас нет прав для редактирования этой записи");
            return await GetResultAsync(() => _service.UpdateAsync(model, id));
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Comment
        /// </summary>
        /// <param name="id">Comment id. Example: 5</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerResponseNoContent]
        public async Task<IActionResult> Delete(long id)
        {
            var comment = await _service.GetAsync(id);
            if (comment == null) 
                return NotFound();
            var userId = GetCurrentUserId();
            if (userId != comment.AuthorId)
                return GenerateErrorResponse(409, "DeletionProhibited", "У вас нет прав для удаления этой записи");
            await _service.DeleteAsync(id);
            return NoContent();
        }
        #endregion
    }
}
