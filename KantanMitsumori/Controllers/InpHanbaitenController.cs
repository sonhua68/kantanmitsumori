using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpHanbaitenController : BaseController
    {
        private readonly IEstimateService _estimateService;

        public InpHanbaitenController(IEstMainService appService, IEstimateService estimateService, ILogger<InpHanbaitenController> logger)
        {
            _estimateService = estimateService;
        }

        #region InpHanbaiten
        public IActionResult Index()
        {
            RequestInp request = new RequestInp();
            request.EstNo = _logSession.sesEstNo;
            request.EstSubNo = _logSession.sesEstSubNo;
            request.UserNo = _logSession.UserNo;
            request.TaxRatio = _logSession.sesTaxRatio;
            var response = _estimateService.GetDetail(request);
            if (response.ResultStatus != (int)enResponse.isSuccess)
            {
                return ErrorAction(response);
            }
            return View(response.Data);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInpHanbaiten([FromForm] RequestUpdateInpHanbaiten requestData)
        {
            var response = await _estimateService.UpdateInpHanbaiten(requestData);
            return Ok(response);
        }
        #endregion InpHanbaiten
    }
}