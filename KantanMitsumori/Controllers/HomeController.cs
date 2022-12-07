using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{

    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            RemoveAllCookies();
            return View();
        }

        public IActionResult Header()
        {
            return PartialView("_Header", _logSession);
        }


    }
}

