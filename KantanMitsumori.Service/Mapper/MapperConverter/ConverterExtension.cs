namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public static class ConverterExtension
    {
        /// <summary>
        /// Return empty string when object is null, or return ToString
        /// </summary>
        public static string ToStringOrEmpty<T>(this T? obj) => obj?.ToString() ?? "";
        /// <summary>
        /// Return empty string when object is null or zero, or return ToString
        /// </summary>        
        /// <returns></returns>
        public static string ToStringWithNoZero(this int? obj) => obj == null || obj == 0 ? "" : obj.Value.ToString();

        /// <summary>
        /// Convert string to built-in type or default value
        /// </summary>        
        public static T FromStringOrDefault<T>(this string text) where T : struct
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
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
    }
}
