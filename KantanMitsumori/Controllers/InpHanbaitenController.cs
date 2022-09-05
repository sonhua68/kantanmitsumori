﻿using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Service;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpHanbaitenController : BaseController
    {
        private readonly IAppService _appService;
        private readonly IEstimateService _estimateService;
        private readonly ILogger<InpHanbaitenController> _logger;
        public InpHanbaitenController(IAppService appService, IConfiguration config, IEstimateService estimateService, ILogger<InpHanbaitenController> logger):base(config)
        {
            _appService = appService;
            _estimateService = estimateService;
            _logger = logger;
        }
        #region HoaiPhong
        public IActionResult Index()
        {
            RequestInputCar res = new RequestInputCar();         
            res.EstNo =_logToken.EstNo;
            res.EstSubNo = _logToken.EstSubNo;
            res.UserNo = "00000001";
            var response = _estimateService.GetDetail(res);
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
        #endregion HoaiPhong
    }
}