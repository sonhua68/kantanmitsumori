using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;
using KantanMitsumori.Models;
namespace KantanMitsumori.Controllers
{

    public class ErrorController :Controller
    {

        public IActionResult ErrorPage(RequestError requestData)
        {
            var ErrorViewModel = new ErrorViewModel()
            {
                MessageCode = requestData.messageCode,
                MessageContent = requestData.messageContent
            };
            return View(ErrorViewModel);
        }
    }
}