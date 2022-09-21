using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public static class ConverterHelper
    {
        /// <summary>
        /// Get Japanese year with Japanese Era in English
        /// </summary>        
        public static string GetWarekiEn(int year)
        {
            if (year >= 2099)
                return "";
            if (year >= 2019)
                return $"R{year - 2018}";
            if (year >= 1989)
                return $"H{year - 1988}";
            if (year >= 1926)
                return $"S{year - 1925}";
            return "";
        }
        /// <summary>
        /// Get Japanese year with Japanese Era in Japanese
        /// </summary>      
        public static string GetWarekiJp(int year)
        {
            if (year >= 2099)
                return "";
            if (year >= 2019)
                return $"令和{year - 2018:00}";
            if (year >= 1989)
                return $"平成{year - 1988:00}";
            if (year >= 1926)
                return $"昭和{year - 1925:00}";
            return "";
        }

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

        /// <summary>
        /// fromDate - toDate in Jp Date
        /// </summary>        
        public static string GetKikan(string? fromDate, string? toDate)
        {
            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                return $"{GetJpDate(fromDate)} - {GetJpDate(toDate)}";
            return "";
        }

        /// <summary>
        /// Date japanese date as yyyy年MM月 from yyyyMM
        /// </summary>        
        public static string GetJpDate(string? date)
        {
            if (string.IsNullOrEmpty(date))
                return "";
            return $"{date.Substring(0, 4)}年{date.Substring(4, 2)}月";
        }

        /// <summary>
        /// BonusFirst月・BonusSecond月
        /// </summary>        
        public static string GetBonusMonth(string? bonusFirst, string? bonusSecond)
        {
            if (!string.IsNullOrEmpty(bonusFirst) && !string.IsNullOrEmpty(bonusSecond))
                return $"{GetBonusMonth(bonusFirst)}・{GetBonusMonth(bonusSecond)}";
            return "";
        }
        public static string GetBonusMonth(string? month)
        {
            if (string.IsNullOrEmpty(month))
                return "";
            return $"{month}月";
        }

        public static string GetRateText(double? rate)
        {
            if (rate == null || rate.Value == 0)
                return "";
            return $"分割払手数料は実質年率 {rate.Value:0.0}% で計算しています";
        }
    }
}