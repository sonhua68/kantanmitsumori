using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpSyohiyoController : BaseController
    {
        private readonly IInpSyohiyoService _inpSyohiyoService;

        public InpSyohiyoController(IInpSyohiyoService inpSyohiyoService)
        {
            _inpSyohiyoService = inpSyohiyoService;
        }

        public IActionResult Index()
        {
            // 見積書番号を取得
            string estNo = _logSession.sesEstNo!;
            string estSubNo = _logSession.sesEstSubNo!;
            var response = _inpSyohiyoService.GetInfoSyohiyo(estNo, estSubNo);
            if (response.ResultStatus != (int)enResponse.isSuccess)
            {
                return ErrorAction(response);
            }
            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpSyohiyo([FromForm] RequestUpdateInpSyohiyo requestData)
        {
            var response = await _inpSyohiyoService.UpdateInpSyohiyo(requestData, _logSession!);
            return Ok(response);
        }
    }
}
