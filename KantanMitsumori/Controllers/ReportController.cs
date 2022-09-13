using KantanMitsumori.IService;
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
        public IActionResult DownloadArticleReport()
        {                        
            var result = _reportService.GetArticleSubReport();
            if (result == null)
                return ErrorAction(ResponseHelper.Error<int>("00000","Result is null."));
            if (result.ResultStatus != 0)
                return ErrorAction(result);
            var model = result.Data;
            if (model == null)
                return ErrorAction(ResponseHelper.Error<int>("00000", "Result data is null."));
            return File(model.Data, model.ContentType, model.Name);
        }

        /// <summary>
        /// Demo download memo sub report
        /// </summary>        
        public IActionResult DownloadMemoReport()
        {
            var result = _reportService.GetMemoSubReport();
            if (result == null)
                return ErrorAction(ResponseHelper.Error<int>("00000", "Result is null."));
            if (result.ResultStatus != 0)
                return ErrorAction(result);
            var model = result.Data;
            if (model == null)
                return ErrorAction(ResponseHelper.Error<int>("00000", "Result data is null."));
            return File(model.Data, model.ContentType, model.Name);
        }

        /// <summary>
        /// Demo download Est sub report
        /// </summary>        
        public IActionResult DownloadEstReport()
        {

            var result = _reportService.GetEstReport();
            if (result == null)
                return ErrorAction(ResponseHelper.Error<int>("00000", "Result is null."));
            if (result.ResultStatus != 0)
                return ErrorAction(result);
            var model = result.Data;
            if (model == null)
                return ErrorAction(ResponseHelper.Error<int>("00000", "Result data is null."));
            return File(model.Data, model.ContentType, model.Name);
        }


        /// <summary>
        /// Demo download Order sub report
        /// </summary>        
        public IActionResult DownloadOrderReport()
        {

            var result = _reportService.GetOrderReport();
            if (result == null)
                return ErrorAction(ResponseHelper.Error<int>("00000", "Result is null."));
            if (result.ResultStatus != 0)
                return ErrorAction(result);
            var model = result.Data;
            if (model == null)
                return ErrorAction(ResponseHelper.Error<int>("00000", "Result data is null."));
            return File(model.Data, model.ContentType, model.Name);
        }

        /// <summary>
        /// Demo download Estimate with memo report
        /// </summary>    
        public IActionResult DownloadEstimateWithMemoReport()
        {

            var result = _reportService.GetEstimateWithMemoReport();
            if (result == null)
                return ErrorAction(ResponseHelper.Error<int>("00000", "Result is null."));
            if (result.ResultStatus != 0)
                return ErrorAction(result);
            var model = result.Data;
            if (model == null)
                return ErrorAction(ResponseHelper.Error<int>("00000", "Result data is null."));
            return File(model.Data, model.ContentType, model.Name);
        }

        /// <summary>
        /// Demo download Estimate with memo report
        /// </summary>    
        public IActionResult DownloadOrderWithArticleReport()
        {

            var result = _reportService.GetOrderWithArticleReport();
            if (result == null)
                return ErrorAction(ResponseHelper.Error<int>("00000", "Result is null."));
            if (result.ResultStatus != 0)
                return ErrorAction(result);
            var model = result.Data;
            if (model == null)
                return ErrorAction(ResponseHelper.Error<int>("00000", "Result data is null."));
            return File(model.Data, model.ContentType, model.Name);
        }

    }
}
