using AutoMapper;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response.Report;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    internal class CustNmResolver : IValueResolver<RequestReport, EstimateReportModel, string>
    {
        public string Resolve(RequestReport source, EstimateReportModel destination, string destMember, ResolutionContext context)
        {
            try
            {
                if (source.CustNm_forPrint.Trim() != "")
                {
                    switch (source.ReportType)
                    {
                        case ReportType.Estimate:
                            return $"{source.CustNm_forPrint}　様";
                        case ReportType.Order:
                            if (source.CustNm_forPrint.Length <= 11)
                                return $"{source.CustNm_forPrint.PadRight(14, '　')}印";
                            else
                                return $"{source.CustNm_forPrint.PadRight(23, '　')}印";
                        default: throw new ArgumentException();
                    }
                }
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }
    }
}
