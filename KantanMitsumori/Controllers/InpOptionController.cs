using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{

    public class InpOptionController : BaseController
    {
        private readonly IAppService _appService;
        private readonly IEstimateService _estimateService;
        private readonly ILogger<InpCarController> _logger;
        public InpOptionController(IAppService appService, IEstimateService estimateService, IConfiguration config, ILogger<InpCarController> logger) : base(config)
        {
            _appService = appService;
            _estimateService = estimateService;
            _logger = logger;
        }

        #region InpOption     
        public IActionResult Index()
        {  
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpOption([FromForm] RequestUpdateInpOption requestData)
        {
            var response = await _estimateService.UpdateInpOption(requestData);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }
        [HttpGet]
        public  IActionResult GetData()
        {
            RequestInp res = new RequestInp();        
            res.EstNo = _logToken.sesEstNo;
            res.EstSubNo = _logToken.sesEstSubNo;
            res.UserNo = _logToken.UserNo;
            var response = _estimateService.GetDetail(res);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }

        #endregion InpOption
    }
}
