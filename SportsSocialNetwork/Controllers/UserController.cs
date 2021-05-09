using SportsSocialNetwork.DataBaseModels;
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
using SportsSocialNetwork.Helpers;
using SportsSocialNetwork.Attributes;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SportsSocialNetwork.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : BaseController
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
                    Code = "Unauthorized",
                    Message = "You have entred incorrect data"
                });
            }
            if (user.LockoutEnd != null || user.LockoutEnd >= DateTime.Today)
            {
                HttpContext.Response.StatusCode = 409;
                return new JsonResult(new Response
                {
                    Code = "Unauthorized",
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
                expires: DateTime.Now.AddHours(5),
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
                    Code = "Conflict",
                    Message = "Such UserName has been already taken!"
                });
            }
            userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                HttpContext.Response.StatusCode = 409;
                return new JsonResult(new Response
                {
                    Code = "Conflict",
                    Message = "Such Email has been already taken!"
                });
            }

            if (!await roleManager.RoleExistsAsync(UserRoles.SPORTSMAN))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.SPORTSMAN));
            if (!await roleManager.RoleExistsAsync(UserRoles.LANDLORD))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.LANDLORD));

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                //FullName = model.FullName,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                //CreatedDate = GetCurrentDate(),
                //RoleId = role.Id
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                HttpContext.Response.StatusCode = 400;
                return new JsonResult(new Response
                {
                    Code = "BadReques",
                    Message = "User creation failed! Please check input data"
                });
            }

            if (await roleManager.RoleExistsAsync(model.Role))
                await userManager.AddToRoleAsync(user, model.Role);
            
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
                    Code = "Conflict",
                    Message = "Such UserName has been already taken!"
                });
            }
            userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                HttpContext.Response.StatusCode = 409;
                return new JsonResult(new Response
                {
                    Code = "Conflict",
                    Message = "Such Email has been already taken!"
                });
            }

            if (!await roleManager.RoleExistsAsync(UserRoles.ADMINISTRATOR))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.ADMINISTRATOR));

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                FirstName = model.FirstName,
                //FullName = model.FullName,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                //CreatedDate = GetCurrentDate(),
                //RoleId = role.Id
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                HttpContext.Response.StatusCode = 400;
                return new JsonResult(new Response
                {
                    Code = "BadReques",
                    Message = "User creation failed! Please check input data"
                });
            }

            if (await roleManager.RoleExistsAsync(UserRoles.ADMINISTRATOR))
                await userManager.AddToRoleAsync(user, UserRoles.ADMINISTRATOR);

            return await Login(new LoginModel
            {
                UserName = user.UserName,
                Password = model.Password
            });
        }

        /// <summary>
        /// Update profile photo
        /// </summary>
        /// <param name="model">Photo file model</param>
        /// <returns></returns>
        [HttpPut]
        [Route("Photo")]
        public async Task<IActionResult> UpdatePhoto(PhotoModel model) 
        {
            await _userService.UpdatePhotoAsync(model, GetCurrentUserId());
            return NoContent();
        }

        /// <summary>
        /// Update profile photo
        /// </summary>
        /// <param name="file">Photo file</param>
        /// <returns></returns>
        [HttpPut]
        [Route("PhotoFile")]
        public async Task<IActionResult> UpdatePhotoFile([FromQuery][Required] IFormFile file)
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

            await _userService.UpdatePhotoAsync(fileBytes, GetCurrentUserId());
            return Ok();
        }

        /// <summary>
        /// Update profile photo
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("Profile")]
        [SwaggerResponse200(typeof(ProfileViewModel))]
        public async Task<IActionResult> GetProfile()
        {
            return await GetResultAsync(() => _userService.GetProfileAsync(GetCurrentUserId(), GetCurrentDate()));
        }
    }
}
