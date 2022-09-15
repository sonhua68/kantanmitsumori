using AutoMapper;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Export.Pdf.Section;
using GrapeCity.ActiveReports.Rendering.IO;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
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
        
        public ResponseBase<ReportFileModel> GenerateEstimateReport(RequestReport model)
        {
            try
            {
                // Load report
                var report = LoadReport("EstimateWithMemo.rpx");
                // Load data
                var data = LoadReportData(model);
                if (data == null || data.Length == 0)
                    return ResponseHelper.Error<ReportFileModel>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                // Bind data
                report.DataSource = data;
                // Generate report
                report.Run();
                // Generate pdf in binary data
                PdfExport pdf = new PdfExport();
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.Export(report.Document, ms);
                    return ResponseHelper.Ok("", "", new ReportFileModel(ms.ToArray()));
                }
            }
            catch
            {
                return ResponseHelper.Error<ReportFileModel>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(HelperMessage.CEST050S));
            }
        }

        public ResponseBase<ReportFileModel> GetArticleSubReport()
        {
            var assembly = Assembly.GetEntryAssembly();
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
                    return ResponseHelper.Ok("","",new ReportFileModel(ms.ToArray()));
                }
            }
        }

        public ResponseBase<ReportFileModel> GetMemoSubReport()
        {
            var assembly = Assembly.GetEntryAssembly();
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
                    return ResponseHelper.Ok("", "", new ReportFileModel(ms.ToArray()));
                }
            }
        }

        public ResponseBase<ReportFileModel> GetEstReport()
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceName = "KantanMitsumori.Reports.Estimate.rpx";
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
                    return ResponseHelper.Ok("", "", new ReportFileModel(ms.ToArray()));
                }
            }
        }

        public ResponseBase<ReportFileModel> GetOrderReport()
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceName = "KantanMitsumori.Reports.Order.rpx";
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
                    return ResponseHelper.Ok("", "", new ReportFileModel(ms.ToArray()));
                }
            }
        }

        public ResponseBase<ReportFileModel> GetEstimateWithMemoReport()
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceName = "KantanMitsumori.Reports.EstimateWithMemo.rpx";
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
                    return ResponseHelper.Ok("", "", new ReportFileModel(ms.ToArray()));
                }
            }
        }

        public ResponseBase<ReportFileModel> GetOrderWithArticleReport()
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceName = "KantanMitsumori.Reports.OrderWithArticle.rpx";
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
                    return ResponseHelper.Ok("", "", new ReportFileModel(ms.ToArray()));
                }
            }
        }

        private EstimateReportModel[] LoadSampleData()
        {
            return new EstimateReportModel[] {
                new EstimateReportModel(){
                    AAInfo = "",
                    AccidentHis = "",
                    AcqTax = "",
                    Address = "東京都88888195",
                    AutoTax = "17,200 円",
                    AutoTaxMonth = "自動車税（9月中登録）",
                    Balance = "",
                    BalanceTitle = "下取車残債",
                    BonusAmount = "",
                    BonusMonth = "",
                    BusiDate = "2022年9月2日",
                    BusinessHis = "自家用",
                    CarKei = "1,087,383 円",
                    CarName = "ﾄﾖﾀbB",
                    CarPrice = "1,027,273 円",
                    CarPriceTitle = "車両本体価格（税抜）",
                    CarSaleKei = "1,241,263 円",
                    CarSaleKeiTitle = "現金販売価格（6～10）",
                    CaseName = "QNC21",
                    ChassisNo = "12345",
                    CheckCarYm = "H28年",
                    Color = "ブラック",
                    ConTax = "115,738 円",
                    ConTaxInputKb = "（税抜）",
                    CustAdr_forPrint = "236/43/2 DIEN BIEN PHU",
                    CustNm_forPrint = "DANG PHAM　様",
                    CustTel_forPrint = "（TEL : 0908324866）",
                    CustZip_forPrint = "〒70201",
                    DaikoAll = "70,000 円",
                    DaikoTitle = "[4]手続代行費用（税抜）",
                    DamageIns = "10,000 円",
                    DamageInsMonth = "自賠責保険料（25ヶ月）",
                    Deposit = "",
                    Discount = "",
                    Equipment = "AW PS PW TV エアバッグ ナビ",
                    EstNo = "22060600001-01",
                    FirstPayAmount = "",
                    FirstRegYm = "H28年",
                    GradeName = "1.5 Z 煌",
                    Kikan = "10,000 円",
                    Mission = "AT5",
                    NebikiTitle = "",
                    Notes = "Notes",
                    NowOdometer = "5 千km",
                    OpSpeCialTitle = "[1]付属品・特別仕様（税抜）",
                    OptionIns = "",
                    OptionName1 = "ＥＴＣ車載",
                    OptionName10 = "カーナビゲーション",
                    OptionName11 = "スタッドレスタイヤ",
                    OptionName12 = "ＥＴＣ車載",
                    OptionName2 = "ドライブレコーダー",
                    OptionName3 = "カーナビゲーション",
                    OptionName4 = "スタッドレスタイヤ",
                    OptionName5 = "ＥＴＣ車載",
                    OptionName6 = "ドライブレコーダー",
                    OptionName7 = "カーナビゲーション",
                    OptionName8 = "スタッドレスタイヤ",
                    OptionName9 = "ＥＴＣ車載",
                    OptionPrice1 = "10,000 円",
                    OptionPrice10 = "10,000 円",
                    OptionPrice11 = "10,000 円",
                    OptionPrice12 = "10,000 円",
                    OptionPrice2 = "10,000 円",
                    OptionPrice3 = "10,000 円",
                    OptionPrice4 = "10,000 円",
                    OptionPrice5 = "10,000 円",
                    OptionPrice6 = "10,000 円",
                    OptionPrice7 = "10,000 円",
                    OptionPrice8 = "10,000 円",
                    OptionPrice9 = "10,000 円",
                    OptionPriceAll = "40,000 円",
                    PartitionAmount = "20,000 円",
                    PartitionFee = "20,000 円",
                    PayAmount = "20,000 円",
                    PayTimes = "",
                    Principal = "",                    
                    Rate = "",
                    RevenueStampTitle = "",
                    SaleAll = "1,196,001 円",
                    SalesSumTitle = "お支払総額（11～14）",
                    SekininName = "責任者 : 全角DANGPHAM",
                    SName = "testShop88888195",
                    Sonota = "20,000 円",
                    SonotaTitle = "その他費用（税抜）",
                    SyakenNew = "110 円",
                    SyakenNewZokT = "車検整備費用（税抜）",
                    Tanto = "担当 : 担当次郎hoge",
                    TaxCheck = "20,000 円",
                    TaxDelivery = "20,000 円",
                    TaxFreeAll = "40,000 円",
                    TaxFreeCheck1 = "20,000 円",
                    TaxFreeGarage = "20,000 円",
                    TaxFreeOther = "20,000 円",
                    TaxFreeRecycle = "20,000 円",
                    TaxFreeSet1 = "20,000 円",
                    TaxFreeSet1Title = "納車費用1",
                    TaxFreeSet2 = "20,000 円",
                    TaxFreeSet2Title = "納車費用2",
                    TaxFreeTradeIn = "20,000 円",
                    TaxGarage = "20,000 円",
                    TaxInsAll = "43,880 円",
                    TaxInsEquivalentAll = "20,000 円",
                    TaxInsEquivalentTitle = "[2]税金・保険料相当額抜）",
                    TaxOther = "20,000 円",
                    TaxRecycle = "20,000 円",
                    TaxSet1 = "20,000 円",
                    TaxSet1Title = "下取車手続・処分1",
                    TaxSet2 = "20,000 円",
                    TaxSet2Title = "下取車手続・処分2",
                    TaxSet3 = "20,000 円",
                    TaxSet3Title = "下取車手続・処分3",
                    TaxTitle = "費税合計",
                    TaxTradeIn = "20,000 円",
                    TaxTradeInSatei = "20,000 円",
                    Tel = "TEL : 0532-57-5554",
                    TradeInBodyColor = "下取",
                    TradeInCarName = "下取",
                    TradeInChassisNo = "下取",
                    TradeInCheckCarYm = "H28年",
                    TradeInFirstRegYm = "H28年",
                    TradeInNowOdometer = "",
                    TradeInPrice = "115,738 円",
                    TradeInRegNo = "",
                    Vol = "1500 cc",
                    WeightTax = "20,000 円",
                    

                    CarImg = LoadImage("img_1.jpg"),
                    CarImg1 = LoadImage("img_2.jpg"),
                    CarImg2 = LoadImage("img_3.jpg"),
                    CarImg3 = LoadImage("img_4.jpg"),
                    CarImg4 = LoadImage("img_5.jpg"),
                    CarImg5 = LoadImage("img_6.jpg"),
                    CarImg6 = LoadImage("img_7.jpg"),
                    CarImg7 = LoadImage("img_8.jpg"),
                    CarImg8 = LoadImage("img_9.jpg"),
                }
            };
        }


        #region Helper Functions

        /// <summary>
        /// Load image as based 64 string
        /// </summary>
        /// <param name="embededResource"></param>
        /// <returns></returns>
        private string LoadImage(string resourceName)
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourcePath = $"KantanMitsumori.Reports.{resourceName}";
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private SectionReport LoadReport(string reportFilename)
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceName = $"KantanMitsumori.Reports.{reportFilename}";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (XmlReader reader = XmlReader.Create(stream))
            {
                SectionReport report = new SectionReport();
                report.LoadLayout(reader);
                return report;
            }            
        }


        #endregion

        #region Private function

        /// <summary>
        /// Load and convert report data from database
        /// </summary>        
        private EstimateReportModel[] LoadReportData(RequestReport input)
        {
            return null;
        }

        #endregion
    }
}
