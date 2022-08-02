using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        public IActionResult Index()
        {               
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Header()
        {
            var mode = new LogToken();
            mode.EstNo = "22071200085"; mode.EstSubNo = "01";
            var token = HelperToken.GenerateJsonToken(mode);
            mode.Token = token;
            return PartialView("_Header", mode);
        }

        [HttpPost]    
        public async Task<JsonResult> TestSummitFormAjax(string token,MakerModel requestData)
        {         
            var response = await _appService.CreateMaker(requestData);   
            var logToken  = HelperToken.EncodingToken(token);
            return Json(response);
        }       
    }
}