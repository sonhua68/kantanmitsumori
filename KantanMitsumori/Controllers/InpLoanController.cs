using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpLoanController : BaseController
    {
        private readonly IAppService _appService;
        private readonly IEstimateService _estimateService;
        private readonly IInpLoanService _inpLoanService;
        private readonly ILogger<InpLoanController> _logger;
        public InpLoanController(IAppService appService, IEstimateService estimateService, IConfiguration config, IInpLoanService inpLoanService, ILogger<InpLoanController> logger) : base(config)
        {
            _appService = appService;
            _estimateService = estimateService;
            _inpLoanService = inpLoanService;
            _logger = logger;
        }
        #region HoaiPhong


        public IActionResult Index()
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
            return View(response.Data);
        }

        [HttpPost]
        public IActionResult CalInpLoan([FromForm] RequestCalInpLoan requestData)
        {
            var response = _inpLoanService.CalInpLoan(requestData);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateInpLoan([FromForm] RequestUpdateInpLoan requestData)
        {
            var response = await _inpLoanService.UpdateInputLoan(requestData);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }
        #endregion HoaiPhong
    }
}