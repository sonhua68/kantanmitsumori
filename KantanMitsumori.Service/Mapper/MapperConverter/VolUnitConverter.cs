using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class VolUnitConverter : IValueConverter<string?, string>
    {
        public string Convert(string? source, ResolutionContext context)
        {
            try
            {
                if (source == null)
                    return "";
                var estSubEntity = context.Items["estSubEntity"] as TEstimateSub;
                int vol = int.Parse(source.Replace("cc", ""));
                // Vol is zero
                if (vol == 0)
                    return "";
                // No value of unit
                if (estSubEntity == null || estSubEntity.DispVolUnit == null)
                    return $"{vol:N0}";
                else // value + unit
                    return $"{vol:N0} {estSubEntity.DispVolUnit}";
            }
            catch
            {
                return "";
            }
        }
    }
}
