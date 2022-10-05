using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;
using KantanMitsumori.Models;
namespace KantanMitsumori.Controllers
{

    public class ErrorController :Controller
    {

        public IActionResult ErrorPage(RequestError model)
        {
            var ErrorViewModel = new ErrorViewModel()
            {
                MessageCode = model.messageCode,
                MessageContent = model.messageContent
            };
            return View(ErrorViewModel);
        }
    }
}