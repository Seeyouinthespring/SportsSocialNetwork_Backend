using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using SportsSocialNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Helpers
{
    internal static class TokenHelper
    {
        internal static string GetCurrentUserId(HttpRequest request)
        {
            string userName = GetCurrentUserName(request);
            if (string.IsNullOrEmpty(userName)) return null;

            var loginService = request.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;

            //return "123";
            return loginService.GetUserByNameAsync(userName).Id;
        }

        internal static string GetCurrentUserName(HttpRequest request)
        {
            if (request == null) return string.Empty;

            var token = GetUserToken(request.Headers["Authorization"].ToString());
            if (string.IsNullOrEmpty(token)) return null;

            return JwtManager.GetUserNameFromToken(token);
        }

        private static string GetUserToken(string authorizationHeader)
        {
            return authorizationHeader.Replace(JwtBearerDefaults.AuthenticationScheme, string.Empty).TrimStart();
        }
    }
}
