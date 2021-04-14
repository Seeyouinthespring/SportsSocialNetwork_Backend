using Microsoft.AspNetCore.Mvc;
using SportsSocialNetwork.Attributes;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Controllers
{
    [Route("api/Sport")]
    public class SportsController : BaseController
    {
        private readonly ISportsService _service;

        public SportsController(ISportsService service)
        {
            _service = service;
        }

        #region GetAll 
        /// <summary>
        /// Get all sports
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse200(typeof(List<SportViewModel>))]
        public async Task<IActionResult> Get()
        {
            return await GetResultAsync(() => _service.GetAllAsync());
        }
        #endregion

        #region Get by id
        /// <summary>
        /// Get sport by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerResponse200(typeof(SportViewModel))]
        public async Task<IActionResult> GetById(long id)
        {
            return await GetResultAsync(() => _service.GetAsync(id));
        }
        #endregion

        #region Create 
        /// <summary>
        /// Create sport
        /// </summary>
        /// <param name="model">Sport Model</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse200(typeof(SportViewModel))]
        public async Task<IActionResult> Create(SportDtoModel model)
        {
            return await GetResultAsync(() => _service.CreateAsync(model));
        }
        #endregion

        #region Update 
        /// <summary>
        /// Update sport
        /// </summary>
        /// <param name="model">Sport Model</param>
        /// <param name="id">SportId. Example 2</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerResponse200(typeof(SportViewModel))]
        public async Task<IActionResult> Update(SportDtoModel model, long id)
        {
            return await GetResultAsync(() => _service.UpdateAsync(model, id));
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete sport
        /// </summary>
        /// <param name="id">Sport id. Example: 5</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerResponseNoContent]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        #endregion
    }
}
