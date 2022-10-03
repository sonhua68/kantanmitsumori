using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Models;
using KantanMitsumori.Service;
using KantanMitsumori.Service.ASEST;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpLeaseCalcController : BaseController
    {
        private readonly ILogger<InpLeaseCalcController> _logger;
        private readonly IInpLeaseCalcService _inpLeaseCalc;
        private readonly IEstMainService _estMainService;
        private readonly IEstimateService _estimateService;
        public InpLeaseCalcController(IInpLeaseCalcService inpLeaseCalc, IEstimateService estimateService, IEstMainService estMainService, IConfiguration config, ILogger<InpLeaseCalcController> logger) : base(config)
        {
            _logger = logger;
            _inpLeaseCalc = inpLeaseCalc;
            _estMainService = estMainService;
            _estimateService = estimateService;
        }

        #region InpLeaseCalc 
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

        [HttpGet]
        public async Task<IActionResult> GetCarType()
        {
            var response = await _inpLeaseCalc.GetCarType();        
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetContractPlan()
        {
            //var response = await _estMainService.setFreeEst(requestData, _logToken);
            //if (response.ResultStatus == (int)enResponse.isError)
            //{
            //    return ErrorAction(response);
            //}
            //setTokenCookie(response.Data!.AccessToken);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetVolInsurance()
        {
            //var response = await _estMainService.setFreeEst(requestData, _logToken);
            //if (response.ResultStatus == (int)enResponse.isError)
            //{
            //    return ErrorAction(response);
            //}
            //setTokenCookie(response.Data!.AccessToken);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> LeaseCalc(RequestSelGrdFreeEst requestData)
        {
            var response = await _estMainService.setFreeEst(requestData, _logToken);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            setTokenCookie(response.Data!.AccessToken);
            return Ok(response);
        }

        #endregion InpLeaseCalc
    }
}
