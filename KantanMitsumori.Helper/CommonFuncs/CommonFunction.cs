using KantanMitsumori.Helper.Constant;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace KantanMitsumori.Helper.CommonFuncs
{
    public static class CommonFunction
    {
        private static readonly ILogger _logger;

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
            formatText = Convert.ToString(Strings.Format(text2, "#,##") + unit);
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
        public static bool DecUserNo(string vEncNo, string vDecNo)
        {
            string wOne = "";
            string wStr = "";
            string wInt = "";
            int i = 0;

            try
            {
                // 文字数分ループ
                for (i = 1; i < vEncNo.Length; i++)
                {
                    wOne = Mid(vEncNo, i, 1);
                    // 取り出した文字が英小文字(a～z)の場合
                    if (Strings.Asc(wOne) >= 97 && Strings.Asc(wOne) <= 122)
                    {
                        // 英小文字と数字がすでに格納されていれば1文字分デコード
                        if (wStr.Length == 1 && wInt.Length > 0)
                        {
                            vDecNo += Strings.Chr(Strings.Asc(wStr) - Convert.ToInt32(wInt));
                            // ワーククリア
                            wStr = "";
                            wInt = "";
                        }

                        //英小文字を格納
                        wStr = wOne;
                    }
                    else
                    {
                        // 数字を格納（'－'マイナスもありえる）
                        wInt += wOne;
                    }
                }

                // 最後の文字分のデコード
                vDecNo += Strings.Chr(Strings.Asc(wStr) - Convert.ToInt32(wInt));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CommonFuncs - DecUserNo - GCMF-010C ◆会員認証エラー◆ 復号化前会員番号：{0}", vEncNo);
                return false;
            }
        }

        /// <summary>
        /// 見積書データ項目 税抜／税込切替時の再計算
        /// </summary>
        /// <param name="oldVal"></param>
        /// <param name="conTaxInputKb"></param>
        /// <param name="vTax"></param>
        /// <returns></returns>
        public static long reCalcItem(long oldVal, string conTaxInputKb, double vTax)
        {
            if (oldVal == 0)
            {
                return 0;
            }

            decimal wkVal;      // 浮動小数点の計算誤差回避のため
            if (conTaxInputKb == "False")
            {
                // 税込 → 税抜
                wkVal = oldVal / (1 + Convert.ToDecimal(vTax));
                return Convert.ToInt64(Math.Ceiling(wkVal));
            }
            else
            {
                // 抜 → 税込
                wkVal = oldVal * (1 + Convert.ToDecimal(vTax));
                return Convert.ToInt64(Math.Floor(wkVal)); ;
            }
        }

        /// <summary>
        /// 日付の"/"を外す
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string DateFormatReplace(string date)
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
        public static bool IsDate(this string input)
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
        /// 車検有効期限 判定・編集
        /// </summary>
        /// <param name="inYM"></param>
        /// <param name="inFlagNone"></param>
        /// <returns></returns>
        public static string setCheckCarYm(string inYM, bool inFlgNone = false)
        {
            string none = "無し";

            long ret;

            // 「無し」フラグ On
            if (inFlgNone)
            {
                return none;
            }

            // 入力なし
            if (inYM == "" || IsNumeric(inYM))
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
    }
}
