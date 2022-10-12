using AutoMapper;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
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
        private readonly IMapper _mapper;

        public ReportController(IReportService reportService, IMapper mapper)
        {
            _reportService = reportService;
            _mapper = mapper;
        }

        /// <summary>
        /// Download estimate report
        /// </summary>        
        public IActionResult DownloadEstimateReport()
        {
            // Get request model
            var model = new RequestReport();
            _mapper.Map(_logToken, model);

            // Sample data            
            model = new RequestReport()
            {
                EstNo = "22092300054",
                EstSubNo = "01",
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

            if (result.ResultStatus != (int)enResponse.isSuccess)
                return ErrorAction(result);

            var responseModel = result.Data;
            if (responseModel == null)
                return ErrorAction(ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S)));

            return File(responseModel.Data, responseModel.ContentType, responseModel.Name);
        }


        /// <summary>
        /// Download order report
        /// </summary>        
        public IActionResult DownloadOrderReport()
        {
            // Get request model
            var model = new RequestReport();
            _mapper.Map(_logToken, model);

            // Sample data            
            model = new RequestReport()
            {
                EstNo = "22092300054",
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
            if (result.ResultStatus != (int)enResponse.isSuccess)
                return ErrorAction(result);

            var responseModel = result.Data;
            if (responseModel == null)
                return ErrorAction(ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S)));

            return File(responseModel.Data, responseModel.ContentType, responseModel.Name);
        }

    }
}
