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
        private readonly URLSettings _urlSettings;

        public PreExaminationController(IPreExaminationService preExaminationService, IOptions<URLSettings> urlSettings)
        {
            _preExaminationService = preExaminationService;
            _urlSettings = urlSettings.Value;
        }

        public IActionResult Index()
        {
            string estNo = _logSession!.sesEstNo ?? "";
            string estSubNo = _logSession.sesEstSubNo ?? "";
            var response = _preExaminationService.GetInfoPreExamination(estNo, estSubNo);
            if (response.ResultStatus != (int)enResponse.isSuccess)
            {
                return ErrorAction(response);
            }
            ViewBag.PointReQuestPreExamination = _urlSettings.PointReQuestPreExamination;
            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePreExamination()
        {
            var response = await _preExaminationService.UpdatePreExamination(_logSession!);
            return Ok(response);
        }
    }
}
