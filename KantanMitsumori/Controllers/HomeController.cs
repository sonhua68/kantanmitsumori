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
            return View();
        }

        public IActionResult Header()
        {
            return PartialView("_Header", _logToken);
        }


    }
}

