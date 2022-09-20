using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model.Response.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class AutoTaxResolver : IValueResolver<TEstimate, EstimateReportModel, string>
    {
        public string Resolve(TEstimate source, EstimateReportModel destination, string destMember, ResolutionContext context)
        {
            try
            {
                var estSubEntity = context.Items["estSubEntity"] as TEstimateSub;                
                if (estSubEntity == null)
                    return "";
                if (estSubEntity.AutoTaxEquivalent.HasValue && estSubEntity.AutoTaxEquivalent.Value > 0)
                    return estSubEntity.AutoTaxEquivalent.Value.ToYenCurrency();
                return source.AutoTax.ToYenCurrency();
            }
            catch
            {
                return "";
            }
        }
    }
}
