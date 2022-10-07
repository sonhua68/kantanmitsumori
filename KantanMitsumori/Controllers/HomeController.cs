using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{

    public class HomeController : BaseController
    {
        private readonly IEstMainService _appService;
        private readonly ILogger<HomeController> _logger;
        private readonly IEstimateService _estimateService;

        public HomeController(IEstMainService appService, IEstimateService estimateService, ILogger<HomeController> logger) : base()
        {
            _appService = appService;
            _estimateService = estimateService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Header()
        {
            return PartialView("_Header", _logToken);
        }


    }
}

