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
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Entity.IDEEnitities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using KantanMitsumori.Service.Mapper.MapperConverter;

namespace KantanMitsumori.Service
{
    public class ReportService : IReportService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfWorkIDE _unitOfWorkIDE;
        private readonly HelperMapper _helperMapper;
        private readonly CommonSettings _commonSettings;

        public ReportService(IMapper mapper, ILogger<ReportService> logger, IUnitOfWork unitOfWork, IUnitOfWorkIDE unitOfWorkIDE, HelperMapper helperMapper, CommonSettings commonSettings)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _unitOfWorkIDE = unitOfWorkIDE;
            _helperMapper = helperMapper;
            _commonSettings = commonSettings;
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

        public ResponseBase<ReportFileModel> GenerateOrderReport(RequestReport model)
        {
            try
            {
                // Load report
                var report = LoadReport("OrderWithArticle.rpx");
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
            // Query data from database
            var estEntity = _unitOfWork.Estimates.GetSingle(i => i.EstNo == input.EstNo && i.EstSubNo == input.EstSubNo && i.Dflag == false);
            var estSubEntity = _unitOfWork.EstimateSubs.GetSingle(i => i.EstNo == input.EstNo && i.EstSubNo == input.EstSubNo && i.Dflag == false);
            var sysEntity = _unitOfWork.Syss.GetSingle(i => i.Corner == estSubEntity.Corner);
            var userEntity = _unitOfWork.UserDefs.GetSingle(i => i.UserNo == estEntity.EstUserNo && i.Dflag == false);
            // Initial model
            var reportModel = new EstimateReportModel();
            // Map entity to model            
            _mapper.Map(estEntity, reportModel
                , o => {
                    o.Items["estSubEntity"] = estSubEntity;
                    o.Items["sysEntity"] = sysEntity;
                    o.Items["commonSettings"] = _commonSettings;
                });
            _mapper.Map(estSubEntity, reportModel
                , o => {
                    o.Items["estEntity"] = estEntity;
                    o.Items["sysEntity"] = sysEntity;
                    o.Items["commonSettings"] = _commonSettings;
                });
            _mapper.Map(input, reportModel);

            MapOthers(reportModel, estEntity, estSubEntity, sysEntity, userEntity);

            return new EstimateReportModel[] { reportModel };
        }

        private void MapOthers(EstimateReportModel reportModel, TEstimate estEntity, TEstimateSub estSubEntity, TbSy sysEntity, MUserDef userEntity)
        {
            UpdateSyakenNewZok(reportModel, estEntity, estSubEntity);
            UpdateUserDefInfo(reportModel, userEntity);
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
                if(expiredYm != null)
                {
                    var timeSpan = expiredYm - DateTime.Today;
                    if(timeSpan.HasValue && timeSpan.Value.TotalDays > 0)
                        reportModel.SyakenNewZokT = CommonConst.def_TitleSyakenZok;
                }
            }
            reportModel.SyakenNew = syaken.ToYenCurrency();
        }
        #endregion
    }
}
