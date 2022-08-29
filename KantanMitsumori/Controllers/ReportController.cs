using KantanMitsumori.IService;
using KantanMitsumori.Service.Helper;
using Microsoft.AspNetCore.Mvc;

namespace KantanMitsumori.Controllers
{



    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;
        public ReportController(IReportService reportService, ILogger<ReportController> logger)
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
    }
}
