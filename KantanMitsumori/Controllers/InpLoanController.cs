using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpLoanController : BaseController
    {
        private readonly IEstimateService _estimateService;
        private readonly IInpLoanService _inpLoanService;

        public InpLoanController(IEstimateService estimateService, IInpLoanService inpLoanService)
        {
            _estimateService = estimateService;
            _inpLoanService = inpLoanService;
        }

        #region InpLoan


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
        public IActionResult CalInpLoan([FromForm] RequestCalInpLoan requestData)
        {
            var response = _inpLoanService.CalInpLoan(requestData);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpLoan([FromForm] RequestUpdateInpLoan requestData)
        {
            var response = await _inpLoanService.UpdateInputLoan(requestData);
            return Ok(response);
        }
        #endregion HoaiPhong
    }
}