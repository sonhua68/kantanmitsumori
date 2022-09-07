using KantanMitsumori.Helper.Constant;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KantanMitsumori.Helper.Utility
{
    public class KantanMitsumoriUtil
    {
        #region String Process
        public static string LeftWid(string inStr, int width)
        {
            return MidWid(inStr, 0, width);
        }

        public static string MidWid(string inStr, int startIndex, int width)
        {
            var asciiCodes = System.Text.ASCIIEncoding.UTF8.GetBytes(inStr);
            var rtnString = "";
            if (asciiCodes.Length <= startIndex)
            {
                return rtnString;
            }
            // Get the range from startIndex to max length of string
            var newRange = asciiCodes.Skip(startIndex).ToArray();
            if (newRange.Length <= width)
            {
                return System.Text.ASCIIEncoding.UTF8.GetString(newRange);
            }
            else
            {
                var getLength = 0;
                var interWidth = 1;
                // Get the length of param width
                for (var i = 0; i < newRange.Length; i++)
                {
                    if (newRange[i] <= 255)
                    {
                        getLength += 1;
                    }
                    else
                    {
                        getLength += 2;
                        interWidth = 2;
                    }
                    if (getLength <= width)
                    {
                        rtnString += System.Text.ASCIIEncoding.UTF8.GetString(newRange, i, interWidth);
                    }
                    else
                    {
                        break;
                    }
                }
                return rtnString;
            }
        }

        public static string RightWid(string inStr, int width)
        {
            // start index is 0
            return MidWid(inStr, LenByte(inStr) - width + 1, width);
        }

        public static int LenByte(string inStr)
        {
            var asciiCodes = System.Text.ASCIIEncoding.UTF8.GetBytes(inStr);
            var lenStr = 0;
            for (var i = 0; i < asciiCodes.Length; i++)
            {
                // Single byte 255 bit
                if (asciiCodes[i] <= 255)
                {
                    lenStr += 1;
                }
                else
                {
                    // Double bytes for japanese
                    lenStr += 2;
                }
            }
            return lenStr;
        }

        public static DateTime ConvertDateTime(string dateTime)
        {
            var year = dateTime.Substring(0, 4);
            var month = dateTime.Substring(4, 2);
            var day = dateTime.Substring(6, 2);
            return Convert.ToDateTime($"{year}/{month}/{day}");
        }

        public static string ConvertToHourFormat(string hour, bool blankType = true, bool leadingZero = true)
        {
            if (string.IsNullOrWhiteSpace(hour) || hour.Length < 4 || Convert.ToInt32(hour) == 0)
            {
                if (blankType)
                {
                    return "";
                }
                return "-";
            }
            var objHour = Convert.ToInt32(hour.Substring(0, hour.Length - 2));
            var objMinute = hour.Substring(hour.Length - 2);
            if (leadingZero)
            {
                return $"{objHour.ToString().PadLeft(2, '0')}:{objMinute.PadLeft(2, '0')}";
            }
            return $"{objHour.ToString()}:{objMinute.PadLeft(2, '0')}";
        }

        public static string SumTime(string FirstTime, string SecondTime, int maxLength = 4)
        {
            if (FirstTime == null)
            {
                if (SecondTime == null)
                {
                    return "".PadLeft(maxLength, '0');
                }
                else
                {
                    return SecondTime;
                }
            }
            if (SecondTime == null)
            {
                return FirstTime;
            }
            var sumHour = Convert.ToInt32(FirstTime.Substring(0, 2)) + Convert.ToInt32(SecondTime.Substring(0, 2));
            var sumMinutes = Convert.ToInt32(FirstTime.Substring(2, 2)) + Convert.ToInt32(SecondTime.Substring(2, 2));
            if (sumMinutes >= 60)
            {
                sumHour += sumMinutes / 60;
                sumMinutes = sumMinutes % 60;
            }
            if (maxLength == 4)
            {
                return $"{sumHour.ToString().PadLeft(2, '0')}{sumMinutes.ToString().PadLeft(2, '0')}";
            }
            // MaxLength = 5
            return $"{sumHour.ToString().PadLeft(3, '0')}{sumMinutes.ToString().PadLeft(2, '0')}";
        }

        public static string SubtractTime(string BigTime, string SmallTime)
        {
            var bigHour = BigTime.Length > 4 ? BigTime.Substring(0, 3) : BigTime.Substring(0, 2);
            var bigMinutes = BigTime.Length > 4 ? BigTime.Substring(3, 2) : BigTime.Substring(2, 2);
            var smallHour = SmallTime.Length > 4 ? SmallTime.Substring(0, 3) : SmallTime.Substring(0, 2);
            var smallMinutes = SmallTime.Length > 4 ? SmallTime.Substring(3, 2) : SmallTime.Substring(2, 2);
            var subHour = Convert.ToInt32(bigHour) - Convert.ToInt32(smallHour);
            var subMinutes = Convert.ToInt32(bigMinutes) - Convert.ToInt32(smallMinutes);
            if (subMinutes < 0)
            {
                subHour -= 1;
                subMinutes = 60 + subMinutes;
            }
            return $"{subHour.ToString().PadLeft(3, '0')}{subMinutes.ToString().PadLeft(2, '0')}";
        }

        public static string GetMessage(string locale, string code)
        {
            switch (locale)
            {
                //case "vn":
                //    return GetVietnamMessage(code);
                case "jp":
                    return GetJapanMessage(code);
                default:
                    return GetJapanMessage(code);
            }
        }
    

        //public static string GetEnglishMessage(string code)
        //{
        //    var message = "";
        //    MessageEnglish.English.TryGetValue(code, out message);
        //    return message;
        //}

        public static string GetJapanMessage(string code)
        {
            var message = "";
            MessageJapan.Japan.TryGetValue(code, out message);
            return message;
        }

        //public static string GetVietnamMessage(string code)
        //{
        //    var message = "";
        //    MessageVietnam.Vietnam.TryGetValue(code, out message);
        //    return message;
        //}

        public static string RemoveVietnameseTone(string text)
        {
            string result = text.ToLower();
            result = Regex.Replace(result, "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|/g", "a");
            result = Regex.Replace(result, "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|/g", "e");
            result = Regex.Replace(result, "ì|í|ị|ỉ|ĩ|/g", "i");
            result = Regex.Replace(result, "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|/g", "o");
            result = Regex.Replace(result, "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|/g", "u");
            result = Regex.Replace(result, "ỳ|ý|ỵ|ỷ|ỹ|/g", "y");
            result = Regex.Replace(result, "đ", "d");
            return result;
        }
        #endregion

        #region Date Process
        public static decimal CalculateTotalDay(int applyOffDay, int StartWorkYear, int remainYearBefore = 0)
        {
            decimal totalDay = 12;
            if (DateTime.Now.Year == StartWorkYear)
            {
                totalDay = totalDay - DateTime.Now.Month + (DateTime.Now.Day < applyOffDay ? 1 : 0);
            }
            else
            {
                int difYear = DateTime.Now.Year - StartWorkYear;
                if (difYear >= 5)
                {
                    int increment = Convert.ToInt32(Math.Truncate(difYear / 5f));
                    totalDay += increment;
                }
            }
            if (DateTime.Now.Year == StartWorkYear + 1)
            {
                totalDay += remainYearBefore;
            }
            return totalDay;
        }

        public static bool isWeekEnd(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;
        }
        public static string GetId()
        {
            string _id = "";
            Guid guid1 = default(Guid);
            guid1 = Guid.NewGuid();
            _id = guid1.ToString();
            return _id;
        }
        public static DateTime DateToDDMMYYY(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                return DateTime.ParseExact(date, "dd-MM-yyyy",
                                      System.Globalization.CultureInfo.InvariantCulture);
            }
            return new DateTime();
        }
        public static string GetMonthInPeriod(int MonthInPeriod, int Period)
        {
            string _temp = "";
            for (int i = 0; i <= MonthInPeriod - 1; i++)
            {
                _temp += (i + 1 + (Period - 1) * MonthInPeriod).ToString() + "','";
            }
            _temp = _temp.Substring(0, _temp.Length - 3);

            return _temp;
        }
        public static int GetWeekOfYear(DateTime time)
        {
            try
            {
                DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
                if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                {
                    time = time.AddDays(3);
                }
            }
            catch { }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public static int GetYearOfDateByWeek(DateTime time)
        {
            try
            {
                if (GetWeekOfYear(time) <= 1 && time.Month >= 12)
                {
                    return time.Year + 1;
                }
                else if (GetWeekOfYear(time) >= 52 && time.Month <= 1)
                {
                    return time.Year - 1;
                }
                else { return time.Year; }
            }
            catch { return time.Year; }
        }
        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            var result = default(DateTime);
            try
            {
                DateTime jan1 = new DateTime(year, 1, 1);
                int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

                DateTime firstThursday = jan1.AddDays(daysOffset);
                var cal = CultureInfo.CurrentCulture.Calendar;
                int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                var weekNum = weekOfYear;
                if (firstWeek <= 1)
                {
                    weekNum -= 1;
                }
                result = firstThursday.AddDays(weekNum * 7);
            }
            catch { result = default(DateTime).AddDays(3); }
            return result.AddDays(-3);
        }
        public static string GetBeginDateOfMonth()
        {
            return new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
        }
        public static string GetBeginDateOfMonthyyyyMMdd()
        {
            return new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyyMMdd");
        }
        public static string GetDateOfMonthyyyyMMdd()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }
        public static string GetEndDateOfMonth()
        {
            System.DateTime d = new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return d.AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");
        }
        public static string GetDateOfNow()
        {
            return DateTime.Now.ToShortDateString();
        }
        public static string GetBeginDateOfYear()
        {
            return new System.DateTime(DateTime.Now.Year, 1, 1).ToShortDateString();
        }
        public static string GetEndDateOfYear()
        {
            System.DateTime d = new System.DateTime(DateTime.Now.Year, 12, 1);
            return d.AddMonths(1).AddDays(-1).ToShortDateString();
        }
        public static string GetYearNow()
        {
            return DateTime.Now.Year.ToString();
        }
        public static string GetMonthNow()
        {
            return String.Format("{0:D2}", DateTime.Now.Month);
        }
        public static string GetDayNow()
        {
            return String.Format("{0:D2}", DateTime.Now.Day);
        }
        public static string GetTimeHHMM()
        {
            return DateTime.Now.ToString("HHmm");
        }
        public static string DateIsddMMMyyyy(object DateString)
        {
            try
            {
                DateTime DateD = Convert.ToDateTime(DateString.ToString());
                return DateD.ToString("dd/MMM/yyyy");
            }
            catch
            {
                return "";
            }
        }
        public static string DateIsddMMMyy(object DateString)
        {
            try
            {
                DateTime DateD = Convert.ToDateTime(DateString.ToString());
                return DateD.ToString("dd/MMM/yy");
            }
            catch
            {
                return "";
            }
        }
        public static string DateIsMMMddyy(object DateString)
        {
            try
            {
                DateTime DateD = Convert.ToDateTime(DateString.ToString());
                return DateD.ToString("MMM/dd/yy");
            }
            catch
            {
                return "";
            }
        }
        public static string DateIsMMMddyyyy(object DateString)
        {
            try
            {
                DateTime DateD = Convert.ToDateTime(DateString.ToString());
                return DateD.ToString("MMM/dd/yyyy");
            }
            catch
            {
                return "";
            }
        }
        public static string DateIsddMMyyyy(object DateString)
        {
            try
            {
                DateTime DateD = Convert.ToDateTime(DateString.ToString());
                return DateD.ToString("dd/MM/yyyy");
            }
            catch
            {
                return "";
            }
        }
        public static string DateIsddMMyy(object DateString)
        {
            try
            {
                DateTime DateD = Convert.ToDateTime(DateString.ToString());
                return DateD.ToString("dd/MM/yy");
            }
            catch
            {
                return "";
            }
        }
        public static string DateIsMMddyy(object DateString)
        {
            try
            {
                DateTime DateD = Convert.ToDateTime(DateString.ToString());
                return DateD.ToString("MM/dd/yy");
            }
            catch
            {
                return "";
            }
        }
        public static string DateIsMMddyyyy(object DateString)
        {
            try
            {
                DateTime DateD = Convert.ToDateTime(DateString.ToString());
                return DateD.ToString("MM/dd/yyyy");
            }
            catch
            {
                return "";
            }
        }


        public static object ConvertToDateTime(object objectValue)
        {
            try
            {
                DateTime d1 = Convert.ToDateTime(objectValue);
                if (d1.Year == 1900 || d1.Year == 1000 || d1.Year == 1)
                {
                    return DBNull.Value;
                }
                else
                {
                    return Convert.ToDateTime(d1);
                }
            }
            catch
            {
                return DBNull.Value;
            }

        }
        public static string ConvertToDateTimeOrEmpty(object objectValue)
        {
            try
            {
                DateTime d1 = Convert.ToDateTime(objectValue);
                if (d1.Year == 1900 || d1.Year == 1000 || d1.Year == 1)
                {
                    return "";
                }
                else
                {
                    return Convert.ToDateTime(d1).ToShortDateString();
                }
            }
            catch
            {
                return "";
            }

        }
        #endregion

     
    }
}
