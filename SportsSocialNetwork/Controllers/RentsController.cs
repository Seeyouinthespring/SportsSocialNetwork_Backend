using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsSocialNetwork.Attributes;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.Business.Constants;
using SportsSocialNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Controllers
{
    [Route("api/Rent")]
    public class RentsController : BaseController
    {
        private readonly IRentService _service;
        
        public RentsController(IRentService service) 
        {
            _service = service;
        }

        #region Create 
        /// <summary>
        /// Create Rent request
        /// </summary>
        /// <param name="model">Rent request model</param>
        /// <returns></returns>
        [Authorize(Roles = UserRoles.SPORTSMAN)]
        [HttpPost]
        [Route("Request")]
        [SwaggerResponse200(typeof(RentRequestViewModel))]
        public async Task<IActionResult> Create([FromBody]RentRquestDtoModel model)
        {
            var userId = GetCurrentUserId();
            
            return await GetResultAsync(() => _service.AddRentRequestAsync(userId, model));
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Rent request
        /// </summary>
        /// <param name="id">Rent request id. Example: 5</param>
        /// <returns></returns>
        [HttpDelete("Request/{id}")]
        [SwaggerResponseNoContent]
        public async Task<IActionResult> Delete(long id)
        {
            var model = await _service.GetAsync(id);
            if (model == null)
                return NotFound();
            var userId = GetCurrentUserId();
            if (userId != model.Renter.Id)
                GenerateErrorResponse(409, "DeletingProhibited", "У вас нет прав на удаление этой записи");
            await _service.DeleteAsync(id);
            return NoContent();
        }
        #endregion

        #region GetAll 
        /// <summary>
        /// Get Rent requests
        /// </summary>
        /// <returns></returns>
        [HttpGet("Request")]
        [SwaggerResponse200(typeof(List<RentRequestViewModel>))]
        public async Task<IActionResult> Get([FromQuery] long? playgroundId)
        {
            string userId = null;
            if (await GetCurrentUserRole() == UserRoles.LANDLORD)
                userId = GetCurrentUserId();
            
            return await GetResultAsync(() => _service.GetAllAsync(userId, playgroundId));
        }
        #endregion

        #region Approve rent request
        /// <summary>
        /// Create Rent
        /// </summary>
        /// <returns></returns>
        [HttpPost("Approve/{requestId}")]
        [SwaggerResponse200(typeof(List<RentRequestViewModel>))]
        public async Task<IActionResult> Approve(long requestId)
        {
            var userId = GetCurrentUserId();
            var rentRequest = await _service.GetAsync(requestId);
            if (rentRequest == null) return NotFound();
            if (rentRequest.Playground.ApplicationUserId != userId)
                return GenerateErrorResponse(409, "UpdatingProhibited", "У вас нет прав на редактирование этой записи");
            //Добавить проверку на то свободно ли это время
            //проверка на до принадлежит ли площадка этому лендлорду

            return await GetResultAsync(() => _service.ApproveRentRequestAsync(requestId, userId, GetCurrentDate()));
        }
        #endregion

        #region Get rents
        /// <summary>
        /// Get rents
        /// </summary>
        /// <returns></returns>
        [HttpGet("{playgroundId}")]
        [SwaggerResponse200(typeof(List<RentViewModel>))]
        public async Task<IActionResult> GetRents(long playgroundId, DateTime? date, DateTime? startDate, DateTime? endDate)
        {
            return await GetResultAsync(() => _service.GetAllRentsAsync(playgroundId, date, startDate, endDate));
        }
        #endregion


    }
}
