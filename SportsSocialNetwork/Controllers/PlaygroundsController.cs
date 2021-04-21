﻿using Microsoft.AspNetCore.Authorization;
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
    [Route("api/Playground")]
    public class PlaygroundsController : BaseController
    {
        private readonly IPlaygroundService _service;

        public PlaygroundsController(IPlaygroundService service)
        {
            _service = service;
        }

        #region GetAll 
        /// <summary>
        /// Get all Playgrounds
        /// </summary>
        /// <returns></returns>
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
        [Authorize(Roles = UserRoles.LANDLORD +","+ UserRoles.ADMINISTRATOR)]
        [HttpPost]
        [SwaggerResponse200(typeof(PlaygroundViewModel))]
        public async Task<IActionResult> Create([FromBody]PlaygroundDtoModel model)
        {
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

    }
}
