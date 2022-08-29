using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Service;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpLoanController : BaseController
    {
        private readonly IAppService _appService;
        private readonly IEstimateService _estimateService;
        private readonly ILogger<InpLoanController> _logger;
        public InpLoanController(IAppService appService, IEstimateService estimateService,  ILogger<InpLoanController> logger)
        {
            _appService = appService;
            _estimateService = estimateService;
            _logger = logger;
        }    

      
        #region HoaiPhong
   
     
        public IActionResult InpLoan()
        {
            RequestInputCar res = new RequestInputCar();
            res.EstNo = "22082900004";
            res.EstSubNo = "01";
            var response = _estimateService.GetDetail(res);
            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpLoan([FromForm] RequestUpdateInputCar requestData)
        {
            var response = await _estimateService.UpdateInputCar(requestData);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }      
        #endregion HoaiPhong
    }
}