using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Model;
using KantanMitsumori.Models;
using KantanMitsumori.Service.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Configuration;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace KantanMitsumori.Controllers
{
  
    public class BaseController : Controller
    {
        private const string COOKIES = "CookiesASEST";
        public IConfiguration _config;
        public LogToken _logToken;
        public BaseController(IConfiguration config)
        {
            _config = config;
            _logToken = new LogToken();

        }
        [Route("[controller]/[action]")]
        public IActionResult ErrorPage([FromQuery] string messageCode, string messageContent)
        {
            var ErrorViewModel = new ErrorViewModel()
            {
                MessageCode = messageCode,
                MessageContent = messageContent
            };
            return View(ErrorViewModel);
        } 
        
        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {

            var cookies = Request.Cookies[COOKIES]!;
            string actionName = filterContext.RouteData.Values["action"]!.ToString()!;
            string controllerName = filterContext.RouteData.Values["controller"]!.ToString()!;
            if (!controllerName.Contains("Home"))
            {
                if (string.IsNullOrEmpty(cookies))
                {
                    var ErrorViewModel = new ErrorViewModel()
                    {
                        MessageCode = HelperMessage.SMAL020P,
                        MessageContent = KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAL020P)
                    };
                    filterContext.Result = new RedirectToActionResult("ErrorPage", "Home", ErrorViewModel);
                    return;

                }               
            }
            _logToken = HelperToken.EncodingToken(cookies!)!;
            var resultContext = await next();
        }

        public IActionResult ErrorAction<T>(ResponseBase<T> response)
        {
            return RedirectToAction("ErrorPage", "Home", new ErrorViewModel { MessageCode = response.MessageCode, MessageContent = response.MessageContent });
        }
        /// <summary>
        ///setTokenCookie
        /// </summary>
        /// <param name="token"></param>     
        public void setTokenCookie(string token)
        {
            var currentDate = DateTime.Now;
            var RefreshExpires = _config["JwtSettings:RefreshExpires"];
            TimeSpan time = TimeSpan.Parse(RefreshExpires);
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = currentDate.Add(time),
            };
            Response.Cookies.Append(COOKIES, token, cookieOptions);
        }


    }
}
