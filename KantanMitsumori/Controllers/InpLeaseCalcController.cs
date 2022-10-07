using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Models;
using KantanMitsumori.Service;
using KantanMitsumori.Service.ASEST;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KantanMitsumori.Controllers
{
    public class InpLeaseCalcController : BaseController
    {
        private readonly ILogger<InpLeaseCalcController> _logger;
        private readonly IInpLeaseCalcService _inpLeaseCalc;
        private readonly IEstMainService _estMainService;
    
        public InpLeaseCalcController(IInpLeaseCalcService inpLeaseCalc, IEstimateService estimateService,
            IEstMainService estMainService, IConfiguration config, ILogger<InpLeaseCalcController> logger) 
        {
            _logger = logger;
            _inpLeaseCalc = inpLeaseCalc;
            _estMainService = estMainService;
            
        }

        #region InpLeaseCalc 
        public IActionResult Index()
        {
            var response =  _estMainService.ReloadGetEstMain(_logToken);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return View(response.Data);
        }

        [HttpGet]
        public  IActionResult GetCarType()
        {
            var response =  _inpLeaseCalc.GetCarType();
            return Ok(response);
        }
        [HttpGet]
        public  IActionResult GetContractPlan()
        {
            var response =  _inpLeaseCalc.GetContractPlan();
            return Ok(response);
        }
        [HttpGet]
        public  IActionResult GetVolInsurance()
        {
            var response =  _inpLeaseCalc.GetVolInsurance();
            return Ok(response);
        }
        [HttpGet]
        public  IActionResult GetFirstAfterSecondTerm(int carType)
        {
            var response =  _inpLeaseCalc.GetFirstAfterSecondTerm(carType);
            return Ok(response);
        }
        [HttpGet]
        public  IActionResult GetUnitPriceRatesLimit()
        {
            var response =  _inpLeaseCalc.GetUnitPriceRatesLimit();
            return Ok(response);
        }

        [HttpPost]
        public IActionResult InpLeaseCal([FromForm] RequestInpLeaseCalc requestData)
        {
            var response = _inpLeaseCalc.InpLeaseCal(requestData,_logToken);           
            return Ok(response);
        }

        #endregion InpLeaseCalc
    }
}
