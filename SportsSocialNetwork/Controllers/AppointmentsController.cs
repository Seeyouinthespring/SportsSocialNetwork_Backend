using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsSocialNetwork.Attributes;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.Business.Constants;
using SportsSocialNetwork.Helpers;
using SportsSocialNetwork.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Controllers
{
    [Route("api/Appointment")]
    public class AppointmentsController : BaseController
    {
        private readonly IAppointmentsService _service;

        public AppointmentsController(IAppointmentsService service)
        {
            _service = service;
        }

        #region GetAll 
        /// <summary>
        /// Get all Appointments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse200(typeof(List<AppointmentShortViewModel>))]
        public async Task<IActionResult> Get(bool? isActual, int? sportId, int? playgroundId )
        {
            return await GetResultAsync(() => _service.GetAllAsync(isActual, sportId, playgroundId, GetCurrentUserId(), GetCurrentDate()));
        }
        #endregion

        #region Get appointments for playground
        /// <summary>
        /// Get all Appointments
        /// </summary>
        /// <param name="id">Playground Id. Example 2</param>
        /// <returns></returns>
        [HttpGet("Playground/{id}")]
        [SwaggerResponse200(typeof(List<AppointmentShortViewModel>))]
        public async Task<IActionResult> GetForPlayground(long id)
        {
            return await GetResultAsync(() => _service.GetForPlaygroundAsync(id, GetCurrentDate(), GetCurrentUserId()));
        }
        #endregion

        #region Get by id
        /// <summary>
        /// Get Appointment by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerResponse200(typeof(SingleAppointmentViewModel))]
        public async Task<IActionResult> GetById(long id)
        {
            return await GetResultAsync(() => _service.GetAsync(id));
        }
        #endregion

        #region Create 
        /// <summary>
        /// Create Appointment
        /// </summary>
        /// <param name="model">Appointment Model</param>
        /// <returns></returns>
        [Authorize(Roles = UserRoles.SPORTSMAN)]
        [HttpPost]
        [SwaggerResponse200(typeof(AppointmentViewModel))]
        [AppointmentAdditingConflict]
        public async Task<IActionResult> Create([FromBody] AppointmentDtoModel model)
        {
            if (!ModelState.IsValid) 
            {

                HttpContext.Response.StatusCode = 400;
                return ValidationModelHelper.GenerateErrorMessage(ModelState);
            }
            string userId = GetCurrentUserId();

            AppointmentAdditingError result = await _service.CreateAsync(model, userId);
            return (result) switch
            {
                AppointmentAdditingError.WrongSport => GenerateErrorResponse(409, AppointmentAdditingConflictAttribute.INCORRECT_SPORT, AppointmentAdditingConflictAttribute.Errors[AppointmentAdditingConflictAttribute.INCORRECT_SPORT]),
                AppointmentAdditingError.WrongPlayground => GenerateErrorResponse(409, "wrongPlayground", "Данное действие нельзя совершить для коммерческой площадки"),
                AppointmentAdditingError.WrongTimeInterval => GenerateErrorResponse(409, "wrongTimings", "Нельзя назначить активность в этот период времени"),
                AppointmentAdditingError.DuplicateAppointment => GenerateErrorResponse(409, "DuplicateAppointment", "У вас уже есть запланированная встреча на это время"),
                AppointmentAdditingError.Ok => Ok(),
                _ => Ok(),
            };
        }
        #endregion

        #region Update 
        /// <summary>
        /// Update Appointment
        /// </summary>
        /// <param name="model">Appointment Model</param>
        /// <param name="id">AppointmentId. Example 2</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        [SwaggerResponse200(typeof(SingleAppointmentViewModel))]
        public async Task<IActionResult> Update([FromBody] AppointmentDtoModel model, long id)
        {
            var appointment = await _service.GetAsync(id);
            var userId = GetCurrentUserId();
            if (userId != appointment.InitiatorId)
                return GenerateErrorResponse(409, "UpdatingProhibited", "У вас нет прав для редактирования этой записи");
            return await GetResultAsync(() => _service.UpdateAsync(model, id, userId));
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Appointment
        /// </summary>
        /// <param name="id">Appointment id. Example: 5</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerResponseNoContent]
        public async Task<IActionResult> Delete(long id)
        {
            var appointment = await _service.GetAsync(id);
            var userId = GetCurrentUserId();
            if (userId != appointment.InitiatorId)
                return GenerateErrorResponse(409, "DeletionProhibited", "У вас нет прав для удаления этой записи");
            await _service.DeleteAsync(id);
            return NoContent();
        }
        #endregion
    }
}
