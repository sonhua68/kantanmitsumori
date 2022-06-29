using KantanMitsumori.IService;
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
            var response = _appService.GetMaker();         
                
            if (response.ResultStatus != 0)
            {
                return ErrorAction(response);
            }
            return View(response.Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}