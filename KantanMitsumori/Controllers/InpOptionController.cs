﻿using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{

    public class InpOptionController : BaseController
    {
        private readonly IEstimateService _estimateService;

        public InpOptionController(IEstimateService estimateService)
        {
            _estimateService = estimateService;
        }

        #region InpOption     
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpOption([FromForm] RequestUpdateInpOption requestData)
        {
            var response = await _estimateService.UpdateInpOption(requestData,_logSession!);
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetData()
        {
            RequestInp request = new RequestInp();
            request.EstNo = _logSession.sesEstNo;
            request.EstSubNo = _logSession.sesEstSubNo;
            request.UserNo = _logSession.UserNo;
            request.TaxRatio = _logSession.sesTaxRatio;
            var response = _estimateService.GetDetail(request);
            return Ok(response);
        }

        #endregion InpOption
    }
}
