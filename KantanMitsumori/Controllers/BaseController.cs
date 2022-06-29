using KantanMitsumori.Model;
using KantanMitsumori.Models;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class BaseController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string messageCode, string messageContent)
        {
            return View(new ErrorViewModel { MessageCode = messageCode, MessageContent = messageContent });
        }

        public IActionResult ErrorAction<T>(ResponseBase<T> response)
        {
            return RedirectToAction("Error", new ErrorViewModel { MessageCode = response.MessageCode, MessageContent = response.MessageContent });
        }
    }
}
