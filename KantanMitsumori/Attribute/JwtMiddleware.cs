using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Model;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace KantanMitsumori.Attribute
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        public LogSession _logSession;
        
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
            _logSession = new LogSession();
        }

        public async Task Invoke(HttpContext context)
        {
            var _logSession = HelperSession.Get<LogSession>(context.Session, CommonConst.KEY_SESSION_ASEST);
            context.Items["Authorized"] = _logSession;
            await _next(context);
        }
    }
}
