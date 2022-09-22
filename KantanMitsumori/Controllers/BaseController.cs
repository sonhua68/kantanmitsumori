﻿using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Model;
using KantanMitsumori.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
            if (_logToken == null && !controllerName.Contains("Home"))
            {
                var ErrorViewModel = new ErrorViewModel()
                {
                    MessageCode = HelperMessage.SMAL020P,
                    MessageContent = KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAL020P)
                };
                filterContext.Result = new RedirectToActionResult("ErrorPage", "Home", ErrorViewModel);
                return;
            }
            _logToken!.sesCustNm_forPrint = GetCookieforPrint(CommonConst.sesCustNm_forPrint);
            _logToken!.sesCustZip_forPrint = GetCookieforPrint(CommonConst.sesCustZip_forPrint);
            _logToken!.sesCustAdr_forPrint = GetCookieforPrint(CommonConst.sesCustAdr_forPrint);
            _logToken!.sesCustTel_forPrint = GetCookieforPrint(CommonConst.sesCustTel_forPrint);
            var resultContext = await next();
        }

        public IActionResult ErrorAction<T>(ResponseBase<T> response)
        {
            return new RedirectToActionResult("ErrorPage", "Home", new ErrorViewModel { MessageCode = response.MessageCode, MessageContent = response.MessageContent });
        }
        /// <summary>
        ///setTokenCookie
        /// </summary>
        /// <param name="token"></param>     
        public void setTokenCookie(string token)
        {
            var currentDate = DateTime.Now;
            var RefreshExpires = _config["JwtSettings:AccessExpires"];
            TimeSpan time = TimeSpan.Parse(RefreshExpires);
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = currentDate.Add(time),
            };
            Response.Cookies.Append(COOKIES, token, cookieOptions);
        }
        /// <summary>
        /// GetCookie
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string GetCookieforPrint(string Key)
        {
            var cookies = Request.Cookies[Key]!;
            if (!string.IsNullOrEmpty(cookies))
            {
                return cookies;
            }
            else
            {
                return "";
            }
        }
    }
}
