using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService.ASEST;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpSitaCarController : BaseController
    {
        private readonly IInpSitaCarService _inpSitaCarService;
        private readonly ILogger<InpSitaCarController> _logger;

        public InpSitaCarController(IConfiguration config, IInpSitaCarService inpSitaCarService, ILogger<InpSitaCarController> logger) : base(config)
        {
            _inpSitaCarService = inpSitaCarService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // 見積書番号を取得
            string estNo = _logToken.sesEstNo;
            string estSubNo = _logToken.sesEstSubNo;
            string userNo = _logToken.UserNo;

            var response = _inpSitaCarService.GetInfoSitaCar(estNo, estSubNo, userNo);

            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }

            return View(response.Data);
        }

        [HttpGet]
        public IActionResult GetListRikuji()
        {
            var response = _inpSitaCarService.GetListOffice();
            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            return Ok(response);
        }


        //[HttpPost]
        //public async Task<IActionResult> UpdateInpCustKana([FromForm] RequestUpdateInpCustKana requestData)
        //{
        //    var response = await _inpCustKanaService.UpdateInpCustKana(requestData);

        //    if (response.ResultStatus == (int)enResponse.isError)
        //    {
        //        return ErrorAction(response);
        //    }
        //    return Ok(response);
        //}
    }
}
