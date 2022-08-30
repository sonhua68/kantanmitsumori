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

        public ResponseBase<ReportFileModel> GetMemoSubReport()
        {
            var assembly = Assembly.GetCallingAssembly();
            var resourceName = "KantanMitsumori.Reports.EstSubMemo.rpx";
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
                    MemberURL = "google.com/dang.pham",
                    SName = "testShop88888195",
                    Address = "東京都88888195",
                    Tanto = "担当 : 担当次郎hoge　　責任者 : 全角DANGPHAM",
                    CarPrice = "1,027,273 円",
                    CarName = "ﾄﾖﾀbB",
                    GradeName = "1.5 Z 煌",
                    CaseName = "QNC21",
                    ChassisNo = "12345",
                    FirstRegYm = "H28年",
                    CheckCarYm = "H28年",
                    NowOdometer = "5 千km",
                    BusinessHis = "自家用",
                    Mission = "AT5",
                    Vol = "1500 cc",
                    Color = "ブラック",
                    Equipment = "AW PS PW TV エアバッグ ナビ",
                    ConTaxInputKb = "（税抜）",
                    CarImg= LoadImage("img_1.jpg"),
                    CarImg1= LoadImage("img_2.jpg"),
                    CarImg2= LoadImage("img_3.jpg"),
                    CarImg3= LoadImage("img_4.jpg"),
                    CarImg4= LoadImage("img_5.jpg"),
                    CarImg5= LoadImage("img_6.jpg"),
                    CarImg6= LoadImage("img_7.jpg"),
                    CarImg7= LoadImage("img_8.jpg"),
                    CarImg8= LoadImage("img_9.jpg"),

                }
            };
        }

        /// <summary>
        /// Load image as based 64 string
        /// </summary>
        /// <param name="embededResource"></param>
        /// <returns></returns>
        private string LoadImage(string reportResource)
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceName = $"KantanMitsumori.Reports.{reportResource}";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}
