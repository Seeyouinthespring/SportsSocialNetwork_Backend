using Microsoft.AspNetCore.Mvc;
using SportsSocialNetwork.Business.BusinessModels;
using SportsSocialNetwork.Helpers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Controllers
{
    public class BaseController : ControllerBase
    {
        protected BaseController()
        {
        }

        internal string GetCurrentUserId()
        {
            return TokenHelper.GetCurrentUserId(Request);
        }

        protected async Task<IActionResult> GetResultAsync<T>([NotNull] Func<Task<T>> getDataFunction)
        {
            T result = await getDataFunction();

            if (result == null)
            {
                return NotFound();
            }

            return new JsonResult(result);
        }

        internal DateTime GetCurrentDate()
        {
            string dateFromRequest = Request.Headers["Current-Date"].ToString();
            if (string.IsNullOrEmpty(dateFromRequest))
                return DateTime.Now;
            return DateTime.TryParse(dateFromRequest, null, DateTimeStyles.AdjustToUniversal, out var currentDate) ? currentDate : DateTime.Now;
        }

        internal JsonResult GenerateErrorResponse(int status, string code, string message = null)
        {
            HttpContext.Response.StatusCode = status;
            return new JsonResult(new Response
            {
                Code = code,
                Message = message
            });
        }

    }
}
