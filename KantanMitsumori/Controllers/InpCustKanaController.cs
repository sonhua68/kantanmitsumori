using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Service.Helper;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpCustKanaController : BaseController
    {
        private readonly IInpCustKanaService _inpCustKanaService;
        private readonly ILogger<InpCustKanaController> _logger;

        private CommonEstimate _commonEst;

        public InpCustKanaController(IConfiguration config, IInpCustKanaService inpCustKanaService, ILogger<InpCustKanaController> logger, CommonEstimate commonEst) : base(config)
        {
            _inpCustKanaService = inpCustKanaService;
            _logger = logger;
            _commonEst = commonEst;
        }

        // GET: InpCustKanaController
        public IActionResult Index()
        {
            // 見積書番号を取得
            string estNo = _logToken.sesEstNo;
            string estSubNo = _logToken.sesEstSubNo;

            var response = _inpCustKanaService.getInfoCust(estNo, estSubNo);

            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }

            return View(response);
        }
    }
}
