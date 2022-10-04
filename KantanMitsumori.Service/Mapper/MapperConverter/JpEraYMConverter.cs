using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{

    /// <summary>
    /// Source format: yyyyMM, yyyy/MM
    /// Destination format: R01年12月
    /// </summary>
    public class JpEraYMConverter : IValueConverter<string?, string>
    {
        public string Convert(string? source, ResolutionContext context)
        {
            try
            {
                if (source == null)
                    return "";
                if (source == "無し" || source == "")
                    return source;
                // Standardlize source: 2022/01/01 -> 20220101
                source = source.Trim().Replace("/", "");
                // Converter source to integer
                int ym = System.Convert.ToInt32(source);
                // Source in format yyyyMM
                if (192600 <= ym && ym <= 209912)
                    return $"{ConverterHelper.GetWarekiEn(ym / 100)}年{ym % 100}月";
                // Source in format yyyyM
                if (19260 <= ym && ym <= 20999)
                    return $"{ConverterHelper.GetWarekiEn(ym / 10)}年{ym % 10}月";
                // Source in format yyyy
                if (1926 <= ym && ym <= 2099)
                    return $"{ConverterHelper.GetWarekiEn(ym)}年";
                return "";
            }
            catch
            {
                return "";
            }
        }

        
    }
}
