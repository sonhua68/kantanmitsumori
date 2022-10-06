using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{

    public class EstmainController : BaseController
    {
        private readonly IEstMainService _appService;
        private readonly ILogger<HomeController> _logger;
        private readonly IEstimateService _estimateService;

        public EstmainController(IEstMainService appService, IEstimateService estimateService, IConfiguration config, ILogger<HomeController> logger) : base(config)
        {
            _appService = appService;
            _estimateService = estimateService;
            _logger = logger;
        }
        public async Task<IActionResult> Index([FromQuery] RequestActionModel requestAction, [FromForm] RequestHeaderModel request)
        {
            var response = new ResponseBase<ResponseEstMainModel>();
            if (requestAction.IsInpBack == 1)
            {
                response = _appService.ReloadGetEstMain(_logToken);
            }
            else
            {
                response = await _appService.getEstMain(requestAction, request);
                if (response.ResultStatus == (int)enResponse.isSuccess)
                {
                    setTokenCookie(response.Data!.AccessToken);
                }
            }
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return View(response.Data);
        }
        [HttpGet]
        public IActionResult CheckGoPageLease(string firstRegYm, string makerName, int nowOdometer)
        {
            var response = _appService.CheckGoPageLease(firstRegYm, makerName, nowOdometer);
            return Ok(response);
        }
    }
}

