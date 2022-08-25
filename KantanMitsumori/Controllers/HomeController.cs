using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Service;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IAppService _appService;
        private readonly IEstimateService _estimateService;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IAppService appService, IEstimateService estimateService,  ILogger<HomeController> logger)
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

        [HttpPost]
        public async Task<IActionResult> UpdateInputCar(RequestUpdateInputCar requestData)
        {
            var response = await _estimateService.UpdateInputCar(requestData);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInpHanbaiten(RequestUpdateInpHanbaiten requestData)
        {
            var response = await _estimateService.UpdateInpHanbaiten(requestData);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }
        #endregion HoaiPhong
    }
}