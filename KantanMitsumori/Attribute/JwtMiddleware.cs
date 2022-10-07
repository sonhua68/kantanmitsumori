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
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwtSettings;
        public LogToken _logToken;
        private const string COOKIES = "CookiesASEST";

        public JwtMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings)
        {
            _next = next;
            _jwtSettings = jwtSettings.Value;
            _logToken = new LogToken();
        }

        public async Task Invoke(HttpContext context)
        {
            var cookies = context.Request.Cookies[COOKIES];
            _logToken = HelperToken.EncodingToken(_jwtSettings, cookies!)!;
            context.Items["Authorized"] = _logToken;
            await _next(context);
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
