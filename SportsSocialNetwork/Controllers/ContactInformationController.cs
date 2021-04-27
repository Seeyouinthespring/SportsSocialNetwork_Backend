using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsSocialNetwork.Attributes;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.Business.Constants;
using SportsSocialNetwork.Interfaces;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Controllers
{
    [Route("api/ContactInfo")]
    public class ContactInformationController : BaseController
    {
        private readonly IContactInformationService _service;
        private readonly IPlaygroundService _playgroundService;

        public ContactInformationController(IContactInformationService service, IPlaygroundService playgroundService)
        {
            _service = service;
            _playgroundService = playgroundService;
        }

        #region Create 
        /// <summary>
        /// Create ContactInformation
        /// </summary>
        /// <param name="model">ContactInformation Model</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [SwaggerResponse200(typeof(ContactInformationViewModel))]
        public async Task<IActionResult> Create([FromBody] ContactInformationDtoModel model)
        {
            string role = await GetCurrentUserRole();
            string userId = GetCurrentUserId();
            if (model.PlaygroundId != null)
            {
                var playground = await _playgroundService.GetAsync(model.PlaygroundId.Value);
                if (playground == null) 
                    return NotFound();
                if (playground.ApplicationUserId != userId && role == UserRoles.LANDLORD || 
                    playground.ApplicationUserId == userId && role == UserRoles.ADMINISTRATOR || 
                    role == UserRoles.SPORTSMAN)
                        return GenerateErrorResponse(409, "CreatingProhibited", "У вас нет прав для данного действия");
            }
            if (model.ApplicationUserId != userId)
                return GenerateErrorResponse(409, "CreatingProhibited", "У вас нет прав для данного действия");

            return await GetResultAsync(() => _service.CreateAsync(model));
        }
        #endregion

        #region Update 
        /// <summary>
        /// Update ContactInformation
        /// </summary>
        /// <param name="model">ContactInformation Model</param>
        /// <param name="id">ContactInformationId. Example 2</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        [SwaggerResponse200(typeof(ContactInformationViewModel))]
        public async Task<IActionResult> Update([FromBody] ContactInformationDtoModel model, long id)
        {
            string role = await GetCurrentUserRole();
            string userId = GetCurrentUserId();
            var contactInfo = await _service.GetAsync(id);
            if (contactInfo == null) return NotFound();
            if (contactInfo.PlaygroundId != null && 
                (contactInfo.Playground.ApplicationUserId != userId && role == UserRoles.LANDLORD ||
                contactInfo.Playground.ApplicationUserId == userId && role == UserRoles.ADMINISTRATOR || role == UserRoles.SPORTSMAN))
                    return GenerateErrorResponse(409, "UpdationProhibited", "У вас нет прав для данного действия");
            if (contactInfo.ApplicationUserId != userId)
                return GenerateErrorResponse(409, "UpdationProhibited", "У вас нет прав для данного действия");
            return await GetResultAsync(() => _service.UpdateAsync(model, id));
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Contact information
        /// </summary>
        /// <param name="id">Contact information id. Example: 5</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerResponseNoContent]
        public async Task<IActionResult> Delete(long id)
        {
            string role = await GetCurrentUserRole();
            string userId = GetCurrentUserId();
            var contactInfo = await _service.GetAsync(id);
            if (contactInfo == null) return NotFound();
            if (contactInfo.PlaygroundId != null &&
                (contactInfo.Playground.ApplicationUserId != userId && role == UserRoles.LANDLORD ||
                contactInfo.Playground.ApplicationUserId == userId && role == UserRoles.ADMINISTRATOR || role == UserRoles.SPORTSMAN))
                return GenerateErrorResponse(409, "DeletionProhibited", "У вас нет прав для данного действия");
            if (contactInfo.ApplicationUserId != userId)
                return GenerateErrorResponse(409, "DeletionProhibited", "У вас нет прав для данного действия");

            await _service.DeleteAsync(id);
            return NoContent();
        }
        #endregion
    }
}
