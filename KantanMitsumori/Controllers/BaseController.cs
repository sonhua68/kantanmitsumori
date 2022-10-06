using GrapeCity.ActiveReports;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Models;
using KantanMitsumori.Service.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace KantanMitsumori.Controllers
{

    public class BaseController : Controller
    {
        private const string COOKIES = "CookiesASEST";        
        public CommonSettings _commonSettings;
        private List<string> optionListController = new List<string> { "Home", "SelCar", "SelGrd", "SerEst" };
        public IConfiguration _config;
        public LogToken _logToken;

        public BaseController(IConfiguration config)
        {            
            _commonSettings = new CommonSettings(config);
            _logToken = new LogToken();

        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            var pramQuery = Request.Query.Count == 0;
            var cookies = Request.Cookies[COOKIES]!;
            string actionName = filterContext.RouteData.Values["action"]!.ToString()!;
            string controllerName = filterContext.RouteData.Values["controller"]!.ToString()!;
            if ((optionListController.Contains(controllerName)) || (controllerName.Contains("Estmain") && pramQuery))
            {
                await next();
            }
            else
            {
                _logToken = HelperToken.EncodingToken(_commonSettings.JwtSettings, cookies!)!;
                if (_logToken == null)
                {
                    if (!actionName.Contains("Index"))
                        filterContext.Result = ErrorAction();
                    else
                        filterContext.Result = new RedirectToActionResult("ErrorPage", "Error", new RouteValueDictionary(new RequestError
                        {
                            messageCode = HelperMessage.SMAI001P,
                            messageContent = KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI001P)
                        }));
                    return;
                }
                else if (_logToken != null)
                {
                    _logToken!.sesCustNm_forPrint = GetCookieforPrint(CommonConst.sesCustNm_forPrint);
                    _logToken!.sesCustZip_forPrint = GetCookieforPrint(CommonConst.sesCustZip_forPrint);
                    _logToken!.sesCustAdr_forPrint = GetCookieforPrint(CommonConst.sesCustAdr_forPrint);
                    _logToken!.sesCustTel_forPrint = GetCookieforPrint(CommonConst.sesCustTel_forPrint);
                }
                await next();
            }

        }

        public IActionResult ErrorAction<T>(ResponseBase<T> response, int isUnexpectedErr = 0)
        {
            if (isUnexpectedErr != 1)

                return new RedirectToActionResult("ErrorPage", "Error", new RouteValueDictionary(new RequestError
                {
                    messageCode = response.MessageCode,
                    messageContent = response.MessageContent
                }));
            else
                return new RedirectToActionResult("ErrorPage", "Error", new RouteValueDictionary(new RequestError
                {
                    messageCode = HelperMessage.ISYS010I,
                    messageContent = KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I)
                }));
        }

        public IActionResult ErrorAction()
        {
            var response = ResponseHelper.Error<int>(HelperMessage.SMAI001P, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI001P));
            return Ok(response);
        }
        [Route("[controller]/[action]")]
        public IActionResult ErrorPage(RequestError model)
        {
            var ErrorViewModel = new ErrorViewModel()
            {
                MessageCode = model.messageCode,
                MessageContent = model.messageContent
            };
            return View(ErrorViewModel);
        }
        /// <summary> 
        ///setTokenCookie
        /// </summary>
        /// <param name="token"></param>     
        public void setTokenCookie(string token)
        {
            var currentDate = DateTime.Now;            
            var RefreshExpires = _commonSettings.JwtSettings.AccessExpires;
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
