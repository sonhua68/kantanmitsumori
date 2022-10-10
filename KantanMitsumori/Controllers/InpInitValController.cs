using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpInitValController : BaseController
    {
        private readonly IInpInitValService _inpInitValService;
        private readonly IEstimateService _estimateService;

        public InpInitValController(IInpInitValService inpInitValService, IEstimateService estimateService)
        {
            _inpInitValService = inpInitValService;
            _estimateService = estimateService;
        }

        #region InpInitVal 

        public IActionResult Index()
        {
            RequestInp request = new RequestInp();
            request.EstNo = _logToken.sesEstNo;
            request.EstSubNo = _logToken.sesEstSubNo;
            request.UserNo = _logToken.UserNo;
            request.TaxRatio = _logToken.sesTaxRatio;
            var response = _estimateService.GetDetail(request);
            if (response.ResultStatus != (int)enResponse.isSuccess)
            {
                return ErrorAction(response);
            }
            return View(response.Data);
        }

        [HttpGet]
        public IActionResult GetUserDefData()
        {
            var response = _inpInitValService.GetUserDefData(_logToken.UserNo!);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpInitVal([FromForm] RequestUpdateInpInitVal requestData)
        {
            requestData.sesMode = _logToken.sesMode;
            var response = await _inpInitValService.UpdateInpInitVal(requestData, _logToken);
            return Ok(response);
        }

        #endregion InpInitVal
    }
}
