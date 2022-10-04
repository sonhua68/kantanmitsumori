using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class TantoResolver : IValueResolver<TEstimate, EstimateReportModel, string>
    {
        public string Resolve(TEstimate source, EstimateReportModel destination, string destMember, ResolutionContext context)
        {
            try
            {
                var requestReport = context.Items["requestReport"] as RequestReport;
                if (requestReport == null)
                    return "";
                switch (requestReport.ReportType)
                {
                    case ReportType.Estimate:                    
                        return GetEstimateTanto(source);
                    case ReportType.Order:                    
                        var tanto = $"担当 : {source.EstTanName}";
                        if (tanto.Length > 23)
                            return tanto.Substring(0, 23);
                        return tanto;                            
                    default:
                        return "";
                }

            }
            catch
            {
                return "";
            }
        }
        private string GetEstimateTanto(TEstimate estEntity)
        {
            var tanto = "";
            var sekinin = "";
            var tel = "";
            if (!string.IsNullOrWhiteSpace(estEntity.EstTanName))
                tanto = $"担当 : {estEntity.EstTanName}　　";
            if (!string.IsNullOrWhiteSpace(estEntity.SekininName))
                sekinin = $"責任者 : {estEntity.SekininName}　　";
            if (!string.IsNullOrWhiteSpace(estEntity.ShopTel))
                tel = $"TEL : {estEntity.ShopTel}";
            if(tanto.Length + sekinin.Length + tel.Length > 42)
                return $"{tanto}{sekinin}\r\n{tel}";
            return $"{tanto}{sekinin}{tel}";
        }
    }
}
