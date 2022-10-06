using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService.ASEST;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class PreExaminationController : BaseController
    {
        private readonly IPreExaminationService _preExaminationService;
        private readonly ILogger<PreExaminationController> _logger;

        public PreExaminationController(IConfiguration config, IPreExaminationService preExaminationService, ILogger<PreExaminationController> logger) : base(config)
        {
            _preExaminationService = preExaminationService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // 見積書データ取得

            string estNo = _logToken.sesEstNo ?? "";
            string estSubNo = _logToken.sesEstSubNo ?? "";

            var response = _preExaminationService.GetInfoPreExamination(estNo, estSubNo);

            if (response.ResultStatus == (int)enResponse.isError)
            {
                return ErrorAction(response);
            }
            ViewBag.PointReQuestPreExamination = _commonSettings.URLSettings.PointReQuestPreExamination;
            return View(response.Data);
        }
    }
}
