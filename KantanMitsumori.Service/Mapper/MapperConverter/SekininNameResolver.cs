using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model.Response.Report;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class SekininNameResolver : IValueResolver<TEstimate, EstimateReportModel, string>
    {
        public string Resolve(TEstimate source, EstimateReportModel destination, string destMember, ResolutionContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(source.SekininName))
                    return "責任者 :";
                if (source.SekininName.Length >= 9)
                    return $"責任者 : {source.SekininName}".Substring(0, 14);
                return $"責任者 : {source.SekininName}";
            }
            catch
            {
                return "";
            }
        }
    }
}
