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
    public class JpYMDConverter : IValueConverter<string?, string>
    {
        public string Convert(string? source, ResolutionContext context)
        {
            try
            {
                if (source == null)
                    return "";
                if (source == "無し" || source == "")
                    return source;
                return ConverterHelper.GetJpDate(source);
            }
            catch
            {
                return "";
            }
        }

        
    }
}
