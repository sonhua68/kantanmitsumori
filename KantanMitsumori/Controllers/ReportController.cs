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
        /// Download estimate report
        /// </summary>        
        public IActionResult DownloadEstimateReport(RequestReport? model)
        {
            // Sample data            
            model = new RequestReport()
            {
                EstNo = "22092000032",
                EstSubNo = "01",
                ReportType = ReportType.Estimate,
                CustNm_forPrint = "DANG PHAM",
                CustZip_forPrint = "702201",
                CustAdr_forPrint = "236/43/2 DIEN BIEN PHU P.17 Q.BT",
                CustTel_forPrint = "028-3801-5151"
            };

            // Set report type
            model.ReportType = ReportType.Estimate;

            // Generate report
            var result = _reportService.GenerateReport(model);
            
            // Process result
            if (result.ResultStatus != 0)
                return ErrorAction(result);

            var responseModel = result.Data;
            if(responseModel == null)
                return ErrorAction(ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S)));

            return File(responseModel.Data, responseModel.ContentType, responseModel.Name);
        }


        /// <summary>
        /// Download order report
        /// </summary>        
        public IActionResult DownloadOrderReport(RequestReport? model)
        {
            // Sample data            
            model = new RequestReport()
            {
                EstNo = "22092000032",
                EstSubNo = "01",                
                CustNm_forPrint = "DANG PHAM",
                CustZip_forPrint = "702201",
                CustAdr_forPrint = "236/43/2 DIEN BIEN PHU P.17 Q.BT",
                CustTel_forPrint = "028-3801-5151"
            };
            
            // Set report type
            model.ReportType = ReportType.Order;

            // Generate report
            var result = _reportService.GenerateReport(model);

            // Process result
            if (result.ResultStatus != 0)
                return ErrorAction(result);

            var responseModel = result.Data;
            if (responseModel == null)
                return ErrorAction(ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S)));

            return File(responseModel.Data, responseModel.ContentType, responseModel.Name);
        }

        /// <summary>
        /// Download Lease Estimate report
        /// </summary>        
        public IActionResult DownloadLeaseEstimateReport(RequestReport? model)
        {
            // Sample data            
            model = new RequestReport()
            {
                EstNo = "22092000032",
                EstSubNo = "01",                
                CustNm_forPrint = "DANG PHAM",
                CustZip_forPrint = "702201",
                CustAdr_forPrint = "236/43/2 DIEN BIEN PHU P.17 Q.BT",
                CustTel_forPrint = "028-3801-5151"
            };

            // Set report type
            model.ReportType = ReportType.LeaseEstimate;

            // Generate report
            var result = _reportService.GenerateReport(model);

            // Process result
            if (result.ResultStatus != 0)
                return ErrorAction(result);

            var responseModel = result.Data;
            if (responseModel == null)
                return ErrorAction(ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S)));

            return File(responseModel.Data, responseModel.ContentType, responseModel.Name);
        }

        /// <summary>
        /// Download Lease Order report
        /// </summary>        
        public IActionResult DownloadLeaseOrderReport(RequestReport? model)
        {
            // Sample data            
            model = new RequestReport()
            {
                EstNo = "22092000032",
                EstSubNo = "01",                
                CustNm_forPrint = "DANG PHAM",
                CustZip_forPrint = "702201",
                CustAdr_forPrint = "236/43/2 DIEN BIEN PHU P.17 Q.BT",
                CustTel_forPrint = "028-3801-5151"
            };

            // Set report type
            model.ReportType = ReportType.LeaseOrder;

            // Generate report
            var result = _reportService.GenerateReport(model);

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
