using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class ExtendedGuaranteeConverter : IValueConverter<int?, string>
    {        
        public string Convert(int? source, ResolutionContext context)
        {
            if (!source.HasValue || source.Value == -1)
                return "";
            if (source.Value == 0)
                return "あり";
            return "なし";
            
        }
    }
}
