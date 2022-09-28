using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpCustKanaController : BaseController
    {
        private readonly IInpCustKanaService _inpCustKanaService;
        private readonly ILogger<InpCustKanaController> _logger;

        public InpCustKanaController(IConfiguration config, IInpCustKanaService inpCustKanaService, ILogger<InpCustKanaController> logger) : base(config)
        {
            _inpCustKanaService = inpCustKanaService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // 見積書番号を取得
            string estNo = _logToken.sesEstNo!;
            string estSubNo = _logToken.sesEstSubNo!;

            var response = _inpCustKanaService.getInfoCust(estNo, estSubNo);

            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }

            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpCustKana([FromForm] RequestUpdateInpCustKana requestData)
        {
            var response = await _inpCustKanaService.UpdateInpCustKana(requestData);

            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }




    }
}
