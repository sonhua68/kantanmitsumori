using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using Microsoft.AspNetCore.Mvc;

using Microsoft.VisualBasic;

namespace KantanMitsumori.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IEstMainService _appService;
        private readonly ILogger<HomeController> _logger;
        private readonly IEstimateService _estimateService;

        public HomeController(IEstMainService appService, IEstimateService estimateService, IConfiguration config, ILogger<HomeController> logger) : base(config)
        {
            _appService = appService;
            _estimateService = estimateService;
            _logger = logger;
        }

        public IActionResult Test()
        {
            return View();
        }

        public IActionResult Index()
        {
            var mode = new LogToken();
            mode.sesEstNo = "22091900091"; mode.sesEstSubNo = "01";
            mode.UserNo = "88888195";
            mode.UserNm = "testuser88888195";
            var token = HelperToken.GenerateJsonToken(mode);
            mode.Token = token;
            setTokenCookie(token);
            return View();
        }

        public IActionResult Header()
        {
            _logToken = new LogToken();
            _logToken.UserNo = "88888195";
            _logToken.UserNm = "test";
            return PartialView("_Header", _logToken);
        }

     
    }
}

