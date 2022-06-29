using KantanMitsumori.Helper.Constant;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

namespace KantanMitsumori.Helper.CommonFuncs
{
    public static class CommonFunction
    {
        /// <summary>
        /// シングルクウォーテーションで囲む（SQL用）
        /// </summary>
        public static string AddQuote(string inStr)
        {
            inStr = inStr.Trim();

            if (inStr != "")
            {
                return "'" + inStr.Replace("'", "''") + "'";
            }
            else
            {
                return "Null";
            }
        }

        /// <summary>
        /// DBNullチェック
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string chkDbNull(object obj)
        {
            if (Convert.IsDBNull(obj))
            {
                return "";
            }
            else
            {
#pragma warning disable CS8603 // Possible null reference return.
                return obj.ToString();
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        /// <summary>
        /// Numeric type check
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value) => value.All(char.IsNumber);

        /// <summary>
        /// 西暦を和暦に変換する(年のみ)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static string GetWareki(string year)
        {
            if (!IsNumeric(year.Trim()))
            {
                return "";
            }

            int intYear = int.Parse(year.Trim());

            string retNengo = "";

            if (1926 <= intYear && intYear <= 1988)
            {
                retNengo = "S" + Convert.ToString(intYear - 1925);
            }
            else if (intYear <= 2018)
            {
                retNengo = "H" + Convert.ToString(intYear - 1988);
            }
            else if (2019 <= intYear)
            {
                retNengo = "R" + Convert.ToString(intYear - 2018);
            }

            return retNengo;
        }

        /// <summary>
        /// フォーマット処理
        /// </summary>
        /// <param name="text2"></param>
        /// <param name="unit"></param>
        /// <param name="formatText"></param>
        /// <returns></returns>
        public static string SetFormat(long text2, string unit, string formatText)
        {
            formatText = Convert.ToString(string.Format("#,##", text2) + unit);
            return formatText;
        }

        /// <summary>
        /// セッション変数を数値で返す
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sessionName"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int GetSessionNum(HttpContext context, string sessionName, int defValue)
        {
            var session = context.Session.Keys.Where(element => element == sessionName).FirstOrDefault();

            if (session == null || Convert.IsDBNull(session))
            {
                return defValue;
            }

            return Convert.ToInt32(Conversion.Val(session)); ;
        }

        /// <summary>
        /// セッション変数を文字列で返す
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sessionName"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static string GetSessionStr(HttpContext context, string sessionName, string defValue)
        {
            var session = context.Session.Keys.Where(element => element == sessionName).FirstOrDefault();

            if (session == null || Convert.IsDBNull(session))
            {
                return defValue;
            }

            return Convert.ToString(session);
        }

        /// <summary>
        /// Returns the left part of this string instance.
        /// </summary>
        /// <param name="count">Number of characters to return.</param>
        public static string Left(this string input, int count)
        {
            return input.Substring(0, Math.Min(input.Length, count));
        }

        /// <summary>
        /// Returns the right part of the string instance.
        /// </summary>
        /// <param name="count">Number of characters to return.</param>
        public static string Right(this string input, int count)
        {
            return input.Substring(Math.Max(input.Length - count, 0), Math.Min(count, input.Length));
        }

        /// <summary>
        /// Returns the mid part of this string instance.
        /// </summary>
        /// <param name="start">Character index to start return the midstring from.</param>
        /// <returns>Substring or empty string when start is outside range.</returns>
        public static string Mid(this string input, int start)
        {
            return input.Substring(Math.Min(start, input.Length));
        }

        /// <summary>
        /// Returns the mid part of this string instance.
        /// </summary>
        /// <param name="start">Starting character index number.</param>
        /// <param name="count">Number of characters to return.</param>
        /// <returns>Substring or empty string when out of range.</returns>
        public static string Mid(this string input, int start, int count)
        {
            return input.Substring(Math.Min(start, input.Length), Math.Min(count, Math.Max(input.Length - start, 0)));
        }

        /// <summary>
        /// 日付フォーマット（月及び日付の0を外す処理）
        /// </summary>
        /// <param name="mDDate"></param>
        /// <returns></returns>
        public static string DateFormatZero(string mDDate)
        {
            if (mDDate.Left(1) == "0")
            {
                mDDate = mDDate.Right(1);
                return mDDate;
            }
            else
            {
                return mDDate;
            }
        }

        /// <summary>
        /// 日付整形
        /// </summary>
        /// <param name="strDay"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public static void FormatDay(string strDay, string year, string month)
        {
            int leday = strDay.Replace("/", "").Length;
            switch (leday)
            {
                case 0:
                    year = CommonConst.def_Space;
                    month = CommonConst.def_Space;
                    break;
                case 1:
                    year = CommonConst.def_Space;
                    month = DateFormatZero(strDay.Right(1)).Trim();
                    break;
                case 2:
                    year = CommonConst.def_Space;
                    month = DateFormatZero(strDay.Right(2)).Trim();
                    break;
                case 4:
                    year = strDay.Left(4).Trim();
                    month = CommonConst.def_Space;
                    break;
                case 5:
                    year = strDay.Left(4).Trim();
                    month = DateFormatZero(strDay.Right(1)).Trim();
                    break;
                case 6:
                    year = strDay.Left(4).Trim();
                    month = DateFormatZero(strDay.Right(2)).Trim();
                    break;
            }
        }

        /// <summary>
        /// 日付整形2（"/"を挿入）
        /// </summary>
        /// <param name="strYM"></param>
        /// <returns></returns>
        public static string FormatDay(string strYM)
        {
            strYM = strYM.Trim();

            if (Strings.InStr(strYM, "/") > 0)
            {
                return strYM;
            }

            string retDay = "";
            switch (strYM.Length)
            {
                case 5:
                    retDay = Left(strYM, 4) + "/0" + Right(strYM, 1);
                    break;
                case 6:
                    retDay = Left(strYM, 4) + "/" + Right(strYM, 2);
                    break;
                case 8:
                    retDay = Left(strYM, 4) + "/" + Mid(strYM, 5, 2);
                    break;
                default:
                    retDay = strYM;
                    break;
            }

            return retDay;
        }

        /// <summary>
        /// 日付フォーマット（月に0を付加する）
        /// </summary>
        /// <param name="chkData"></param>
        /// <returns></returns>
        public static string DateFormat(string chkData)
        {
            if (chkData.Length == 1)
            {
                return "0" + chkData;
            }
            else
            {
                return chkData;
            }
        }

        /// <summary>
        /// 指定桁未満を切り捨て
        /// </summary>
        /// <param name="dValue"></param>
        /// <param name="iDigits"></param>
        /// <returns></returns>
        public static double ToRoundDown(double dValue, int iDigits)
        {
            double dCoef = Math.Pow(10, iDigits);

            if (dValue > 0)
            {
                return Math.Floor(dValue * dCoef) / dCoef;
            }
            else
            {
                return Math.Ceiling(dValue * dCoef) / dCoef;
            }
        }

        /// <summary>
        /// 会員番号デコード vEncNo:エンコードされた会員番号
        /// </summary>
        /// <param name="vEncNo"></param>
        /// <param name="vDecNo"></param>
        /// <returns></returns>
        //public static bool DecUserNo(string vEncNo, string vDecNo)
        //{
        //    string wOne = "";
        //    string wStr = "";
        //    string wInt = "";
        //    string wDec = "";
        //    int i = 0;

        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}



    }
}
