using KantanMitsumori.Helper.Enum;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class InpNotesController : BaseController
    {
        private readonly IInpNotesService _inpNotesService;

        public InpNotesController(IInpNotesService inpNotesService)
        {
            _inpNotesService = inpNotesService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // 見積書番号を取得
            string estNo = _logSession.sesEstNo!;
            string estSubNo = _logSession.sesEstSubNo!;

            var response = _inpNotesService.getInfoNotes(estNo, estSubNo);

            if (response.ResultStatus != (int)enResponse.isSuccess)
            {
                return ErrorAction(response);
            }

            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInpNotes([FromForm] RequestUpdateInpNotes requestData)
        {
            var response = await _inpNotesService.UpdateInpNotes(requestData);
            return Ok(response);
        }

    }
}
