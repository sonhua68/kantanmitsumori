using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{

    public class InpOptionController : BaseController
    {
        private readonly IEstMainService _appService;
        private readonly IEstimateService _estimateService;
        private readonly ILogger<InpCarController> _logger;
        public InpOptionController(IEstMainService appService, IEstimateService estimateService, ILogger<InpCarController> logger) : base()
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
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetData()
        {
            RequestInp request = new RequestInp();
            request.EstNo = _logToken.sesEstNo;
            request.EstSubNo = _logToken.sesEstSubNo;
            request.UserNo = _logToken.UserNo;
            request.TaxRatio = _logToken.sesTaxRatio;
            var response = _estimateService.GetDetail(request);
            return Ok(response);
        }

        #endregion InpOption
    }
}
