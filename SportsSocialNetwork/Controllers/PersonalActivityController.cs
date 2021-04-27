using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsSocialNetwork.Attributes;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Controllers
{
    [Route("api/PersonalActivity")]
    public class PersonalActivityController : BaseController
    {
        private readonly IPersonalActivityService _service;

        public PersonalActivityController(IPersonalActivityService service)
        {
            _service = service;
        }

        #region GetAll 
        /// <summary>
        /// Get all Personal Activities
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse200(typeof(List<PersonalActivityViewModel>))]
        public async Task<IActionResult> Get()
        {
            return await GetResultAsync(() => _service.GetAllAsync(GetCurrentUserId()));
        }
        #endregion

        #region Get by id
        /// <summary>
        /// Get PersonalActivity by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerResponse200(typeof(PersonalActivityViewModel))]
        public async Task<IActionResult> GetById(long id)
        {
            return await GetResultAsync(() => _service.GetAsync(id));
        }
        #endregion

        #region Create 
        /// <summary>
        /// Create PersonalActivity
        /// </summary>
        /// <param name="model">PersonalActivity Model</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [SwaggerResponse200(typeof(PersonalActivityViewModel))]
        public async Task<IActionResult> Create([FromBody] PersonalActivityDtoModel model)
        {
            string userId = GetCurrentUserId();
            return await GetResultAsync(() => _service.CreateAsync(model, userId));
        }
        #endregion

        #region Update 
        /// <summary>
        /// Update Personal Activity
        /// </summary>
        /// <param name="model">PersonalActivity Model</param>
        /// <param name="id">Personal ActivityId. Example 2</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        [SwaggerResponse200(typeof(PersonalActivityViewModel))]
        public async Task<IActionResult> Update([FromBody] PersonalActivityDtoModel model, long id)
        {
            var personalActivity = await _service.GetAsync(id);
            var userId = GetCurrentUserId();
            if (userId != personalActivity.InitiatorId)
                return GenerateErrorResponse(409, "UpdatingProhibited", "У вас нет прав для редактирования этой записи");
            return await GetResultAsync(() => _service.UpdateAsync(model, id, userId));
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Personal Activity
        /// </summary>
        /// <param name="id">PersonalActivity id. Example: 5</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerResponseNoContent]
        public async Task<IActionResult> Delete(long id)
        {
            var personalActivity = await _service.GetAsync(id);
            var userId = GetCurrentUserId();
            if (userId != personalActivity.InitiatorId)
                return GenerateErrorResponse(409, "DeletionProhibited", "У вас нет прав для удаления этой записи");
            await _service.DeleteAsync(id);
            return NoContent();
        }
        #endregion
    }
}
