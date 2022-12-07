using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Models;
using KantanMitsumori.Service.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace KantanMitsumori.Controllers
{

    public class BaseController : Controller
    {     
        private List<string> optionListController = new List<string> { "Home", "Error", "SelCar", "SelGrd", "SerEst" };
        public LogSession? _logSession;
        public BaseController()
        {
            _logSession = new LogSession();
        }
        public string Referer => Request.Headers["Referer"];
        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {        
            _logSession = HelperSession.Get<LogSession>(filterContext.HttpContext.Session, CommonConst.KEY_SESSION_ASEST);
            string actionName = filterContext.RouteData.Values["action"]!.ToString()!;
            string controllerName = filterContext.RouteData.Values["controller"]!.ToString()!;
            if (controllerName == "Home" && actionName == "Index") { RemoveSession(); }
            if ((optionListController.Contains(controllerName)) || (controllerName.Contains("Estmain") && _logSession == null))
            {
                await next();
            }
            else
            {
                if (_logSession == null)
                {
                    RemoveAllCookies();
                    if (!actionName.Contains("Index"))
                        filterContext.Result = ErrorAction();
                    else
                        filterContext.Result = new RedirectToActionResult("ErrorPage", "Error", new RouteValueDictionary(new RequestError
                        {
                            messageCode = HelperMessage.SCOM001S,
                            messageContent = KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SCOM001S)
                        }));
                    return;
                }
                else
                {
                    _logSession!.sesCustNm_forPrint = GetCookieforPrint(filterContext, CommonConst.sesCustNm_forPrint);
                    _logSession!.sesCustZip_forPrint = GetCookieforPrint(filterContext, CommonConst.sesCustZip_forPrint);
                    _logSession!.sesCustAdr_forPrint = GetCookieforPrint(filterContext, CommonConst.sesCustAdr_forPrint);
                    _logSession!.sesCustTel_forPrint = GetCookieforPrint(filterContext, CommonConst.sesCustTel_forPrint);
                    setSession(_logSession);
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
            var response = ResponseHelper.Error<int>(HelperMessage.SCOM001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SCOM001S));
            return Ok(response);
        }
        [Route("[controller]/[action]")]
        public IActionResult ErrorPage(RequestError model)
        {
            var ErrorViewModel = new ErrorViewModel()
            {
                MessageCode = model.messageCode,
                MessageContent = model.messageContent,
                LogSession = _logSession

            };
            return View(ErrorViewModel);
        }    
        public void setSession(LogSession logSession)
        {
            HelperSession.Set<LogSession>(HttpContext.Session, CommonConst.KEY_SESSION_ASEST, logSession);
            HelperSession.Set<string>(HttpContext.Session, CommonConst.KEY_SESSION_USERNO, logSession.UserNo!);
        }
        public void RemoveSession()
        {
            HttpContext.Session.Remove(CommonConst.KEY_SESSION_ASEST);
            HttpContext.Session.Remove(CommonConst.KEY_SESSION_USERNO);
        }
      
        public string GetCookieforPrint(ActionExecutingContext filterContext, string Key)
        {
            var cookies = filterContext.HttpContext.Request.Cookies[Key]!;
            if (!string.IsNullOrEmpty(cookies))
            {         
                return Encoding.UTF8.GetString(Convert.FromBase64String(cookies));
            }
            else
            {
                return "";
            }
        }

        public void RemoveAllCookies()
        {
            RemoveCookies(CommonConst.sesCustNm_forPrint);
            RemoveCookies(CommonConst.sesCustZip_forPrint);
            RemoveCookies(CommonConst.sesCustAdr_forPrint);
            RemoveCookies(CommonConst.sesCustTel_forPrint);
        }       
        public void RemoveCookies(string Key)
        {
            var cookies = Request.Cookies[Key]! ?? "";
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMonths(-1),
            };
            Response.Cookies.Append(Key, cookies, cookieOptions);
        }

    }
}
