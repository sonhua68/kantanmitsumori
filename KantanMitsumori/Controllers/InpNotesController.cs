﻿using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Service.Helper;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpNotesController : BaseController
    {
        private readonly IInpNotesService _inpNotesService;
        private readonly ILogger<InpCustKanaController> _logger;

        private CommonEstimate _commonEst;

        public InpNotesController(IConfiguration config, IInpNotesService inpNotesService, ILogger<InpCustKanaController> logger, CommonEstimate commonEst) : base(config)
        {
            _inpNotesService = inpNotesService;
            _logger = logger;
            _commonEst = commonEst;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // 見積書番号を取得
            string estNo = _logToken.sesEstNo;
            string estSubNo = _logToken.sesEstSubNo;

            var response = _inpNotesService.getInfoNotes(estNo, estSubNo);

            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }

            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpNotes([FromForm] RequestUpdateInpNotes requestData)
        {
            var response = await _inpNotesService.UpdateInpNotes(requestData);

            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }




    }
}
