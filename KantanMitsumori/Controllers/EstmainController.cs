using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using Microsoft.AspNetCore.Mvc;

using Microsoft.VisualBasic;

namespace KantanMitsumori.Controllers
{

    public class EstmainController : BaseController
    {
        private readonly IAppService _appService;
        private readonly ILogger<HomeController> _logger;
        private readonly IEstimateService _estimateService;

        public EstmainController(IAppService appService, IEstimateService estimateService, IConfiguration config, ILogger<HomeController> logger) : base(config)
        {
            _appService = appService;
            _estimateService = estimateService;
            _logger = logger;
        }
        public IActionResult Header()
        {
            _logToken = new LogToken();
            _logToken.UserNo = "88888195";
            _logToken.UserNm = "test";
            return PartialView("_Header", _logToken);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromQuery] RequestActionModel requestAction, [FromForm] RequestHeaderModel request)
        {
            Uri pageUrl;
            try
            {
                string headRef = Request.Headers["Referer"];
                pageUrl = new Uri(headRef);
            }
            catch (Exception)
            {
                pageUrl = new Uri("http://www.asnet2.com/asest2/test.html");
            }
            var response = new ResponseBase<ResponseEstMainModel>();
            if (Strings.InStr(pageUrl.AbsolutePath, "/asest2/") == 0 || Strings.InStr(pageUrl.AbsolutePath, "/test.htm/") > 0)
                response = await _appService.getEstMain(requestAction, request);
            else
                response = await _appService.setFreeEst();

            // check response result 
            if (response.ResultStatus == (int)enResponse.isError)
                return ErrorAction(response);
            // set cookie access token 
            setTokenCookie(response.Data!.AccessToken);
            return View(response.Data);
        }

    }
}

