using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpSyohiyoController : BaseController
    {
        private readonly IInpSyohiyoService _inpSyohiyoService;
        private readonly ILogger<InpSyohiyoController> _logger;

        public InpSyohiyoController(IInpSyohiyoService inpSyohiyoService, ILogger<InpSyohiyoController> logger) : base()
        {
            _inpSyohiyoService = inpSyohiyoService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // 見積書番号を取得
            string estNo = _logToken.sesEstNo!;
            string estSubNo = _logToken.sesEstSubNo!;
            var response = _inpSyohiyoService.GetInfoSyohiyo(estNo, estSubNo);
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpSyohiyo([FromForm] RequestUpdateInpSyohiyo requestData)
        {
            var response = await _inpSyohiyoService.UpdateInpSyohiyo(requestData);
            return Ok(response);
        }
    }
}
