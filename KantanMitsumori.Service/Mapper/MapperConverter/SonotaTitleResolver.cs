using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class SonotaTitleConverter : IValueConverter<string?, string>
    {
        public string Convert(string? source, ResolutionContext context)
        {
            try
            {
                var estEntity = context.Items["estEntity"] as TEstimate;
                if (estEntity == null || source == null || source == "")
                    return "";
                if (estEntity.ConTaxInputKb.HasValue && estEntity.ConTaxInputKb.Value)
                    return $"{source}{CommonConst.def_TitleInTax}";
                return $"{source}{CommonConst.def_TitleOutTax}";
            }
            catch
            {
                return "";
            }
        }
    }
}
