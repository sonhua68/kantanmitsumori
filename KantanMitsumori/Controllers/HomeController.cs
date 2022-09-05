using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Service;
using Microsoft.AspNetCore.Mvc;
using static Org.BouncyCastle.Math.EC.ECCurve;
using KantanMitsumori.Models;

namespace KantanMitsumori.Controllers
{
    
    public class HomeController : BaseController
    {
        private readonly IAppService _appService;
        private readonly IEstimateService _estimateService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IAppService appService, IEstimateService estimateService, IConfiguration config, ILogger<HomeController> logger) : base(config)
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
            mode.EstNo = "22071200085"; mode.EstSubNo = "01";
            mode.UserNo = "88888195";
            mode.UserNm = "testuser88888195";
            var token = HelperToken.GenerateJsonToken(mode);
            mode.Token = token;
            setTokenCookie(token);
            return View();
        }
      
        public IActionResult Header()
        {       
            return PartialView("_Header",_logToken);
        }

        [HttpPost]
        public async Task<JsonResult> TestSummitFormAjax(string token, MakerModel requestData)
        {
            var response = await _appService.CreateMaker(requestData);
            var logToken = HelperToken.EncodingToken(token);
            return Json(response);
        }
        public async Task<IActionResult> Test(string token, MakerModel requestData)
        {
            var response = await _appService.CreateMaker(requestData);
            var logToken = HelperToken.EncodingToken(token);

            if (response.ResultStatus == 0)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }

        public IActionResult EstMain()
        {
            return View();
        }
        public IActionResult Error()
        { 
            return View();
        }
    }
}