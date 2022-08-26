﻿using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace KantanMitsumori.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IAppService _appService;

        private readonly ILogger<HomeController> _logger;


        public HomeController(IAppService appService, ILogger<HomeController> logger)
        {
            _appService = appService;
            _logger = logger;
        }

        public IActionResult Test()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Header()
        {
            var mode = new LogToken();
            mode.EstNo = "22071200085"; mode.EstSubNo = "01";
            mode.UserNo = "88888195";
            mode.UserNm = "testuser88888195";
            var token = HelperToken.GenerateJsonToken(mode);
            mode.Token = token;
            setTokenCookie(token);
            return PartialView("_Header", mode);
        }

        [HttpPost]
        public async Task<JsonResult> TestSummitFormAjax(string token, MakerModel requestData)
        {
            var response = await _appService.CreateMaker(requestData);
            var logToken = HelperToken.EncodingToken(token);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> Test(string token, MakerModel requestData)
        {
            var response = await _appService.CreateMaker(requestData);
            var logToken = HelperToken.EncodingToken(token);


            if (response.ResultStatus == 0)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetEstMain([FromQuery] string sel, [FromForm] RequestHeaderModel request)
        {
            Uri pageUrl;

            try
            {
                string headRef = Request.Headers["Referer"];

                pageUrl = new Uri(headRef);
            }
            catch (Exception)
            {
                pageUrl = new Uri("http://www.asnet2.com/asest2/test.html");
            }

            if (Strings.InStr(pageUrl.AbsolutePath, "/asest2/") == 0 || Strings.InStr(pageUrl.AbsolutePath, "/test.htm/") > 0)
            {
                return await EstMain(sel, request);
            }
            else if (Strings.InStr(pageUrl.AbsolutePath, "/asest2/SelGrd.cshtml/") == 0 || Strings.InStr(pageUrl.AbsolutePath, "/test.htm/") > 0)
            {
                return await EstMainLocal(sel, request);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EstMain(string sel, RequestHeaderModel request)
        {
            // câu truc link summit qa https://mit.autoserver.co.jp/asest2/EstMain.aspx
            //var response = await _appService.getEstMain(isInputBack, sel, request);

            var response = _appService.calcSum("22071200085", "01");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EstMainLocal(string sel, RequestHeaderModel request)
        {
            //var response = await _appService.getEstMain(isInputBack, sel, request);

            return View();
        }
    }
}