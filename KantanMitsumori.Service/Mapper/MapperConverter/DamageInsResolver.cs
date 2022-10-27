using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model.Response.Report;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class DamageInsResolver : IValueResolver<TEstimate, EstimateReportModel, string>
    {
        public string Resolve(TEstimate source, EstimateReportModel destination, string destMember, ResolutionContext context)
        {
            try
            {
                var estSubEntity = context.Items["estSubEntity"] as TEstimateSub;
                if (estSubEntity == null)
                    return "";
                if (estSubEntity.DamageInsEquivalent.HasValue && estSubEntity.DamageInsEquivalent.Value > 0)
                    return estSubEntity.DamageInsEquivalent.Value.ToYenCurrency();
                return source.DamageIns.ToYenCurrency();
            }
            catch
            {
                return "";
            }
        }
    }
}
