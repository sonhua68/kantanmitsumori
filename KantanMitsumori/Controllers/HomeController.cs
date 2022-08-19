using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IAppService _appService;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IAppService appService, ILogger<HomeController> logger)
        {
            _appService = appService;
            _logger = logger;
        }

        public IActionResult Test()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Header()
        {
            var mode = new LogToken();
            mode.EstNo = "22071200085"; mode.EstSubNo = "01";
            mode.UserNo = "88888195";
            mode.UserNm = "testuser88888195";
            var token = HelperToken.GenerateJsonToken(mode);
            mode.Token = token;   
            setTokenCookie(token);
            return PartialView("_Header", mode);
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

    }
}