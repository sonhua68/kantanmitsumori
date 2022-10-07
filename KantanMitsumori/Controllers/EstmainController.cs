using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KantanMitsumori.Controllers
{

    public class EstmainController : BaseController
    {
        private readonly IEstMainService _appService;
        private readonly ILogger<HomeController> _logger;
        private readonly IEstimateService _estimateService;
        private readonly JwtSettings _jwtSettings;

        public EstmainController(IEstMainService appService, IEstimateService estimateService, ILogger<HomeController> logger, IOptions<JwtSettings> jwtSettings) : base()
        {
            _appService = appService;
            _estimateService = estimateService;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
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
                    setTokenCookie(_jwtSettings.AccessExpires, response.Data!.AccessToken);
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

