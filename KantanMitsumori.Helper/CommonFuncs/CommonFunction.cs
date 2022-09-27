using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
using System.Globalization;

namespace KantanMitsumori.Helper.CommonFuncs
{
    public class CommonFunction
    {
        /// <summary>
        /// Returns the left part of this string instance.
        /// </summary>
        /// <param name="count">Number of characters to return.</param>
        public static string Left(string input, int count)
        {
            return input.Substring(0, Math.Min(input.Length, count));
        }

        /// <summary>
        /// Returns the right part of the string instance.
        /// </summary>
        /// <param name="count">Number of characters to return.</param>
        public static string Right(string input, int count)
        {
            return input.Substring(Math.Max(input.Length - count, 0), Math.Min(count, input.Length));
        }

        /// <summary>
        /// Returns the mid part of this string instance.
        /// </summary>
        /// <param name="start">Character index to start return the midstring from.</param>
        /// <returns>Substring or empty string when start is outside range.</returns>
        public static string Mid(string input, int start)
        {
            return input.Substring(Math.Min(start, input.Length));
        }

        /// <summary>
        /// Returns the mid part of this string instance.
        /// </summary>
        /// <param name="start">Starting character index number.</param>
        /// <param name="count">Number of characters to return.</param>
        /// <returns>Substring or empty string when out of range.</returns>
        public static string Mid(string input, int start, int count)
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
            if (Left(mDDate, 1) == "0")
            {
                mDDate = Right(mDDate, 1);
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
        public static void FormatDay(string strDay, ref string year, ref string month)
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
                    month = DateFormatZero(Right(strDay, 1)).Trim();
                    break;
                case 2:
                    year = CommonConst.def_Space;
                    month = DateFormatZero(Right(strDay, 2)).Trim();
                    break;
                case 4:
                    year = Left(strDay, 4).Trim();
                    month = CommonConst.def_Space;
                    break;
                case 5:
                    year = Left(strDay, 4).Trim();
                    month = DateFormatZero(Right(strDay, 1)).Trim(); ;
                    break;
                case 6:
                    year = Left(strDay, 4).Trim();
                    month = DateFormatZero(Right(strDay, 2)).Trim();
                    break;
            }
        }

