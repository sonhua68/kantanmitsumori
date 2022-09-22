using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class TradeInMiUnitConverter : IValueConverter<int?, string>
    {
        public string Convert(int? source, ResolutionContext context)
        {
            try
            {
                var estSubEntity = context.Items["estSubEntity"] as TEstimateSub;
                // No value or zero
                if (!source.HasValue || source.Value == 0)
                    return "";
                // No value of unit
                if (estSubEntity == null || estSubEntity.TradeInMilUnit == null)
                    return $"{source.Value:N0}";
                // value + unit                
                return $"{source.Value:N0} {estSubEntity.TradeInMilUnit}";
            }
            catch
            {
                return "";
            }
        }
    }
}
