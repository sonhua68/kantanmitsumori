using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;
using KantanMitsumori.Models;
using KantanMitsumori.Model;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Utility;

namespace KantanMitsumori.Controllers
{

    public class ErrorController :BaseController
    {
        public IActionResult Index()
        {
            var ErrorViewModel = new ErrorViewModel()
            {
                MessageCode = HelperMessage.ISYS010I,
                MessageContent = KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I),
                LogSession = _logSession ?? new LogSession(),
            };
            return View(ErrorViewModel);
        }
    }
}