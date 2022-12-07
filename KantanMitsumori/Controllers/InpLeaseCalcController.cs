﻿using KantanMitsumori.Helper.Enum;
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

        public InpLeaseCalcController(IInpLeaseCalcService inpLeaseCalc,
           ILogger<InpLeaseCalcController> logger)
        {
            _logger = logger;
            _inpLeaseCalc = inpLeaseCalc;

        }

        #region InpLeaseCalc 
        public IActionResult Index()
        {
            var response = _inpLeaseCalc.GetDataInpLease(_logSession!);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return View(response.Data);
        }

        [HttpGet]
        public IActionResult GetCarType()
        {
            var response = _inpLeaseCalc.GetCarType();
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetContractPlan()
        {
            var response = _inpLeaseCalc.GetContractPlan();
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetVolInsurance()
        {
            var response = _inpLeaseCalc.GetVolInsurance();
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetFirstAfterSecondTerm(int carType)
        {
            var response = _inpLeaseCalc.GetFirstAfterSecondTerm(carType);
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetUnitPriceRatesLimit()
        {
            var response = _inpLeaseCalc.GetUnitPriceRatesLimit();
            return Ok(response);
        }

        [HttpPost]
        public IActionResult InpLeaseCal([FromForm] RequestInpLeaseCalc requestData)
        {
            var response = _inpLeaseCalc.InpLeaseCal(requestData, _logSession!);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLeaseProgressIde(int requestData)
        {
            var response = await _inpLeaseCalc.UpdateLeaseProgressIde(requestData, _logSession!);
            return Ok(response);
        }

        #endregion InpLeaseCalc
    }
}
