﻿using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpCustKanaController : BaseController
    {
        private readonly IInpCustKanaService _inpCustKanaService;

        public InpCustKanaController(IInpCustKanaService inpCustKanaService)
        {
            _inpCustKanaService = inpCustKanaService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // 見積書番号を取得
            string estNo = _logSession.sesEstNo!;
            string estSubNo = _logSession.sesEstSubNo!;

            var response = _inpCustKanaService.getInfoCust(estNo, estSubNo);

            if (response.ResultStatus != (int)enResponse.isSuccess)
            {
                return ErrorAction(response);
            }

            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpCustKana([FromForm] RequestUpdateInpCustKana requestData)
        {
            var response = await _inpCustKanaService.UpdateInpCustKana(requestData);
            return Ok(response);
        }

    }
}
