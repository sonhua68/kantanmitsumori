using AutoMapper;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Export.Pdf.Section;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response.Report;
using KantanMitsumori.Service.Helper;
using KantanMitsumori.Service.Mapper.MapperConverter;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Xml;

namespace KantanMitsumori.Service
{
    public class ReportService : IReportService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfWorkIDE _unitOfWorkIDE;
        private readonly HelperMapper _helperMapper;
        private readonly PhysicalPathSettings _physicalPathSettings;
        private readonly URLSettings _urlSettings;

        public ReportService(IMapper mapper, ILogger<IReportService> logger, IUnitOfWork unitOfWork, IUnitOfWorkIDE unitOfWorkIDE, HelperMapper helperMapper, IOptions<PhysicalPathSettings> physicalPathSettings, IOptions<URLSettings> urlSettings)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _unitOfWorkIDE = unitOfWorkIDE;
            _helperMapper = helperMapper;
            _physicalPathSettings = physicalPathSettings.Value;
            _urlSettings = urlSettings.Value;
        }

        public ResponseBase<ReportFileModel> GenerateReport(RequestReport model)
        {
            try
            {
                // Load data
                var data = LoadReportData(model);
                if (data == null || data.Length == 0)
                    return ResponseHelper.Error<ReportFileModel>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));

                // Get report name
                var reportName = GetReportName(model.ReportType, data[0].LeaseFlag);
                // Load report
                var report = LoadReport(reportName);
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

        #region Private function
        /// <summary>
        /// Get report file name
        /// </summary>
        private string GetReportName(ReportType reportType, string leaseFlag)
        {
            if (leaseFlag == "1")
            {
                if (reportType == ReportType.Estimate)
                    return "LeaseEstimateWithMemo.rpx";
                if (reportType == ReportType.Order)
                    return "LeaseOrderWithArticle.rpx";
            }
            else
            {
                if (reportType == ReportType.Estimate)
                    return "EstimateWithMemo.rpx";
                if (reportType == ReportType.Order)
                    return "OrderWithArticle.rpx";
            }
            return "";
        }

        /// <summary>
        /// Load and convert report data from database
        /// </summary>        
        private EstimateReportModel[] LoadReportData(RequestReport input)
        {
            // Query data from database
            var estEntity = _unitOfWork.Estimates.GetSingle(i => i.EstNo == input.EstNo && i.EstSubNo == input.EstSubNo && i.Dflag == false);
            var estSubEntity = _unitOfWork.EstimateSubs.GetSingle(i => i.EstNo == input.EstNo && i.EstSubNo == input.EstSubNo && i.Dflag == false);
            var sysEntity = _unitOfWork.Syss.GetSingle(i => i.Corner == estSubEntity.Corner);
            var userEntity = _unitOfWork.UserDefs.GetSingle(i => i.UserNo == estEntity.EstUserNo && i.Dflag == false);
            var estIDEEntity = _unitOfWork.EstimateIdes.GetSingle(i => i.EstNo == input.EstNo && i.EstSubNo == input.EstSubNo);
            // Initial model
            var reportModel = new EstimateReportModel();
            // Map entity to model            
            _mapper.Map(estEntity, reportModel
                , o =>
                {
                    o.Items["estSubEntity"] = estSubEntity;
                    o.Items["sysEntity"] = sysEntity;
                    o.Items["requestReport"] = input;
                    o.Items["pathSetting"] = _physicalPathSettings.DmyImg;
                });
            _mapper.Map(estSubEntity, reportModel
                , o =>
                {
                    o.Items["estEntity"] = estEntity;
                    o.Items["sysEntity"] = sysEntity;
                    o.Items["requestReport"] = input;
                    o.Items["pathSetting"] = _physicalPathSettings.DmyImg;
                });
            if (estEntity!.LeaseFlag == "1" && estIDEEntity != null)
            {
                _mapper.Map(estIDEEntity, reportModel, o =>
                {
                    o.Items["estEntity"] = estEntity;
                    o.Items["sysEntity"] = sysEntity;
                    o.Items["pathSetting"] = _physicalPathSettings.DmyImg;
                    o.Items["requestReport"] = input;
                });
                var contractPlanEntity = _unitOfWorkIDE.ContractPlans.GetSingle(i => i.Id == estIDEEntity.ContractPlanId);
                _mapper.Map(contractPlanEntity, reportModel);
                var insuranceEntity = _unitOfWorkIDE.VoluntaryInsurances.GetSingle(i => i.Id == estIDEEntity.InsuranceCompanyId);
                _mapper.Map(insuranceEntity, reportModel);
            }
            _mapper.Map(input, reportModel);

            MapOthers(reportModel, estEntity, estSubEntity, sysEntity, userEntity);

            return new EstimateReportModel[] { reportModel };
        }

        /// <summary>
        /// Manual map for complex processing fields
        /// </summary>        
        private void MapOthers(EstimateReportModel reportModel, TEstimate estEntity, TEstimateSub estSubEntity, TbSy sysEntity, MUserDef userEntity)
        {
            UpdateSyakenNewZok(reportModel, estEntity, estSubEntity);
            UpdateUserDefInfo(reportModel, userEntity);

            // set logo
            reportModel.AutoFlatLogo = _urlSettings.AutoFlagLogoUrl;
        }

        private void UpdateUserDefInfo(EstimateReportModel reportModel, MUserDef userEntity)
        {
            if (userEntity == null)
                return;
            reportModel.AsArticle = userEntity.AsArticle.ToStringOrEmpty();
            reportModel.MemberURL = userEntity.MemberUrl.ToStringOrEmpty();
        }

        private void UpdateSyakenNewZok(EstimateReportModel reportModel, TEstimate estEntity, TEstimateSub estSubEntity)
        {
            int syaken = 0;
            int syakenNew = estEntity.SyakenNew ?? 0;
            int syakenZok = estEntity.SyakenZok ?? 0;
            if (syakenNew > 0 && syakenZok == 0)
            {
                syaken = syakenNew;
                reportModel.SyakenNewZokT = CommonConst.def_TitleSyakenNew;
            }
            else if (syakenNew == 0 && syakenZok > 0)
            {
                syaken = syakenZok;
                reportModel.SyakenNewZokT = CommonConst.def_TitleSyakenZok;
            }
            else
            {
                syaken = 0;
                reportModel.SyakenNewZokT = CommonConst.def_TitleSyakenNew;
                var expiredYm = ConverterHelper.ParseDate(estEntity.CheckCarYm);
                if (expiredYm != null)
                {
                    var timeSpan = expiredYm - DateTime.Today;
                    if (timeSpan.HasValue && timeSpan.Value.TotalDays > 0)
                        reportModel.SyakenNewZokT = CommonConst.def_TitleSyakenZok;
                }
            }
            reportModel.SyakenNew = syaken.ToYenCurrency();
        }

        #endregion

        #region Helper Functions
        /// <summary>
        /// Load report from embeded resource
        /// </summary>                
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


    }
}
