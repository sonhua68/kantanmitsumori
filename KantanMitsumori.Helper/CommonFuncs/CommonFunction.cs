
using KantanMitsumori.Helper.Constant;
using Microsoft.VisualBasic;

namespace KantanMitsumori.Helper.CommonFuncs
{
    public class CommonFunction
    {
        /// <summary>
        /// 西暦を和暦に変換する(年のみ)
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetWareki(string year)
        {
            if (!Information.IsNumeric(year.Trim()))
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
        /// 日付の"/"を外す
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public string DateFormatReplace(string date)
        {
            if (Strings.InStr(1, date, "/") > 0)
            {
                date = date.Replace("/", "");
                return date;
            }
            else
            {
                return date;
            }
        }

        /// <summary>
        /// Check date format
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsDate(string input)
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
        public string SetCheckCarYm(string inYM, bool inFlgNone = false)
        {
            string none = "無し";

            long ret;

            // 「無し」フラグ On
            if (inFlgNone)
            {
                return none;
            }

            // 入力なし
            if (inYM == "" || Information.IsNumeric(inYM))
            {
                return "";
            }

            if (inYM.Length == 6 && Mid(inYM, 5) == "00")
            {
                //MM が "00" の場合、YYYY 部分のみ生かす
                inYM = Left(inYM, 4);
            }

            if (inYM.Length == 4)
            {
                if (IsDate(inYM + "/01/01"))
                {
                    ret = DateAndTime.DateDiff(DateInterval.Year, DateTime.Today, DateTime.Parse(inYM + "/01/01"));
                    if (ret > 3)
                    {
                        // 未来過ぎて不正なので、入力なし扱い
                        return "";
                    }
                    else if (ret < 0)
                    {
                        // 過去なので、「無し」扱い
                        return none;
                    }
                    else
                    {
                        return inYM;
                    }
                }
            }
            else if (inYM.Length == 6)      // 年月の場合
            {
                if (IsDate(Left(inYM, 4) + "/" + Right(inYM, 2) + "/01"))
                {
                    ret = DateAndTime.DateDiff(DateInterval.Month, DateTime.Today, DateTime.Parse(Left(inYM, 4) + "/" + Right(inYM, 2)));
                    if (ret > 36)
                    {
                        // 未来過ぎて不正なので、入力なし扱い
                        return "";
                    }
                    else if (ret < 0)
                    {
                        // 過去なので、「無し」扱い
                        return none;
                    }
                    else
                    {
                        return inYM;
                    }
                }
            }

            // 年月形式不正の場合、「無し」となす
            return none;
        }
        public static decimal ConvertDecimal(object value)
        {
            if (value == null) return 0;
            if (value is DBNull) return 0;
            return Convert.ToDecimal(value);
        }
        public static double ConvertToDouble(object value)
        {
            if (value == null) return 0;
            if (value is DBNull) return 0;
            return Convert.ToDouble(value);
        }
        public static int ConvertToInt32(object value)
        {
            if (value == null) return 0;
            if (value is DBNull) return 0;
            return Convert.ToInt32(value);
        }
        public static long ConvertToInt64(object value)
        {
            if (value == null) return 0;
            if (value is DBNull) return 0;
            return Convert.ToInt64(value);
        }

        public static string setCheckCarYm(string inYM, bool inFlgNone = false)
        {
            var NONE = "無し";

            long ret;

            // 「無し」フラグ On
            if (inFlgNone)
                return NONE;

            // 入力なし
            if (inYM == "" | Information.IsNumeric(inYM) == false)
                return "";

            if (Strings.Len(inYM) == 6 && Strings.Mid(inYM, 5) == "00")
                // MM が "00" の場合、YYYY 部分のみ生かす
                inYM = Strings.Left(inYM, 4);

            if (Strings.Len(inYM) == 4)
            {
                if (Information.IsDate(inYM + "/01/01"))
                {
                    ret = DateAndTime.DateDiff(DateInterval.Year, System.DateTime.Today, DateTime.Parse(inYM + "/01/01"));
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
            else if (Strings.Len(inYM) == 6)
            {
                if (Information.IsDate(Strings.Left(inYM, 4) + "/" + Strings.Right(inYM, 2) + "/01"))
                {
                    ret = DateAndTime.DateDiff(DateInterval.Month, System.DateTime.Today, DateTime.Parse(Strings.Left(inYM, 4) + "/" + Strings.Right(inYM, 2) + "/01"));
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

        public static void chkImgFile(string imgPath, string strSesName, string strDefImg)
        {
            if (Strings.Trim(imgPath) == "" || File.Exists(imgPath) == false)
            {
                strSesName = strDefImg;
            }
            else
            {
                int maxIndx = imgPath.Split(@"\").GetUpperBound(0);   // 切り分けて格納した配列の最後尾を取得
                                                                      // 上で取得したファイル部分を置換してpathのみを取り出し、ファイルをgetする(最終確認の為)
                foreach (string nFileName in Directory.GetFiles(Strings.Replace(imgPath, (string?)imgPath.Split(@"\").GetValue(maxIndx), "")))
                    strSesName = imgPath;
            }
        }

        // **************************************************************************
        // * フォーマット処理
        // **************************************************************************
        //public static string setFormat(long param, string formatParm = "")
        //{
        //    formatParm = Convert.ToString(Strings.Format(param, "#,##0") + " 円");
        //    return formatParm;
        //}

        // **************************************************************************
        // * フォーマット処理
        // **************************************************************************
        public static string setFormatCurrency(object value, string unit = " 円")
        {
            var formatParm = "";
            if (Convert.ToInt32(value) == 0)
            {
                return formatParm;
            }
            else
            {
                formatParm = Convert.ToString(Strings.Format(value, "#,##0") + unit);
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
            if (!Information.IsNumeric(Strings.Trim(year)))
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
            int leDay = Strings.Len(Strings.Replace(strDay, "/", ""));
            string rtstrDay = "";
            switch (leDay)
            {
                case 8:
                    {
                        rtstrDay = Strings.Trim(Strings.Left(strDay, 4)) + "年" + Strings.Trim(DateFormatZero(Strings.Mid(strDay, 5, 2))) + "月" + Strings.Trim(DateFormatZero(Strings.Right(strDay, 2))) + "日";
                        break;
                    }
                case 6:
                    {
                        rtstrDay = Strings.Trim(Strings.Left(strDay, 4)) + "年" + Strings.Trim(DateFormatZero(Strings.Mid(strDay, 5, 2))) + "月";
                        break;
                    }
            }

            return rtstrDay;

        }
    }
}
