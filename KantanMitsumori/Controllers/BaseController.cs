using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Models;
using KantanMitsumori.Service.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using static GrapeCity.Enterprise.Data.DataEngine.ExpressionEvaluation.Eval;

namespace KantanMitsumori.Controllers
{

    public class BaseController : Controller
    {
        private const string COOKIES = "CookiesASEST";
        private List<string> optionListController = new List<string> { "Home","Error", "SelCar", "SelGrd", "SerEst" };
        public LogToken? _logToken;

        public BaseController()
        {
            _logToken = new LogToken();

        }

        public string Referer => Request.Headers["Referer"];

        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {      
            var cookies = filterContext.HttpContext.Request.Cookies[COOKIES];
            _logToken = filterContext.HttpContext.Items["Authorized"] as LogToken;
            string actionName = filterContext.RouteData.Values["action"]!.ToString()!;
            string controllerName = filterContext.RouteData.Values["controller"]!.ToString()!;
           if ((optionListController.Contains(controllerName)) || (controllerName.Contains("Estmain") && cookies == null))
            {
                await next();
            }
            else
            {

                if (_logToken == null)
                {
                    RemoveCookies(COOKIES);
                    RemoveCookies(CommonConst.sesCustNm_forPrint);
                    RemoveCookies(CommonConst.sesCustZip_forPrint);
                    RemoveCookies(CommonConst.sesCustAdr_forPrint);
                    RemoveCookies(CommonConst.sesCustTel_forPrint);
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
                else
                {
                    _logToken!.sesCustNm_forPrint =  GetCookieforPrint(filterContext, CommonConst.sesCustNm_forPrint);
                    _logToken!.sesCustZip_forPrint = GetCookieforPrint(filterContext,CommonConst.sesCustZip_forPrint);
                    _logToken!.sesCustAdr_forPrint = GetCookieforPrint(filterContext,CommonConst.sesCustAdr_forPrint);
                    _logToken!.sesCustTel_forPrint = GetCookieforPrint(filterContext,CommonConst.sesCustTel_forPrint);
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
                    messageContent = response.MessageContent,

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
                MessageContent = model.messageContent,
                logToken = _logToken
                
            };
            return View(ErrorViewModel);
        }
        /// <summary> 
        ///setTokenCookie
        /// </summary>
        /// <param name="token"></param>     
        public void setTokenCookie(string accessExp, string token)
        {
            //var currentDate = DateTime.Now;         
            //var refreshExpires = accessExp;
            //TimeSpan time = TimeSpan.Parse(refreshExpires);
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                //Expires = currentDate.Add(time),
            };
            Response.Cookies.Append(COOKIES, token, cookieOptions);
        }
        /// <summary>
        /// GetCookie
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string GetCookieforPrint(ActionExecutingContext filterContext,string Key)
        {
            var cookies = filterContext.HttpContext.Request.Cookies[Key]!;
            if (!string.IsNullOrEmpty(cookies))
            {
                return cookies;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Remove Cookies
        /// </summary>
        /// <param name="Key"></param>
        public void RemoveCookies(string Key)
        {
            var cookies = Request.Cookies[Key]!??"";
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMonths(-1),
            };
            Response.Cookies.Append(Key, cookies, cookieOptions);
        }
        
    }
}
