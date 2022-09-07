using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpZeiHokenController : BaseController
    {
        private readonly IAppService _appService;
        private readonly IEstimateService _estimateService;
        private readonly ILogger<InpZeiHokenController> _logger;
        public InpZeiHokenController(IAppService appService, IEstimateService estimateService, IConfiguration config, ILogger<InpZeiHokenController> logger) : base(config)
        {
            _appService = appService;
            _estimateService = estimateService;
            _logger = logger;
        }

        #region InpZeiHoken     
        public IActionResult Index()
        {
            RequestInp res = new RequestInp();
            res.EstNo = "22082900004";
            res.EstSubNo = "01";
            res.UserNo = "00000001";
            var response = _estimateService.GetDetail(res);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpZeiHoken([FromForm] RequestUpdateInpZeiHoken requestData)
        {
            var response = await _estimateService.UpdateInpZeiHoken(requestData);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }


        #endregion InpZeiHoken
    }
}
