using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace KantanMitsumori.Controllers
{

    public class EstmainController : BaseController
    {
        private readonly IEstMainService _appService;
        private readonly PhysicalPathSettings _physicalPathSettings;

        public EstmainController(IEstMainService appService, IOptions<PhysicalPathSettings> physicalPathSettings)
        {
            _appService = appService;
            _physicalPathSettings = physicalPathSettings.Value;
        }

        public async Task<IActionResult> Index([FromQuery] RequestActionModel requestAction, [FromForm] RequestHeaderModel request)
        {
            _appService.ConnectApiReport();
            var response = new ResponseBase<ResponseEstMainModel>();
            if (requestAction.IsInpBack == 1)
            {
                response = _appService.ReloadGetEstMain(_logSession!);
            }
            else
            {
                RemoveAllCookies();
                response = await _appService.getEstMain(requestAction, request);
                if (response.ResultStatus == (int)enResponse.isSuccess)
                {
                    setSession(response.Data!.LogSession);
                    //HelperTemData.Put<ResponseBase<ResponseEstMainModel>>(TempData, "Data", response);
                }
            }
            if (response.ResultStatus != (int)enResponse.isSuccess)
            {
                return ErrorAction(response);
            }
            return View("Index", response!.Data);


        }
        [HttpGet]
        public IActionResult CheckGoPageLease(string firstRegYm, string makerName, int nowOdometer)
        {
            var response = _appService.CheckGoPageLease(firstRegYm, makerName, nowOdometer);
            return Ok(response);
        }

        /// <summary>
        /// Download PDF
        /// </summary>
        /// <returns></returns>
        public IActionResult DownloadPdf()
        {
            string filepath = _physicalPathSettings.BizFilePdf;
            string[] strArr = filepath.Split(@"\");

            return File("~/pdf/" + strArr[strArr.Length - 1], "application/pdf", strArr[strArr.Length - 1]);
        }

        /// <summary>
        /// Export CSV
        /// </summary>
        /// <returns></returns>
        /// 
        //[HttpPost]
        public IActionResult ExportCSV()
        {
            var response = _appService.ExportDataCSV(_logSession!);

            // Process result
            if (response.ResultStatus != (int)enResponse.isSuccess)
                return ErrorAction(response);

            var data = Encoding.UTF8.GetBytes(response.Data!);
            var result = Encoding.UTF8.GetPreamble().Concat(data).ToArray();
            return File(result, "text/csv", "ASEST_" + _logSession!.sesEstNo + "-" + _logSession.sesEstSubNo + ".csv");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateJiko([FromForm] RequestUpdateJiko requestData)
        {
            var response = await _appService.UpdateJiko(requestData);
            return Ok(response);
        }


    }
}


