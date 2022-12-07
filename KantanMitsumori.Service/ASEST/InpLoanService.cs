using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;
using System.Text;

namespace KantanMitsumori.Service
{
    public class InpLoanService : IInpLoanService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public InpLoanService(ILogger<InpLoanService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public ResponseBase<ResponseInpLoan> CalInpLoan(RequestCalInpLoan model)
        {
            try
            {
                ResponseInpLoan response = new ResponseInpLoan();
                string mesg = checkCalcData(model);
                if (!string.IsNullOrEmpty(mesg))
                {
                    response.CalcInfo = mesg;
                    return ResponseHelper.Ok<ResponseInpLoan>(HelperMessage.I0003, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0003), response);

                };
                CommonSimLon simLon = new CommonSimLon(_logger);
                simLon.SaleSumPrice = model.SaleSumPrice;
                simLon.Deposit = model.Deposit;
                simLon.MoneyRate = model.MoneyRate;
                simLon.PayTimes = model.PayTimes;
                simLon.Bonus = model.Bonus;
                simLon.FirstMonth = model.FirstMonth;
                simLon.BonusFirst = model.BonusFirst;
                simLon.BonusSecond = model.BonusSecond;
                simLon.ConTax = model.ConTax;
                if (simLon.CalcRegLoan())
                {
                    response.MoneyRate = simLon.MoneyRate;
                    response.Deposit = simLon.Deposit;
                    response.Principal = simLon.Principal;
                    response.Fee = simLon.Fee;
                    response.PayTotal = simLon.PayTotal;
                    response.FirstPay = simLon.FirstPay;
                    response.FirstPayMonth = simLon.FirstPayMonth;
                    response.LastPayMonth = simLon.LastPayMonth;
                    response.PayMonth = simLon.PayMonth;
                    response.Bonus = simLon.Bonus;
                    response.BonusFirst = simLon.BonusFirst.ToString();
                    response.BonusSecond = simLon.BonusSecond.ToString();
                    response.BonusTimes = simLon.BonusTimes;
                    response.PayTimes = simLon.PayTimes;
                    return ResponseHelper.Ok<ResponseInpLoan>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), response);
                }
                else
                {
                    response.CalcInfo = simLon.CalcInfo;
                    return ResponseHelper.Ok<ResponseInpLoan>(HelperMessage.I0003, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0003), response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CalInpLoan");
                return ResponseHelper.Error<ResponseInpLoan>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public async Task<ResponseBase<string>> UpdateInputLoan(RequestUpdateInpLoan model)
        {
            try
            {
                var remesg = "";
                string mesg = checkUpdateData(model);
                if (!string.IsNullOrEmpty(mesg))
                {
                    remesg = mesg;
                    return ResponseHelper.Ok<string>(HelperMessage.I0003, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0003), remesg);

                };
                TEstimate dtEstimates = _unitOfWork.Estimates.GetSingle(n => n.EstNo == model.EstNo && n.EstSubNo == model.EstSubNo && n.Dflag == false);
                TEstimateSub dtEstimateSubs = _unitOfWork.EstimateSubs.GetSingle(n => n.EstNo == model.EstNo && n.EstSubNo == model.EstSubNo && n.Dflag == false);
                if (dtEstimates == null || dtEstimateSubs == null)
                {
                    return ResponseHelper.Error<string>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S), remesg);
                }
                dtEstimates.Rate = model.MoneyRateCl;
                dtEstimates.Deposit = model.DepositCl;
                dtEstimates.Principal = model.Principal;
                dtEstimates.PartitionFee = model.Fee;
                dtEstimates.PartitionAmount = model.PayTotal;
                dtEstimates.FirstPayMonth = model.FirstPayMonth;
                dtEstimates.LastPayMonth = model.LastPayMonth;
                dtEstimates.FirstPayAmount = model.FirstPay;
                dtEstimates.PayAmount = model.PayMonth;
                dtEstimates.BonusAmount = model.BonusCl;
                dtEstimates.BonusFirst = model.BonusFirstMonth;
                dtEstimates.BonusSecond = model.BonusSecondMonth;
                dtEstimates.BonusTimes = model.BonusTimes;
                dtEstimates.PayTimes = model.PayTimesCl;
                dtEstimateSubs.LoanModifyFlag = model.LoanModifyFlag == 1 ? true : false;
                dtEstimateSubs.LoanRecalcSettingFlag = model.chkProhibitAutoCalc == 1 ? false : true;
                dtEstimateSubs.LoanInfo = 0;
                _unitOfWork.Estimates.Update(dtEstimates);
                _unitOfWork.EstimateSubs.Update(dtEstimateSubs);
                await _unitOfWork.CommitAsync();
                return ResponseHelper.Ok<string>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), remesg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInputLoan");
                return ResponseHelper.Error<string>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
            }
        }

        #region fuc
        private string checkCalcData(RequestCalInpLoan model)
        {
            string errMsg = "";
            errMsg = chkNumber(model.Deposit.ToString(), "頭金");
            errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.MoneyRate.ToString(), "金利（実質年率）", true);

            errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.PayTimes.ToString(), "支払回数");

            errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.FirstMonth.ToString(), "第1回支払月");

            if (model.rdBonus)
            {

                errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.Bonus.ToString(), "ボーナス加算額");

                if (chkNumber(model.BonusFirst.ToString(), "ボーナス第1回支払月") != "")
                    errMsg += CommonFunction.AppendBr(errMsg) + "ボーナス第1回支払月が正しく選択されていません。";
                else if (model.BonusFirst < 1 || model.BonusFirst > 12)
                    errMsg += CommonFunction.AppendBr(errMsg) + "ボーナス第1回支払月が正しく選択されていません。";

                if (chkNumber(model.BonusSecond.ToString(), "ボーナス第2回支払月") != "")
                    errMsg += CommonFunction.AppendBr(errMsg) + "ボーナス第2回支払月が正しく選択されていません。";
                else
                    if (model.BonusSecond < 1 || model.BonusSecond > 12)
                    errMsg += CommonFunction.AppendBr(errMsg) + "ボーナス第2回支払月が正しく選択されていません。";
            }
            if (errMsg == "<br />")
                return "";
            else
            {
                return errMsg;
            }
        }
        private string checkUpdateData(RequestUpdateInpLoan model)
        {
            string errMsg = "";

            if (model.DepositCl != 0)
            {
                errMsg = chkNumber(model.DepositCl.ToString(), "頭金");
            }
            if (model.MoneyRateCl != 0)
                errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.MoneyRateCl.ToString(), "金利（実質年率）", true);

            if (model.Fee != 0)
                errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.Fee.ToString(), "分割手数料");

            if (model.PayTotal != 0)
                errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.PayTotal.ToString(), "分割支払金合計");

            if (model.PayTimesCl != 0)
                errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.PayTimesCl.ToString(), "支払回数");

            if (!string.IsNullOrEmpty(model.FirstPayMonth))
            {
                if (chkNumber(model.FirstPayMonth!, "支払期間（開始年月）") != "")
                    errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.FirstPayMonth!, "支払期間（開始年月）");
                else if (model.FirstPayMonth.Length != 6)
                    errMsg += CommonFunction.AppendBr(errMsg) + "支払期間（開始年月）の日付形式が不正です。";
                else if (CommonFunction.IsDate(CommonFunction.Left(model.FirstPayMonth!, 4) + "/" + CommonFunction.Right(model.FirstPayMonth!, 2) + "/01") == false)
                    errMsg += CommonFunction.AppendBr(errMsg) + "支払期間（開始年月）の日付形式が不正です。";
            }
            if (!string.IsNullOrEmpty(model.LastPayMonth))
            {
                if (chkNumber(model.LastPayMonth, "支払期間（終了年月）") != "")
                    errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.LastPayMonth, "支払期間（終了年月）");
                else if (model.LastPayMonth.Length != 6)
                    errMsg += CommonFunction.AppendBr(errMsg) + "支払期間（終了年月）の日付形式が不正です。";
                else if (CommonFunction.IsDate(CommonFunction.Left(model.LastPayMonth, 4) + "/" + CommonFunction.Right(model.LastPayMonth, 2) + "/01") == false)
                    errMsg += CommonFunction.AppendBr(errMsg) + "支払期間（終了年月）の日付形式が不正です。";
            }
            if (model.FirstPay != 0)
                errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.FirstPay.ToString(), "初回支払額");

            if (model.PayMonth != 0)
                errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.PayMonth.ToString(), "2回目以降支払額");

            if (model.rdBonus_Result!.Contains("rbBonusU_Result"))
            {
                if (model.BonusCl != 0)
                    errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.BonusCl.ToString(), "ボーナス加算額");
                else
                    errMsg += CommonFunction.AppendBr(errMsg) + "ボーナス加算額が未入力です。";

                if (!string.IsNullOrEmpty(model.BonusFirstMonth))
                {
                    if (chkNumber(model.BonusFirstMonth!, "ボーナス第1回支払月") != "")
                        errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.BonusFirstMonth, "ボーナス第1回支払月");
                    else if (Convert.ToInt32(model.BonusFirstMonth) < 1 | Convert.ToInt32(model.BonusFirstMonth) > 12)
                        errMsg += CommonFunction.AppendBr(errMsg) + "ボーナス第1回支払月の値が不正です。";
                }
                if (!string.IsNullOrEmpty(model.BonusSecondMonth))
                {
                    if (chkNumber(model.BonusSecondMonth, "ボーナス第2回支払月") != "")
                        errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.BonusSecondMonth, "ボーナス第2回支払月");
                    else if (Convert.ToInt32(model.BonusSecondMonth) < 1 | Convert.ToInt32(model.BonusSecondMonth) > 12)
                        errMsg += CommonFunction.AppendBr(errMsg) + "ボーナス第2回支払月の値が不正です。";
                }
                if (model.BonusTimes != 0)
                    errMsg += CommonFunction.AppendBr(errMsg) + chkNumber(model.BonusTimes.ToString(), "ボーナス加算回数");
            }
            if (CommonFunction.Right(errMsg, 6) == "<br />")
                errMsg = errMsg.Substring(0, errMsg.Length - 6);
            if (errMsg == "")
                return "";
            else
            {
                return errMsg;
            }
        }

        private string chkNumber(string strNumber, string itemName, bool isDec = false)
        {
            var ByteLength = Encoding.GetEncoding("Shift_JIS").GetByteCount(strNumber);
            int n;
            decimal m;
            bool isNumeric = isDec ? decimal.TryParse(strNumber, out m) : int.TryParse(strNumber, out n);

            if ((strNumber.Length) != ByteLength)
                return itemName + "に半角数字以外は入力できません。";
            else if (isNumeric == false)
                return itemName + "に半角数字以外は入力できません。";
            else if (Convert.ToDecimal(strNumber) < 0)
                return itemName + "にマイナス値は入力できません。";
            else if (isDec)
            {
                try
                {
                    double num = Convert.ToDouble(strNumber);
                }
                catch (Exception)
                {
                    return itemName + "の形式が不正です。";
                }
            }
            else if (strNumber.Contains("."))
                return itemName + "に小数は入力できません。";

            return "";
        }

        #endregion
    }

}