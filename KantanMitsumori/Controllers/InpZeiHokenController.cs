﻿using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpZeiHokenController : BaseController
    {
        private readonly IEstMainService _appService;
        private readonly IEstimateService _estimateService;
        private readonly IInpZeiHokenService _inpZeiHokenService;
        private readonly ILogger<InpZeiHokenController> _logger;

        public InpZeiHokenController(IEstMainService appService, IEstimateService estimateService, IInpZeiHokenService inpZeiHokenService, ILogger<InpZeiHokenController> logger) : base()
        {
            _appService = appService;
            _estimateService = estimateService;
            _logger = logger;
            _inpZeiHokenService = inpZeiHokenService;
        }

        #region InpZeiHoken     
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

        [HttpPost]
        public async Task<IActionResult> UpdateInpZeiHoken([FromForm] RequestUpdateInpZeiHoken requestData)
        {
            var response = await _estimateService.UpdateInpZeiHoken(requestData);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult calcCarTax(RequestInpZeiHoken requestData)
        {
            var response = _inpZeiHokenService.calcCarTax(requestData);
            return Ok(response);
        }
        [HttpPost]
        public IActionResult calcJibai(RequestInpZeiHoken requestData)
        {
            var response = _inpZeiHokenService.calcJibai(requestData);
            return Ok(response);
        }
        #endregion InpZeiHoken
    }
}
