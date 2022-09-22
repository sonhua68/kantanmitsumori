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
    internal class CustNmResolver : IValueResolver<RequestReport, EstimateReportModel, string>
    {
        public string Resolve(RequestReport source, EstimateReportModel destination, string destMember, ResolutionContext context)
        {
            try
            {
                switch (source.ReportType)
                {
                    case ReportType.Estimate:
                    case ReportType.LeaseEstimate:
                        return $"{source.CustNm_forPrint}　様";
                    case ReportType.Order:
                    case ReportType.LeaseOrder:
                        if (source.CustNm_forPrint.Length <= 11)
                            return $"{source.CustNm_forPrint.PadRight(14, '　')}印";
                        else
                            return $"{source.CustNm_forPrint.PadRight(23, '　')}印";
                    default: throw new ArgumentException();
                }
            }
            catch
            {
                return "";
            }
        }
    }
}
