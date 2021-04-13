﻿using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Business.Constants;
using SportsSocialNetwork.Business.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using SportsSocialNetwork.Interfaces;

namespace SportsSocialNetwork.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public UserController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IUserService userService
            ) : base()
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _userService = userService;
        }

        /// <summary>
        /// Login in the system
        /// </summary>
        /// <returns></returns>
        //[Swagger200(typeof(ApplicationUserEnterViewModel))]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            //var user = await _userService.GetUserByNameAsync(model.UserName);
            if (user == null || await userManager.CheckPasswordAsync(user, model.Password) == false)
            {
                HttpContext.Response.StatusCode = 401;
                return new JsonResult(new Response
                {
                    Status = "Unauthorized",
                    Message = "You have entred incorrect data"
                });
            }
            if (user.LockoutEnd != null || user.LockoutEnd >= DateTime.Today)
            {
                HttpContext.Response.StatusCode = 409;
                return new JsonResult(new Response
                {
                    Status = "Unauthorized",
                    Message = @$"Your account has been locked: {((TimeSpan)(user.LockoutEnd - DateTime.Today)).TotalDays } days left"
                });
            }
            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenAuthentication:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["TokenAuthentication:Issuer"],
                audience: _configuration["TokenAuthentication:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });

            //var result = user.MapTo<ApplicationUserEnterViewModel>();
            //result.Expiration = token.ValidTo;
            //result.Token = new JwtSecurityTokenHandler().WriteToken(token);
            //result.Role = userRoles.First();

            //return Ok(result);
        }

        /// <summary>
        /// Register in the system
        /// </summary>
        /// <returns></returns>
        //[Swagger200(typeof(ApplicationUserEnterViewModel))]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
            {
                HttpContext.Response.StatusCode = 409;
                return new JsonResult(new Response
                {
                    Status = "Conflict",
                    Message = "Such UserName has been already taken!"
                });
            }
            userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                HttpContext.Response.StatusCode = 409;
                return new JsonResult(new Response
                {
                    Status = "Conflict",
                    Message = "Such Email has been already taken!"
                });
            }

            if (!await roleManager.RoleExistsAsync(UserRoles.PLAYER))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.PLAYER));
            if (!await roleManager.RoleExistsAsync(UserRoles.LANDLORD))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.LANDLORD));

            var role = await roleManager.FindByNameAsync(UserRoles.PLAYER);

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                //FullName = model.FullName,
                //Gender = model.Gender,
                //BirthDate = model.DateOfBirth.DateTime,
                //CreatedDate = GetCurrentDate(),
                //RoleId = role.Id
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                HttpContext.Response.StatusCode = 400;
                return new JsonResult(new Response
                {
                    Status = "BadReques",
                    Message = "User creation failed! Please check input data"
                });
            }

            if (await roleManager.RoleExistsAsync(UserRoles.PLAYER))
                await userManager.AddToRoleAsync(user, UserRoles.PLAYER);
            

            return await Login(new LoginModel
            {
                UserName = user.UserName,
                Password = model.Password
            });
        }

        /// <summary>
        /// Register as admin in the system
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
            {
                HttpContext.Response.StatusCode = 409;
                return new JsonResult(new Response
                {
                    Status = "Conflict",
                    Message = "Such UserName has been already taken!"
                });
            }
            userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                HttpContext.Response.StatusCode = 409;
                return new JsonResult(new Response
                {
                    Status = "Conflict",
                    Message = "Such Email has been already taken!"
                });
            }

            if (!await roleManager.RoleExistsAsync(UserRoles.ADMINISTRATOR))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.ADMINISTRATOR));

            var role = await roleManager.FindByNameAsync(UserRoles.ADMINISTRATOR);

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                //FullName = model.FullName,
                //Gender = model.Gender,
                //BirthDate = model.DateOfBirth.DateTime,
                //CreatedDate = GetCurrentDate(),
                //RoleId = role.Id
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                HttpContext.Response.StatusCode = 400;
                return new JsonResult(new Response
                {
                    Status = "BadReques",
                    Message = "User creation failed! Please check input data"
                });
            }

            if (await roleManager.RoleExistsAsync(UserRoles.ADMINISTRATOR))
            {
                await userManager.AddToRoleAsync(user, UserRoles.ADMINISTRATOR);
            }

            return await Login(new LoginModel
            {
                UserName = user.UserName,
                Password = model.Password
            });
        }
    }
}
