namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public static class ConverterHelper
    {
        /// <summary>
        /// Load image from path return base64 string
        /// </summary>        
        public static string LoadImage(string filePath)
        {
            try
            {
                return Convert.ToBase64String(File.ReadAllBytes(filePath));
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Parse date from yyyymm, yyyym, yyyy/mm, yyyy/m
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static DateTime? ParseDate(string? dateStr)
        {
            try
            {
                if (string.IsNullOrEmpty(dateStr))
                    return null;
                // Standardlize source: 2022/01 -> 202201
                dateStr = dateStr.Trim().Replace("/", "");
                // Converter source to integer
                int ym = System.Convert.ToInt32(dateStr);
                // Source in format yyyyMM
                if (192600 <= ym && ym <= 209912)
                    return new DateTime(ym / 100, ym % 100, 1);
                // Source in format yyyyM
                if (19260 <= ym && ym <= 20999)
                    return new DateTime(ym / 10, ym % 10, 1);
                // Source in format yyyy
                if (1926 <= ym && ym <= 2099)
                    return new DateTime(ym, 1, 1);
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}