using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

using Microsoft.VisualBasic;

namespace KantanMitsumori.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IAppService _appService;

        private readonly ILogger<HomeController> _logger;

        private readonly IEstimateService _estimateService;


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
            return View();
        }

        public IActionResult Header()
        {
            return PartialView("_Header", _logToken);
        }

        [HttpPost]
        public async Task<JsonResult> TestSummitFormAjax(string token, MakerModel requestData)
        {
            var response = await _appService.CreateMaker(requestData);
            var logToken = HelperToken.EncodingToken(token);
            return Json(response);
        }

        [HttpPost]
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

        [HttpPost]
        public async Task<IActionResult> GetEstMain([FromQuery] string sel, [FromForm] RequestHeaderModel request)
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

            if (Strings.InStr(pageUrl.AbsolutePath, "/asest2/") == 0 || Strings.InStr(pageUrl.AbsolutePath, "/test.htm/") > 0)
            {
                return await EstMain(sel, request);
            }
            else if (Strings.InStr(pageUrl.AbsolutePath, "/asest2/SelGrd.cshtml/") == 0 || Strings.InStr(pageUrl.AbsolutePath, "/test.htm/") > 0)
            {
                return await EstMainLocal(sel, request);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EstMain(string sel, RequestHeaderModel request)
        {
            // câu truc link summit qa https://mit.autoserver.co.jp/asest2/EstMain.aspx
            //var response = await _appService.getEstMain(isInputBack, sel, request);

            //var response = _appService.calcSum("22071200085", "01");
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> EstMainLocal(string sel, RequestHeaderModel request)
        {
            //var response = await _appService.getEstMain(isInputBack, sel, request);

            return View();
        }

        #region HoaiPhong

        public IActionResult Inpcar()
        {
            RequestInputCar res = new RequestInputCar();
            res.EstNo = "22082300011";
            res.EstSubNo = "01";
            var response = _estimateService.GetDetail(res);
            return View(response.Data);
        }

        public IActionResult InpHanbaiten()
        {
            RequestInputCar res = new RequestInputCar();
            res.EstNo = "22082300011";
            res.EstSubNo = "01";
            var response = _estimateService.GetDetail(res);
            return View(response.Data);
        }

        public async Task<IActionResult> UpdateInputCar([FromForm] RequestUpdateInputCar requestData)
        {
            var response = await _estimateService.UpdateInputCar(requestData);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpHanbaiten([FromForm] RequestUpdateInpHanbaiten requestData)
        {
            var response = await _estimateService.UpdateInpHanbaiten(requestData);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }

        #endregion
    }
}

