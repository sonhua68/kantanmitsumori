﻿using KantanMitsumori.Helper.Constant;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace KantanMitsumori.Service.Helper
{
    public class CommonSimLon
    {
        private readonly ILogger _logger;

        #region Model
        // 販売価格計
        public long SaleSumPrice { get; set; }
        // 車両本体価格
        public long CarPrice { get; set; }
        // 付属品
        public long OptionPrice { get; set; }
        // 値引き
        public long Discount { get; set; }
        // 税金保険料
        public long Tax { get; set; }
        // 課税対象諸費用
        public long TaxCost { get; set; }
        // 非課税預り法定費用
        public long TaxFree { get; set; }
        // 下取車充当額
        public long TradeIn { get; set; }
        // 残債
        public long Balance { get; set; }
        // 頭金
        public long Deposit { get; set; }
        // 金利
        public decimal MoneyRate { get; set; }
        // 支払回数
        public int PayTimes { get; set; }
        // 第1回支払月
        public int FirstMonth { get; set; }
        // ボーナス月加算額
        public long Bonus { get; set; }
        // 第1回ボーナス支払月
        public int BonusFirst { get; set; }
        // 第2回ボーナス支払月
        public int BonusSecond { get; set; }
        // 消費税率
        public decimal ConTax { get; set; }
        // 現金価格計
        public long CashTotal { get; set; }
        // 諸費用合計
        public long TotalCost { get; set; }
        // 消費税合計
        public long TotalTax { get; set; }
        // 現金価格合計
        public long Total { get; set; }
        // 頭金計
        public long DepositTotal { get; set; }
        // 残金
        public long Principal { get; set; }
        // 表示用アドオン率
        public decimal AddonDisp { get; set; }
        // 分割払分手数料
        public long Fee { get; set; }
        // 支払合計額
        public long PayTotal { get; set; }
        // ボーナス回数
        public long BonusTimes { get; set; }
        // ボーナス支払額合計
        public long BonusTotal { get; set; }
        // 分割支払総額
        public long PartitionPayTotal { get; set; }
        // 第1回目分割支払金
        public long FirstPay { get; set; }
        // 第2回目以降分割支払金
        public long PayMonth { get; set; }
        // 内分割払分vFirstPayMonth
        public long UtiPrincipal { get; set; }
        // 初回支払年月
        public long FirstPayMonth { get; set; }
        // 最終回支払年月
        public long LastPayMonth { get; set; }
        // 手数料合計
        public long FeeTotal { get; set; }
        // 月々支払希望額
        public decimal hPayMonth { get; set; }
        // ボーナス月加算希望額
        public decimal hBonus { get; set; }

        // 計算後メッセージ
        public string CalcInfo { get; set; }
        #endregion model

        public CommonSimLon(ILogger logger)
        {
            _logger = logger;
        }

        // 通常ローン計算
        public bool calcRegLoan()
        {
            try
            {
                // 共通部（残金まで）計算
                // 残金 = 販売価格計 - 頭金
                Principal = SaleSumPrice - Deposit;

                // 分割払分手数料
                // --金利０の場合
                if (MoneyRate > 0)
                {
                    // --アドオン率計算
                    decimal vCalcRate = MoneyRate / (decimal)100;
                    // --計算用(小数点5桁以下四捨五入)
                    decimal vAddon = ToHalfAjust(PayTimes * (vCalcRate / 12) / (1 - (decimal)Math.Pow((double)(1 + (vCalcRate / 12)), (PayTimes * -1))) - 1, 4);
                    // --表示用
                    AddonDisp = ToHalfAjust(vAddon * 100, 2);
                    // --分割払分手数料(小数点以下切捨て)
                    Fee = (long)ToRoundDown(Principal, vAddon, 0);
                }

                // 支払合計額        
                PayTotal = Principal + Fee;
                // ボーナス回数
                if (Bonus > 0)
                    BonusTimes = countBonusTimes(FirstMonth, PayTimes, BonusFirst, BonusSecond);
                else
                    BonusTimes = 0;
                // ボーナス支払額合計
                BonusTotal = Bonus * BonusTimes;
                // 分割支払総額
                PartitionPayTotal = PayTotal - BonusTotal;
                // 第2回目以降分割支払金(100円未満切捨て)
                PayMonth = (long)ToRoundDown(PartitionPayTotal / (decimal)PayTimes, -2);
                // 第1回目分割支払金
                FirstPay = PartitionPayTotal - PayMonth * (PayTimes - 1);

                // 計算結果のチェック

                // --月々支払額が3000円未満の場合
                if (FirstPay < 3000 | PayMonth < 3000)
                {
                    CalcInfo = CommonConst.msgPayMonthShort;
                    return false;
                }
                // --ボーナス支払額合計が分割支払金合計の70%超の場合
                // If vPrincipal / 2 < vBonusTotal Then
                if (PayTotal * 0.7 < BonusTotal)
                {
                    CalcInfo = CommonConst.msgBonusSevenOver;
                    return false;
                }
                // --ボーナス月がローン期間外
                if (BonusTimes == 0 & Bonus > 0)
                {
                    CalcInfo = CommonConst.msgBonusMonthErr;
                    return false;
                }
                // 分割支払金合計上限チェック
                if (chgNullToZeroLng(Convert.ToString(PayTotal)) > 99999999)
                {
                    CalcInfo = CommonConst.msgPayTotalOver;
                    return false;
                }

                // --ボーナス回数が一回の時（ガイダンスのみ）
                if (BonusTimes == 1 & BonusSecond != 0)
                    CalcInfo = CommonConst.msgBonusTimesErr;

                // 初回支払年月
                DateTime wFirstDt;
                string wNowMonth = DateTime.Now.ToString("MM");
                // 操作日が10月以降で翌年の1月～3月が指定された場合
                if (int.Parse(wNowMonth) >= 10 & FirstMonth <= 3)
                {
                    wNowMonth = DateTime.Now.AddYears(1).Year + "/" + FirstMonth + "/01";
                    wFirstDt = DateTime.Parse(wNowMonth);
                    FirstPayMonth = Convert.ToInt32(Strings.Format(wFirstDt, "yyyyMM"));
                }
                else
                {
                    wNowMonth = DateTime.Now.Year + "/" + FirstMonth + "/01";
                    wFirstDt = DateTime.Parse(wNowMonth);
                    FirstPayMonth = Convert.ToInt32(Strings.Format(wFirstDt, "yyyyMM"));
                }

                // 最終回支払年月
                LastPayMonth = Convert.ToInt32(Strings.Format(wFirstDt.AddMonths(PayTimes - 1), "yyyyMM"));
            }
            catch (Exception ex)
            {
                // エラーログ書出し
                CalcInfo = CommonConst.msgCalcException;
                _logger.LogInformation(ex, "CalcRegLoan", "CSIM-010C");
                return false;
            }

            return true;
        }




        // **************************************************************************
        // * 小数点以下指定桁未満を四捨五入
        // **************************************************************************
        public decimal ToHalfAjust(decimal dValue, int iDigits)
        {
            decimal dCoef = (decimal)Math.Pow(10, iDigits);

            if (dValue > 0)
                return Math.Floor((dValue * dCoef) + (decimal)0.5) / dCoef;
            else
                return Math.Ceiling((dValue * dCoef) - (decimal)0.5) / dCoef;
        }

        // **************************************************************************
        // * ボーナス回数を求める
        // **************************************************************************
        public int countBonusTimes(int inFirstMonth, int inPayTimes, int inBonusFirst, int inBonusSecond)
        {
            int i;
            int loopMonth = inFirstMonth;
            int vBonusTimes = 0;  // ボーナス回数
            for (i = 0; i <= inPayTimes - 1; i++)
            {
                // 現回数が偶数の場合、第1回ボーナス月と比較
                if (vBonusTimes % 2 == 0)
                {
                    if (loopMonth == inBonusFirst)
                        vBonusTimes += 1;
                }
                else if (loopMonth == inBonusSecond)
                    vBonusTimes += 1;
                // 月を+1(12月だったら次月は1月)
                if (loopMonth == 12)
                    loopMonth = 1;
                else
                    loopMonth += 1;
            }
            return vBonusTimes;
        }
        // **************************************************************************
        // * 指定桁未満を切り捨て
        // **************************************************************************
        public decimal ToRoundDown(decimal dValue, int iDigits)
        {
            decimal dCoef = (decimal)Math.Pow(10, iDigits);

            if (dValue > 0)
                return Math.Floor(dValue * dCoef) / dCoef;
            else
                return Math.Ceiling(dValue * dCoef) / dCoef;
        }
        // **************************************************************************
        // * 指定桁未満を切り捨て2
        // **************************************************************************
        public decimal ToRoundDown(long inLong, decimal inSgl, int iDigits)
        {
            decimal dCoef = (decimal)Math.Pow(10, iDigits);
            int intR = (int)(inSgl * 10000);
            decimal dValue = inLong * intR / (decimal)10000;
            if (dValue > 0)
                return Math.Floor(dValue * dCoef) / dCoef;
            else
                return Math.Ceiling(dValue * dCoef) / dCoef;
        }
        // **************************************************************************
        // * 金額項目　空白からゼロへ変換
        // **************************************************************************
        public long chgNullToZeroLng(string val)
        {

            // 前後の空白除去
            val = Strings.Trim(val);

            if (val == "")
                return 0;
            else
                return Convert.ToInt64(val);
        }
    }
}
