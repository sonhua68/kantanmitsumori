using AutoMapper;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class ImageConverter : IValueConverter<string?, string>
    {
        /// <summary>
        /// Load image from input path and convert to base64 string
        /// </summary>
        public string Convert(string? imgFilePath, ResolutionContext context)
        {            
            if (string.IsNullOrEmpty(imgFilePath) || !File.Exists(imgFilePath))
                return ConverterHelper.LoadImage(CommonSettings.PhysicalPathSettings.def_DmyImg);
            return ConverterHelper.LoadImage(imgFilePath);
        }
    }
}
