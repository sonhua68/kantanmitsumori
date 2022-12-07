using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{

    public class InpCarController : BaseController
    {
        private readonly IEstimateService _estimateService;
        public InpCarController(IEstimateService estimateService)
        {
            _estimateService = estimateService;
        }

        #region InpCar     
        public IActionResult Index()
        {
            RequestInp request = new RequestInp();
            request.EstNo = _logSession!.sesEstNo;
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
        public async Task<IActionResult> UpdateInputCar([FromForm] RequestUpdateInputCar requestData)
        {
            var response = await _estimateService.UpdateInputCar(requestData);
            return Ok(response);
        }
        #endregion InpCar
    }
}