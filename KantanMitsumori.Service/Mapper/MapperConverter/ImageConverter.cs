using AutoMapper;
using KantanMitsumori.Helper.CommonFuncs;
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
            var settings = context.Items["commonSettings"] as CommonSettings;
            if (settings == null)
                throw new MissingMemberException("The settings is missing.");
            if (string.IsNullOrEmpty(imgFilePath) || !File.Exists(imgFilePath))
                return ConverterHelper.LoadImage(settings.def_DmyImg);
            return ConverterHelper.LoadImage(imgFilePath);
        }
    }
}
