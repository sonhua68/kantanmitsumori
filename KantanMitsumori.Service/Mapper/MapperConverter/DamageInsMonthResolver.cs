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
    public class DamageInsMonthResolver : IValueResolver<TEstimate, EstimateReportModel, string>
    {
        public string Resolve(TEstimate source, EstimateReportModel destination, string destMember, ResolutionContext context)
        {
            try
            {
                var estSubEntity = context.Items["estSubEntity"] as TEstimateSub;
                if (estSubEntity == null)                    
                    return "";
                if (!string.IsNullOrEmpty(source.CheckCarYm) && source.CheckCarYm != "無し")
                    return CommonConst.def_TitleDamageIns;
                if (estSubEntity.DamageInsEquivalent.HasValue && estSubEntity.DamageInsEquivalent.Value > 0)
                    return CommonConst.def_TitleDamageInsEquivalent;
                if (!source.DamageIns.HasValue || source.DamageIns.Value == 0)
                    return CommonConst.def_TitleDamageIns;
                return $"{CommonConst.def_TitleDamageIns}（{estSubEntity.DamageInsMonth}ヶ月）";
            }
            catch
            {
                return "";
            }
        }
    }
}