        /// <summary>
        /// 日付整形2（"/"を挿入）
        /// </summary>
        /// <param name="strYM"></param>
        /// <returns></returns>
        public string FormatDay(string strYM)
        {
            strYM = strYM.Trim();

            if (strYM.IndexOf("/") > 0)
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
        public static decimal ToRoundDown(decimal dValue, int iDigits)
        {
            decimal dCoef = (decimal)Math.Pow(10, iDigits);

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
        /// 見積書データ項目 税抜／税込切替時の再計算
        /// </summary>
        /// <param name="oldVal"></param>
        /// <param name="conTaxInputKb"></param>
        /// <param name="vTax"></param>
        /// <returns></returns>
        public static int reCalcItem(long oldVal, bool conTaxInputKb, decimal vTax)
        {
            if (oldVal == 0)
            {
                return 0;
            }

            decimal wkVal;      // 浮動小数点の計算誤差回避のため
            if (conTaxInputKb == false)
            {
                // 税込 → 税抜
                wkVal = oldVal / (1 + Convert.ToDecimal(vTax));
                return Convert.ToInt32(Math.Ceiling(wkVal));
            }
            else
            {
                // 抜 → 税込
                wkVal = oldVal * (1 + Convert.ToDecimal(vTax));
                return Convert.ToInt32(Math.Floor(wkVal)); ;
            }
        }

        /// <summary>
        /// Check date format
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDate(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                DateTime dt;
                return (DateTime.TryParse(input, out dt));
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// FormatString
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatString(int? value)
        {
            if (value == 0)
            {
                return "";
            }
            else
            {
                return value.ToString()!;
            }
        }

        /// <summary>
        /// 車検有効期限 判定・編集
        /// </summary>
        /// <param name="inYM"></param>
        /// <param name="inFlagNone"></param>
        /// <returns></returns>
        public static string setCheckCarYm(string inYM, bool inFlgNone = false)
        {
            var NONE = "無し";

            int ret;

            // 「無し」フラグ On
            if (inFlgNone)
                return NONE;

            // 入力なし
            if (IsNumeric(inYM) == false)
                return "";

            if (inYM.Length == 6 && Mid(inYM, 5) == "00")
            {
                // MM が "00" の場合、YYYY 部分のみ生かす
                inYM = Left(inYM, 4);
            }

            if (inYM.Length == 4)
            {
                if (IsDate(inYM + "/01/01"))
                {
                    ret = DateDiff(IntervalEnum.Years, DateTime.Today, DateTime.Parse(inYM + "/01/01"));
                    if (ret > 3)
                        // 未来過ぎて不正なので、入力なし扱い
                        return "";
                    else if (ret < 0)
                        // 過去なので、「無し」扱い
                        return NONE;
                    else
                        return inYM;
                }
            }
            else if (inYM.Length == 6)
            {
                if (IsDate(Left(inYM, 4) + "/" + Right(inYM, 2) + "/01"))
                {
                    ret = DateDiff(IntervalEnum.Months, DateTime.Today, DateTime.Parse(Left(inYM, 4) + "/" + Right(inYM, 2) + "/01"));
                    if (ret > 36)
                        // 未来過ぎて不正なので、入力なし扱い
                        return "";
                    else if (ret < 0)
                        // 過去なので、「無し」扱い
                        return NONE;
                    else
                        return inYM;
                }
            }

            // 年月形式不正の場合、「無し」となす
            return NONE;
        }

        public static string chkImgFile(string imgPath, string strSesName, string strDefImg)
        {
            if (imgPath.Trim() == "" || File.Exists(imgPath) == false)
            {
                strSesName = strDefImg;
            }
            else
            {
                int maxIndx = imgPath.Split(@"\").GetUpperBound(0);   // 切り分けて格納した配列の最後尾を取得
                                                                      // 上で取得したファイル部分を置換してpathのみを取り出し、ファイルをgetする(最終確認の為)
                foreach (string nFileName in Directory.GetFiles(imgPath.Replace((string?)imgPath.Split(@"\").GetValue(maxIndx), "")))
                    strSesName = imgPath;
            }

            return strSesName;
        }
        public static string setFormatCurrency(object value)
        {
            var formatParm = "";
            if (Convert.ToInt32(value) == 0)
            {
                return formatParm;
            }
            else
            {
                string format = "{0:#,##0.##}";
                CultureInfo cul = new CultureInfo("en-Us");
                formatParm = string.Format(cul, format, value) + " 円";
            }
            return formatParm;
        }
        public static string setFormatCurrency(object value, string unit)
        {
            var formatParm = "";
            if (Convert.ToInt32(value) == 0)
            {
                return formatParm;
            }
            else
            {
                string format = "{0:#,##0.##}";
                CultureInfo cul = new CultureInfo("en-Us");
                formatParm = string.Format(cul, format, value) + " " + unit;
            }
            return formatParm;
        }

        /// <summary>
        /// 西暦を和暦に変換する(年のみ)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static string getWareki(string year)
        {
            if (IsNumeric(year.Trim()) == false)
            {
                return "";
            }

            int intYear = int.Parse(year);

            string retNengo = "";

            if (1926 <= intYear & intYear <= 1988)
                retNengo = "S" + Convert.ToString(intYear - 1925);
            else if (intYear <= 2018)
                retNengo = "H" + Convert.ToString(intYear - 1988);
            else if (2019 <= intYear)
                retNengo = "R" + Convert.ToString(intYear - 2018);

            return retNengo;
        }

        public static string japaneseFormat(DateTime date)
        {
            return date.ToString("yyyy") + '年' + date.ToString("MM") + '月' + date.ToString("dd") + '日';
        }

        public static string getFormatDayYMD(string strDay)
        {
            if (string.IsNullOrEmpty(strDay)) return "";

            int leDay = strDay.Replace("/", "").Length;
            string rtstrDay = "";
            switch (leDay)
            {
                case 8:
                    {
                        rtstrDay = Left(strDay.Trim(), 4) + "年" + DateFormatZero(Mid(strDay.Trim(), 5, 2)) + "月" + DateFormatZero(Right(strDay.Trim(), 2)) + "日";
                        break;
                    }
                case 6:
                    {
                        rtstrDay = Left(strDay.Trim(), 4) + "年" + DateFormatZero(Mid(strDay.Trim(), 5, 2)) + "月";
                        break;
                    }
            }

            return rtstrDay;
        }
        public static string checkwSyakenNew(int? syakenNew, int? syakenZok)
        {
            if (syakenNew > 0 && syakenZok == 0)
            {
                return syakenNew.ToString()!;

            }
            else if (syakenNew == 0 && syakenZok > 0)
            {

                return syakenZok.ToString()!;
            }
            return "";

        }
        public static string checkwSyakenValue(int? syakenNew, int? syakenZok, string inYM)
        {
            if (syakenNew > 0 && syakenZok == 0)
            {
                return "new";

            }
            else if (syakenNew == 0 && syakenZok > 0)
            {

                return "zok";
            }
            else
            {
                if (inYM.Length == 4)
                {
                    if (IsDate(inYM + "/01"))
                    {
                        var ret = DateDiff(IntervalEnum.Years, DateTime.Today, DateTime.Parse(inYM + "/01"));
                        if (ret > 0)
                        {
                            return "zok";
                        }

                    }
                }
                else if (inYM.Length == 6)
                {
                    if (IsDate(Left(inYM, 4) + "/" + Right(inYM, 2) + "/01"))
                    {
                        var ret = DateDiff(IntervalEnum.Months, DateTime.Today, DateTime.Parse(Left(inYM, 4) + "/" + Right(inYM, 2) + "/01"));
                        if (ret > 0)
                        {
                            return "zok";
                        }

                    }
                }
            }
            return "";

        }
        public static string Syaken(int? syakenNew, int? syakenZok)
        {
            if (syakenNew > 0 && syakenZok == 0)
            {
                return CommonConst.def_TitleSyakenNew;

            }
            else if (syakenNew == 0 && syakenZok > 0)
            {

                return CommonConst.def_TitleSyakenZok; ;
            }
            return "";

        }

        public static int DateDiff(IntervalEnum eInterval, DateTime dtInit, DateTime dtEnd)
        {
            if (dtEnd < dtInit)
                throw new ArithmeticException("Init date should be previous to End date.");

            switch (eInterval)
            {
                case IntervalEnum.Days:
                    return (int)(dtEnd - dtInit).TotalDays;
                case IntervalEnum.Months:
                    return ((dtEnd.Year - dtInit.Year) * 12) + dtEnd.Month - dtInit.Month;
                case IntervalEnum.Years:
                    return dtEnd.Year - dtInit.Year;
                default:
                    throw new ArgumentException("Incorrect interval code.");
            }
        }

        public static bool IsNumeric(string s)
        {
            bool value = true;
            if (string.IsNullOrEmpty(s))
            {
                value = false;
            }
            else
            {
                foreach (char c in s.ToCharArray())
                {
                    if (!char.IsDigit(c)) return false;
                }
            }
            return value;
        }

    }
}