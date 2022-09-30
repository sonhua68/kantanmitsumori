using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public static class ConverterExtension
    {
        /// <summary>
        /// Return empty string when object is null, or return ToString with format of DateTime
        /// </summary>
        public static string ToString(this DateTime? date, string format) => date?.ToString(format) ?? "";
        /// <summary>
        /// Return empty string when object is null, or return ToString
        /// </summary>
        public static string ToStringOrEmpty<T>(this T? obj) => obj?.ToString() ?? "";
        /// <summary>
        /// Convert string to built-in type or default value
        /// </summary>        
        public static T FromStringOrDefault<T>(this string text) where T : struct
        {
            try
            {
                if(string.IsNullOrWhiteSpace(text))
                    return default;                
                if (typeof(T) == typeof(byte))
                    return (T)Convert.ChangeType(Convert.ToByte(text), typeof(T));
                if (typeof(T) == typeof(short))
                    return (T)Convert.ChangeType(Convert.ToInt16(text), typeof(T));
                if (typeof(T) == typeof(int))
                    return (T)Convert.ChangeType(Convert.ToInt32(text), typeof(T));
                if (typeof(T) == typeof(long))
                    return (T)Convert.ChangeType(Convert.ToInt64(text), typeof(T));
                if (typeof(T) == typeof(bool))
                    return (T)Convert.ChangeType(Convert.ToBoolean(text), typeof(T));                
                if (typeof(T) == typeof(double))
                    return (T)Convert.ChangeType(Convert.ToDouble(text), typeof(T));
                if (typeof(T) == typeof(float))
                    return (T)Convert.ChangeType(Convert.ToSingle(text), typeof(T));
                if (typeof(T) == typeof(decimal))
                    return (T)Convert.ChangeType(Convert.ToSingle(text), typeof(T));
                return default;
            }
            catch
            {
                return default;
            }
        }
        /// <summary>
        /// Check TradeInUm value
        /// </summary>        
        public static bool IsTradeIn(this ResolutionContext context)
        {
            var estSubEntity = context.Items["estSubEntity"] as TEstimateSub;
            if (estSubEntity == null)
                return false;
            if (!estSubEntity.TradeInUm.HasValue || estSubEntity.TradeInUm.Value == 0)
                return false;
            return true;
        }
        /// <summary>
        /// Check TradeInUm value
        /// </summary>        
        public static bool IsTaxFreeKb(this ResolutionContext context)
        {
            var estEntity = context.Items["estEntity"] as TEstimate;
            if (estEntity == null)
                return false;
            return estEntity.TaxFreeKb ?? false;
        }

        /// <summary>
        /// Check TradeInUm value
        /// </summary>        
        public static bool IsTaxCostKb(this ResolutionContext context)
        {
            var estEntity = context.Items["estEntity"] as TEstimate;
            if (estEntity == null)
                return false;
            return estEntity.TaxCostKb ?? false;
        }

        /// <summary>
        /// #,##0 円
        /// </summary>        
        public static string ToYenCurrency(this int number, string unit = " 円") => number != 0 ? $"{number:N0}{unit}" : "";

        /// <summary>
        /// #,##0 円
        /// </summary>        
        public static string ToYenCurrency(this int? number, string unit= " 円") => number.HasValue && number.Value != 0 ? $"{number.Value:N0}{unit}" : "";

       
    }
}
