using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SportsSocialNetwork.Attributes;
using SportsSocialNetwork.Interfaces;
using System.Threading.Tasks;
using SportsSocialNetwork.Business.BusinessModels;

namespace SportsSocialNetwork.Controllers
{
    [Authorize]
    [Route("api/Visiting")]
    public class VisitingsController : BaseController
    {
        private readonly IVisitingsService _service;
        private readonly IAppointmentsService _appointmentsService;

        public VisitingsController(IVisitingsService service, IAppointmentsService appointmentsService)
        {
            _service = service;
            _appointmentsService = appointmentsService;
        }

        #region Create 
        /// <summary>
        /// Create Appointment visiting
        /// </summary>
        /// <param name="appointmentId">Appointment Id. Example: 2</param>
        /// <returns></returns>
        [HttpPost("{appointmentId}")]
        [SwaggerResponse200(typeof(VisitingBaseModel))]
        public async Task<IActionResult> Create(long appointmentId)
        {
            var userId = GetCurrentUserId();
            var appointment = await _appointmentsService.GetAsync(appointmentId);
            if (appointment.InitiatorId == userId)
                GenerateErrorResponse(409, "AdditingProhibited", "Вы не можете добавить запись на встречу, созданную вами");
            var model = await _service.GetVisitingByAppointmentAsync(appointmentId, userId);
            if (model != null)
                GenerateErrorResponse(409, "AdditingProhibited", "У вас уже существует запись на эту встречу");

            return await  GetResultAsync(()=>  _service.CreateAsync(appointmentId, userId));
        }
        #endregion

        #region Update 
        /// <summary>
        /// Update Appointment visiting status
        /// </summary>
        /// <param name="appointmentId">AppointmentId. Example 2</param>
        /// <returns></returns>
        [HttpPut("{appointmentId}")]
        [SwaggerResponse200(typeof(VisitingBaseModel))]
        public async Task<IActionResult> Update(long appointmentId)
        {
            //var model = await _service.GetVisitingByIdAsync(appointmentId);
            //if (model == null) 
            //    return NotFound();
            var userId = GetCurrentUserId();
            //if (userId != model.MemberId)
            //    GenerateErrorResponse(409, "UpdatingProhibited", "У вас нет прав на редактирование этой записи");
            return await GetResultAsync(() => _service.UpdateStatusAsync(appointmentId, userId));
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Appointment Visiting
        /// </summary>
        /// <param name="appointmentId">Appointment visiting id. Example: 5</param>
        /// <returns></returns>
        [HttpDelete("{appointmentId}")]
        [SwaggerResponseNoContent]
        public async Task<IActionResult> Delete(long appointmentId)
        {
            //var model = await _service.GetVisitingByIdAsync(id);
            //if (model == null)
            //    return NotFound();
            var userId = GetCurrentUserId();
            //if (userId != model.MemberId)
            //    GenerateErrorResponse(409, "DeletingProhibited", "У вас нет прав на удаление этой записи");
            await _service.DeleteAsync(appointmentId, userId);
            return NoContent();
        }
        #endregion

    }
}
