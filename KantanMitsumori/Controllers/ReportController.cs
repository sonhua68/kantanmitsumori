using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.IService;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Service.Helper;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;
        public ReportController(IReportService reportService, IConfiguration config, ILogger<ReportController> logger):base(config)
        {
            _reportService = reportService;
            _logger = logger;
        }

        /// <summary>
        /// Demo download article sub report
        /// </summary>        
        public IActionResult DownloadEstimateReport()
        {          
          var   model = new RequestReport()
            {
                EstNo = _logToken.sesEstNo!,
                EstSubNo = _logToken.sesEstSubNo!,
                ReportType = ReportType.Estimate,
                CustNm_forPrint = _logToken.sesCustNm_forPrint!,
                CustZip_forPrint = _logToken.sesCustZip_forPrint!,
                CustAdr_forPrint = _logToken.sesCustAdr_forPrint!,
                CustTel_forPrint = _logToken.sesCustTel_forPrint!
          };
            // Generate report
            var result = _reportService.GenerateEstimateReport(model);
            
            // Process result

            if (result.ResultStatus != 0)
                return ErrorAction(result);

            var responseModel = result.Data;
            if(responseModel == null)
                return ErrorAction(ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S)));

            return File(responseModel.Data, responseModel.ContentType, responseModel.Name);
        }


        /// <summary>
        /// Demo download article sub report
        /// </summary>        
        public IActionResult DownloadOrderReport()
        {
            // Sample data            
           var  model = new RequestReport()
            {
               EstNo = _logToken.sesEstNo!,
               EstSubNo = _logToken.sesEstSubNo!,
               ReportType = ReportType.Order,
               CustNm_forPrint = _logToken.sesCustNm_forPrint!,
               CustZip_forPrint = _logToken.sesCustZip_forPrint!,
               CustAdr_forPrint = _logToken.sesCustAdr_forPrint!,
               CustTel_forPrint = _logToken.sesCustTel_forPrint!
           };
            // Generate report
            var result = _reportService.GenerateOrderReport(model);

            // Process result
            if (result.ResultStatus != 0)
                return ErrorAction(result);

            var responseModel = result.Data;
            if (responseModel == null)
                return ErrorAction(ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S)));

            return File(responseModel.Data, responseModel.ContentType, responseModel.Name);
        }


    }
}
