using KantanMitsumori.Model;
using KantanMitsumori.Models;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class BaseController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string messageCode, string messageContent)
        {
            return View(new ErrorViewModel { MessageCode = messageCode, MessageContent = messageContent });
        }

        public IActionResult ErrorAction<T>(ResponseBase<T> response)
        {
            return RedirectToAction("Error", new ErrorViewModel { MessageCode = response.MessageCode, MessageContent = response.MessageContent });
        }      
        public void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

    }
}
