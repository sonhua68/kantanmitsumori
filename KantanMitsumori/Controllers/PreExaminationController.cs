using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.IService.ASEST;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KantanMitsumori.Controllers
{
    public class PreExaminationController : BaseController
    {
        private readonly IPreExaminationService _preExaminationService;
        private readonly ILogger<PreExaminationController> _logger;
        private readonly URLSettings _urlSettings;

        public PreExaminationController(IPreExaminationService preExaminationService, ILogger<PreExaminationController> logger, IOptions<URLSettings> urlSettings) : base()
        {
            _preExaminationService = preExaminationService;
            _logger = logger;
            _urlSettings = urlSettings.Value;
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
            ViewBag.PointReQuestPreExamination = _urlSettings.PointReQuestPreExamination;
            return View(response.Data);
        }
    }
}
