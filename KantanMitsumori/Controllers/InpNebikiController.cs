using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpNebikiController : BaseController
    {
        private readonly IEstMainService _appService;
        private readonly IEstimateService _estimateService;
        private readonly ILogger<InpNebikiController> _logger;
        public InpNebikiController(IEstMainService appService, IEstimateService estimateService, IConfiguration config, ILogger<InpNebikiController> logger) : base(config)
        {
            _appService = appService;
            _estimateService = estimateService;
            _logger = logger;
        }
        #region InpNebiki     
        public IActionResult Index()
        {
            RequestInp request = new RequestInp();
            request.EstNo = _logToken.sesEstNo;
            request.EstSubNo = _logToken.sesEstSubNo;
            request.UserNo = _logToken.UserNo;
            request.TaxRatio = _logToken.sesTaxRatio;
            var response = _estimateService.GetDetail(request);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpNebiki([FromForm] RequestUpdateInpNebiki requestData)
        {
            var response = await _estimateService.UpdateInpNebiki(requestData);           
            return Ok(response);
        }
        #endregion InpNebiki
    }
}
