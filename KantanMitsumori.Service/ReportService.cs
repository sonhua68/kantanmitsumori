using AutoMapper;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Export.Pdf.Section;
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
            var resourceName = "KantanMitsumori.Reports.EstSubArticle.rpx";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (XmlReader reader = XmlReader.Create(stream))
            {
                SectionReport report = new SectionReport();
                report.LoadLayout(reader);                
                report.DataSource = LoadSampleData();                
                report.Run();
                PdfExport pdf = new PdfExport();                
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.Export(report.Document, ms);                    
                    return ResponseHelper.Ok(new ReportFileModel(ms.ToArray()));
                }
            }
        }

        private EstimateReportModel[] LoadSampleData()
        {
            return new EstimateReportModel[] {
                new EstimateReportModel(){
                    EstNo = "22060600001-01",
                    BusiDate = DateTime.Now.ToString("yyyy年M月d日"),
                    MemberURL = "google.com/dang.pham"
                }
            };
        }


    }
}
