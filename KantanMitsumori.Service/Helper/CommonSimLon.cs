using KantanMitsumori.Helper.Constant;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Service.Helper
{
    public class CommonSimLon
    {
        private readonly ILogger _logger;

        #region Model
        // 販売価格計
        public int SaleSumPrice { get; set; }
        // 車両本体価格
        public int CarPrice { get; set; }
        // 付属品
        public int OptionPrice { get; set; }
        // 値引き
        public int Discount { get; set; }
        // 税金保険料
        public int Tax { get; set; }
        // 課税対象諸費用
        public int TaxCost { get; set; }
        // 非課税預り法定費用
        public int TaxFree { get; set; }
        // 下取車充当額
        public int TradeIn { get; set; }
        // 残債
        public int Balance { get; set; }
        // 頭金
        public int Deposit { get; set; }

        // 金利
        public decimal MoneyRate { get; set; }
        // 支払回数
        public int PayTimes { get; set; }
        // 第1回支払月
        public int FirstMonth { get; set; }
        // ボーナス月加算額
        public int Bonus { get; set; }
        // 第1回ボーナス支払月
        public int BonusFirst { get; set; }
        // 第2回ボーナス支払月
        public int BonusSecond { get; set; }
        // 消費税率
        public decimal ConTax { get; set; }
        // 現金価格計
        public int CashTotal { get; set; }
        // 諸費用合計
        public int TotalCost { get; set; }
        // 消費税合計
        public int TotalTax { get; set; }
        // 現金価格合計
        public int Total { get; set; }
        // 頭金計
        public int DepositTotal { get; set; }
        // 残金
        public int Principal { get; set; }
        // 表示用アドオン率
        public decimal AddonDisp { get; set; }
        // 分割払分手数料
        public int Fee { get; set; }
        // 支払合計額
        public int PayTotal { get; set; }
        // ボーナス回数
        public int BonusTimes { get; set; }
        // ボーナス支払額合計
        public int BonusTotal { get; set; }
        // 分割支払総額
        public int PartitionPayTotal { get; set; }
        // 第1回目分割支払金
        public int FirstPay { get; set; }
        // 第2回目以降分割支払金
        public int PayMonth { get; set; }
        // 内分割払分vFirstPayMonth
        public int UtiPrincipal { get; set; }
        // 初回支払年月
        public int FirstPayMonth { get; set; }
        // 最終回支払年月
        public int LastPayMonth { get; set; }
        // 手数料合計
        public int FeeTotal { get; set; }
        // 月々支払希望額
        public decimal HPayMonth { get; set; }
        // ボーナス月加算希望額
        public decimal HBonus { get; set; }

        // 計算後メッセージ
        public string CalcInfo { get; set; }

        #endregion model

        public CommonSimLon(ILogger logger)
        {
            _logger = logger;
        }
        public bool CalcRegLoan()
        {
            try
            {
                Principal = SaleSumPrice - Deposit;
                if (MoneyRate > 0)
                {
                    decimal vCalcRate = MoneyRate / (decimal)100;
                    decimal vAddon = ToHalfAjust(PayTimes * (vCalcRate / 12) / (1 - (decimal)Math.Pow((double)(1 + (vCalcRate / 12)), (PayTimes * -1))) - 1, 4);

                    AddonDisp = ToHalfAjust(vAddon * 100, 2);
                    Fee = (int)ToRoundDown(Principal, vAddon, 0);
                }
                PayTotal = Principal + Fee;
                if (Bonus > 0)
                    BonusTimes = CountBonusTimes(FirstMonth, PayTimes, BonusFirst, BonusSecond);
                else
                    BonusTimes = 0;
                BonusTotal = Bonus * BonusTimes;
                PartitionPayTotal = PayTotal - BonusTotal;
                PayMonth = (int)ToRoundDown(PartitionPayTotal / (decimal)PayTimes, -2);
                FirstPay = PartitionPayTotal - PayMonth * (PayTimes - 1);
                if (FirstPay < 3000 | PayMonth < 3000)
                {
                    CalcInfo = CommonConst.msgPayMonthShort;
                    return false;
                }
                if (PayTotal * 0.7 < BonusTotal)
                {
                    CalcInfo = CommonConst.msgBonusSevenOver;
                    return false;
                }
                if (BonusTimes == 0 & Bonus > 0)
                {
                    CalcInfo = CommonConst.msgBonusMonthErr;
                    return false;
                }
                if (PayTotal > 99999999)
                {
                    CalcInfo = CommonConst.msgPayTotalOver;
                    return false;
                }
                if (BonusTimes == 1 & BonusSecond != 0)
                    CalcInfo = CommonConst.msgBonusTimesErr;

                DateTime wFirstDt;
                string wNowMonth = DateTime.Now.ToString("MM");
                if (int.Parse(wNowMonth) >= 10 & FirstMonth <= 3)
                {
                    wNowMonth = DateTime.Now.AddYears(1).Year + "/" + FirstMonth + "/01";
                }
                else
                {
                    wNowMonth = DateTime.Now.Year + "/" + FirstMonth + "/01";
                }
                wFirstDt = DateTime.Parse(wNowMonth);
                FirstPayMonth = Convert.ToInt32(wFirstDt.ToString("yyyyMM"));
                LastPayMonth = Convert.ToInt32(wFirstDt.AddMonths(PayTimes - 1).ToString("yyyyMM"));
            }
            catch (Exception ex)
            {
                CalcInfo = CommonConst.msgCalcException;
                _logger.LogError(ex, "CalcRegLoan", "CSIM-010C");
                return false;
            }

            return true;
        }

        // **************************************************************************
        // * 小数点以下指定桁未満を四捨五入
        // **************************************************************************
        public static decimal ToHalfAjust(decimal dValue, int iDigits)
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
        public static int CountBonusTimes(int inFirstMonth, int inPayTimes, int inBonusFirst, int inBonusSecond)
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
        public static decimal ToRoundDown(decimal dValue, int iDigits)
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
        public static decimal ToRoundDown(long inint, decimal inSgl, int iDigits)
        {
            decimal dCoef = (decimal)Math.Pow(10, iDigits);
            long intR = (int)(inSgl * 10000);
            decimal dValue = (inint * intR * 1.0m) / 10000.0m;
            if (dValue > 0)
                return Math.Floor(dValue * dCoef) / dCoef;
            else
                return Math.Ceiling(dValue * dCoef) / dCoef;
        }
    }
}
