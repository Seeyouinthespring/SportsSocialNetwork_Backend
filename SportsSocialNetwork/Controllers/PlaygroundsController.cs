using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsSocialNetwork.Attributes;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.Business.Constants;
using SportsSocialNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Controllers
{
    [Route("api/Playground")]
    public class PlaygroundsController : BaseController
    {
        private readonly IPlaygroundService _service;

        public PlaygroundsController(IPlaygroundService service)
        {
            _service = service;
        }

        #region GetAll SHortModel 
        /// <summary>
        /// Get all Playgrounds short models
        /// </summary>
        /// <returns></returns>
        [HttpGet("Short")]
        [SwaggerResponse200(typeof(List<PlaygroundShortViewModel>))]
        public async Task<IActionResult> GetShortModels(PlaygroundQueryModel queryModel)
        {
            return await GetResultAsync(() => _service.GetAllShortModelsAsync(queryModel));
        }
        #endregion

        #region GetAll 
        /// <summary>
        /// Get all Playgrounds
        /// </summary>
        /// <returns> List of short playground models</returns>
        [HttpGet]
        [SwaggerResponse200(typeof(List<PlaygroundViewModel>))]
        public async Task<IActionResult> Get()
        {
            return await GetResultAsync(() => _service.GetAllAsync());
        }
        #endregion

        #region Get by id
        /// <summary>
        /// Get Playground by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerResponse200(typeof(PlaygroundViewModel))]
        public async Task<IActionResult> GetById(long id)
        {
            return await GetResultAsync(() => _service.GetAsync(id));
        }
        #endregion

        #region Create 
        /// <summary>
        /// Create Playground
        /// </summary>
        /// <param name="model">Playground Model</param>
        /// <returns></returns>
        [Authorize(Roles = UserRoles.LANDLORD +","+ UserRoles.SPORTSMAN)]
        [HttpPost]
        [SwaggerResponse200(typeof(PlaygroundViewModel))]
        public async Task<IActionResult> Create([FromBody]PlaygroundDtoModel model)
        {
            if (!this.ModelState.IsValid)
                return GenerateErrorResponse(400, "BadRequest", "Данные введены не верно");
            string userId = null;
            if (await GetCurrentUserRole() == UserRoles.LANDLORD)
                userId = GetCurrentUserId();
            return await GetResultAsync(() => _service.CreateAsync(model, userId));
        }
        #endregion

        #region Update 
        /// <summary>
        /// Update Playground
        /// </summary>
        /// <param name="model">Playground Model</param>
        /// <param name="id">PlaygroundId. Example 2</param>
        /// <returns></returns>
        [Authorize(Roles = UserRoles.LANDLORD + "," + UserRoles.ADMINISTRATOR)]
        [HttpPut("{id}")]
        [SwaggerResponse200(typeof(PlaygroundViewModel))]
        public async Task<IActionResult> Update(PlaygroundDtoModel model, long id)
        {
            string userId = null;
            if (await GetCurrentUserRole() == UserRoles.LANDLORD)
                userId = GetCurrentUserId();
            return await GetResultAsync(() => _service.UpdateAsync(model, id, userId));
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Playground
        /// </summary>
        /// <param name="id">Playground id. Example: 5</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerResponseNoContent]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        #endregion

        #region Approve playground
        /// <summary>
        /// Update Playground status
        /// </summary>
        /// <param name="id">PlaygroundId. Example 2</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("Approve/{id}")]
        [SwaggerResponse200(typeof(PlaygroundViewModel))]
        public async Task<IActionResult> Approve(long id)
        {
            return await GetResultAsync(() => _service.ApproveAsync(id));
        }
        #endregion

        #region Get Playground summary info 
        /// <summary>
        /// Get planned number of visitors on the playground
        /// </summary>
        /// <param name="id">Playground id. Example: 5</param>
        /// <param name="date">Date. Example: 2021-02-02</param>
        /// <param name="time">Time. Example: 19:00:00 </param>
        /// <returns></returns>
        [HttpGet("Visitors/{id}")]
        [SwaggerResponse200(typeof(VisitorsNumberViewModel))]
        public async Task<IActionResult> GetVisitorsNumber(long id, DateTime date, TimeSpan time)
        {
            return await GetResultAsync(() => _service.GetVisitorsNumberAsync(id, date, time));
        }
        #endregion

        #region Get Free timings
        /// <summary>
        /// Get free for rent timings
        /// </summary>
        /// <param name="id">Playground id. Example: 5</param>
        /// <param name="date">Date. Example: </param>
        /// <returns></returns>
        [HttpGet("{id}/Timings/{date}")]
        [SwaggerResponse200(typeof(List<TimingIntervalModel>))]
        public async Task<IActionResult> GetFreeTimings(long id, DateTime date)
        {
            return await GetResultAsync(() => _service.GetFreeTimingsAsync(id, date));
        }
        #endregion

        #region Get Playground summary info
        /// <summary>
        /// Get playground info
        /// </summary>
        /// <param name="id">Playground id. Example: 5</param>
        /// <returns></returns>
        [HttpGet("{id}/SummaryInfo")]
        [SwaggerResponse200(typeof(PlaygroundSummaryInfoViewModel))]
        public async Task<IActionResult> GetSummaryInfo(long id)
        {
            return await GetResultAsync(() => _service.GetSummaryInfoAsync(id));
        }
        #endregion

        #region Update photo
        /// <summary>
        /// Update Playground
        /// </summary>
        /// <param name="id">PlaygroundId. Example 2</param>
        /// <param name="file">Photo file. Example 2</param>
        /// <returns></returns>
        //[Authorize(Roles = UserRoles.LANDLORD + "," + UserRoles.ADMINISTRATOR)]
        [HttpPut("{id}/Photo")]
        [SwaggerResponse200(typeof(PlaygroundViewModel))]
        public async Task<IActionResult> Update([FromQuery][Required]IFormFile file, long id)
        {
            byte[] fileBytes;
            //string userId = null;
            //if (await GetCurrentUserRole() == UserRoles.LANDLORD)
            //    userId = GetCurrentUserId();
            using (var ms = new MemoryStream()) 
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            await _service.UpdatePhotoAsync(fileBytes, id);
            return Ok();
        }
        #endregion

        #region Get Playgrounds for map
        /// <summary>
        /// Get playgrounds for map
        /// </summary>
        /// <returns></returns>
        [HttpGet("MapObjects")]
        [SwaggerResponse200(typeof(PlaygroundMapViewModel))]
        public async Task<IActionResult> GetPlaygroundsForMap()
        {
            return await GetResultAsync(() => _service.GetPlaygroundsForMapAsync());
        }
        #endregion
    }
}
