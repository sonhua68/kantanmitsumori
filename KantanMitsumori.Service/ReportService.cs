using AutoMapper;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Export.Pdf.Page;
using GrapeCity.ActiveReports.Rendering.IO;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Response.Report;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KantanMitsumori.Service
{
    public class ReportService : IReportService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IMapper mapper, ILogger<AppService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public ResponseBase<ReportFileModel> GetArticleSubReport()
        {
            var assembly = Assembly.GetCallingAssembly();
            var resourceName = "KantanMitsumori.Reports.EstSubArticle.rdlx";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))            
            {                
                PageReport report = new PageReport();
                report.Load(reader);
                report.Document.LocateDataSource += ArticleReport_LocateDataSource;                
                // Provide settings for your rendering output.
                Settings pdfSetting = new Settings();

                // Set the rendering extension and render the report.
                PdfRenderingExtension pdfRenderingExtension = new PdfRenderingExtension();
                MemoryStreamProvider outputProvider = new MemoryStreamProvider();
                report.Document.Render(pdfRenderingExtension, outputProvider, pdfSetting);
                using (var ms = outputProvider.GetPrimaryStream().OpenStream() as MemoryStream)
                    return ResponseHelper.Ok(new ReportFileModel(ms.ToArray()));
            }
        }

        private void ArticleReport_LocateDataSource(object sender, LocateDataSourceEventArgs args)
        {
            if (args.DataSet.Name == "EstPrintDs")
            {
                
                args.Data = LoadSampleData();
            }
        }

        private EstimateReportModel[] LoadSampleData()
        {
            return new EstimateReportModel[] {
                new EstimateReportModel(){
                    EstNo = "22060600001-01",
                    BusiDate = DateTime.Now.ToString("yyyy年M月d日")
                }
            };
        }


    }
}
