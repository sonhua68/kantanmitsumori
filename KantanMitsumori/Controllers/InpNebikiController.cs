using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpNebikiController : BaseController
    {
        private readonly IEstimateService _estimateService;

        public InpNebikiController(IEstimateService estimateService)
        {
            _estimateService = estimateService;
        }

        #region InpNebiki     
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
        public async Task<IActionResult> UpdateInpNebiki([FromForm] RequestUpdateInpNebiki requestData)
        {
            var response = await _estimateService.UpdateInpNebiki(requestData,_logSession!);
            return Ok(response);
        }
        #endregion InpNebiki
    }
}
