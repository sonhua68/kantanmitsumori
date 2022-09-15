﻿using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpHanbaitenController : BaseController
    {
        private readonly IAppService _appService;
        private readonly IEstimateService _estimateService;
        private readonly ILogger<InpHanbaitenController> _logger;
        public InpHanbaitenController(IAppService appService, IConfiguration config, IEstimateService estimateService, ILogger<InpHanbaitenController> logger) : base(config)
        {
            _appService = appService;
            _estimateService = estimateService;
            _logger = logger;
        }
        #region InpHanbaiten
        public IActionResult Index()
        {
            RequestInp request = new RequestInp();
            request.EstNo = _logToken.sesEstNo;
            request.EstSubNo = _logToken.sesEstSubNo;
            request.UserNo = _logToken.UserNo;
            var response = _estimateService.GetDetail(request);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return View(response.Data);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInpHanbaiten([FromForm] RequestUpdateInpHanbaiten requestData)
        {
            var response = await _estimateService.UpdateInpHanbaiten(requestData);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }
        #endregion InpHanbaiten
    }
}