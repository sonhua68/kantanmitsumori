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
        public async Task<IActionResult> Index()
        {
            var response = await _estMainService.ReloadGetEstMain(_logToken);
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
            var response = await _inpLeaseCalc.GetContractPlan();
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetVolInsurance()
        {
            var response = await _inpLeaseCalc.GetVolInsurance();
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetFirstAfterSecondTerm(int carType)
        {
            var response = await _inpLeaseCalc.GetFirstAfterSecondTerm(carType);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetUnitPriceRatesLimit()
        {
            var response = await _inpLeaseCalc.GetUnitPriceRatesLimit();
            return Ok(response);
        }


        #endregion InpLeaseCalc
    }
}
