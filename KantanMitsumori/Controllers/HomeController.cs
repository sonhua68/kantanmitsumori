using KantanMitsumori.IService;
using Microsoft.AspNetCore.Mvc;

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
            var response = _appService.GetMaker();

            if (response.ResultStatus != 0)
            {
                return ErrorAction(response);
            }
            return View(response.Data);
        }


        public IActionResult Header()
        {
            ViewData["MemberId"] = "333333";
            ViewData["MemberName"] = "Test";
            return PartialView();
        }

        public IActionResult EstMain()
        {
            return View();
        }

    }
}