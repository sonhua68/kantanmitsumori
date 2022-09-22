using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Model.Response.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class AutoTaxMonthResolver : IValueResolver<TEstimate, EstimateReportModel, string>
    {
        public string Resolve(TEstimate source, EstimateReportModel destination, string destMember, ResolutionContext context)
        {
            try
            {
                var estSubEntity = context.Items["estSubEntity"] as TEstimateSub;
                if (estSubEntity == null)
                    return "";
                if (estSubEntity.AutoTaxEquivalent.HasValue && estSubEntity.AutoTaxEquivalent.Value > 0)
                    return CommonConst.def_TitleAutoTaxEquivalent;
                if (source.AutoTax.HasValue && source.AutoTax.Value > 0)
                    return $"{CommonConst.def_TitleAutoTax}（{estSubEntity.AutoTaxMonth}月中登録）";
                return CommonConst.def_TitleAutoTax;
            }
            catch
            {
                return "";
            }
        }
    }
}
