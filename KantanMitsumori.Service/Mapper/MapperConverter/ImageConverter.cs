using AutoMapper;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class ImageConverter : IValueConverter<string?, string>
    {
        /// <summary>
        /// Load image from input path and convert to base64 string
        /// </summary>
        public string Convert(string? imgFilePath, ResolutionContext context)
        {
            try
            {
                var defaultImgPath = context.Items["pathSetting"] as string;
                if (string.IsNullOrEmpty(imgFilePath) || !File.Exists(imgFilePath))
                    return ConverterHelper.LoadImage(defaultImgPath!);
                return ConverterHelper.LoadImage(imgFilePath);
            }
            catch
            {
                return "";
            }
        }
    }
}
