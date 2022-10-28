using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Model.Response.Report;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class DamageInsMonthResolver : IValueResolver<TEstimate, EstimateReportModel, string>
    {
        public string Resolve(TEstimate source, EstimateReportModel destination, string destMember, ResolutionContext context)
        {
            try
            {
                var estSubEntity = context.Items["estSubEntity"] as TEstimateSub;
                if (estSubEntity == null)
                    return "";
                if (estSubEntity.DamageInsEquivalent.HasValue && estSubEntity.DamageInsEquivalent.Value > 0)
                    return CommonConst.def_TitleDamageInsEquivalent;
                return $"{CommonConst.def_TitleDamageIns}（{estSubEntity.DamageInsMonth}ヶ月）";
            }
            catch
            {
                return "";
            }
        }
    }
}
