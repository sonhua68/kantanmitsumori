using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;
using KantanMitsumori.Models;
using KantanMitsumori.Model;

namespace KantanMitsumori.Controllers
{

    public class ErrorController :BaseController
    {

        //public IActionResult ErrorPage(RequestError requestData)
        //{    
        //    var ErrorViewModel = new ErrorViewModel()
        //    {
        //        MessageCode = requestData.messageCode,
        //        MessageContent = requestData.messageContent,
        //        logToken = _logToken ?? new LogToken(),
        //    };
        //    return View(ErrorViewModel);
        //}
    }
}