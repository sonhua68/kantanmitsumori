using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Model;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace KantanMitsumori.Attribute
{
    public class ActionFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CommonSettings _commonSettings;
        public LogToken _logToken;
        private List<string> optionListController = new List<string> { "Home", "SelCar", "SelGrd", "SerEst" };
        private const string COOKIES = "CookiesASEST";

        public ActionFilterMiddleware(RequestDelegate next, IOptions<CommonSettings> commonSettings)
        {
            _next = next;
            _commonSettings = commonSettings.Value;
            _logToken = new LogToken();
        }

        public async Task Invoke(HttpContext context)
        {
            var pramQuery = context.Request.Query.Count == 0;
            var cookies = context.Request.Cookies[COOKIES];
            string actionName = context.GetRouteValue("action")!.ToString()!;
            string controllerName = context.GetRouteValue("controller")!.ToString()!;

            if (optionListController.Contains(controllerName) || (controllerName.Contains("Estmain") && pramQuery))
                await _next(context);
            else
            {
                //  TO DO append commonSettings
                _logToken = HelperToken.EncodingToken(_commonSettings.JwtSettings, cookies!)!;

                if (_logToken == null)
                {
                    var response = ResponseHelper.Error<int>(HelperMessage.SMAI001P, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI001P));
                    var json = JsonConvert.SerializeObject(response);

                    if (!actionName.Contains("Index"))
                    {
                        context.Response.StatusCode = 200;
                    }
                    else
                    {
                        context.Response.StatusCode = 308;
                        context.Response.Headers[HeaderNames.Location] = $"/Error/ErrorPage";
                    }

                    using (var bodyWriter = new StreamWriter(context.Response.Body))
                    {
                        bodyWriter.Write(json);
                        bodyWriter.Flush();
                    }
                }
                else if (_logToken != null)
                {
                    _logToken!.sesCustNm_forPrint = GetCookieforPrint(context, CommonConst.sesCustNm_forPrint);
                    _logToken!.sesCustZip_forPrint = GetCookieforPrint(context, CommonConst.sesCustZip_forPrint);
                    _logToken!.sesCustAdr_forPrint = GetCookieforPrint(context, CommonConst.sesCustAdr_forPrint);
                    _logToken!.sesCustTel_forPrint = GetCookieforPrint(context, CommonConst.sesCustTel_forPrint);
                }

                await _next(context);
            }
        }

        public string GetCookieforPrint(HttpContext context, string Key)
        {
            var cookies = context.Request.Cookies[Key]!;
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
