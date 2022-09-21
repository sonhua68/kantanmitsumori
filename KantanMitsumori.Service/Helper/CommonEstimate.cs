﻿using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Response;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Reflection;

namespace KantanMitsumori.Service.Helper
{
    public class CommonEstimate
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfWorkIDE _unitOfWorkIDE;
        private readonly IMapper _mapper;
        private readonly HelperMapper _helperMapper;

        private LogToken valToken;
        private CommonFuncHelper _commonFuncHelper;
        private List<string> reCalEstModel;
        private List<string> reCalEstSubModel;

        public CommonEstimate(ILogger<CommonEstimate> logger, IUnitOfWork unitOfWork, IUnitOfWorkIDE unitOfWorkIDE, IMapper mapper, HelperMapper helperMapper, CommonFuncHelper commonFuncHelper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _unitOfWorkIDE = unitOfWorkIDE;
            _mapper = mapper;
            _commonFuncHelper = commonFuncHelper;
            _helperMapper = helperMapper;

            valToken = new LogToken();
            reCalEstModel = new List<string>();
            reCalEstSubModel = new List<string>();
        }

        /// <summary>
        /// * 見積書データ 小計・合計計算（税抜／税込切替時の調整、および小計・合計計算）
        /// </summary>
        /// <returns></returns>
        public async Task<bool> calcSum(string inEstNo, string inEstSubNo, LogToken logToken)
        {
            try
            {
                var estModel = _unitOfWork.Estimates.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false);
                var estSubModel = _unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == estModel.EstNo && x.EstSubNo == estModel.EstSubNo && x.Dflag == false);
                if (estModel == null || estSubModel == null)
                {
                    return false;
                }
                // 再計算前の総額
                int? oldSalesSum = estModel.SalesSum;

                // 消費税率取得
                var vTax = _commonFuncHelper.getTax((DateTime)estModel.Udate!, logToken.sesTaxRatio, logToken.UserNo);
                valToken.sesTaxRatio = vTax;

                // 会員諸費用設定取得
                var getUserDef = _commonFuncHelper.getUserDefData(logToken.UserNo);

                if (getUserDef != null)
                {
                    if (estModel.ConTaxInputKb != getUserDef.ConTaxInputKb)
                    {
                        // 消費税区分（税込／税抜）がデータと設定値で不一致の場合、データの各項目を再設定
                        estModel.ConTaxInputKb = getUserDef.ConTaxInputKb;

                        Type typeEst = estModel.GetType();
                        Type typeEstSub = estSubModel.GetType();

                        IList<PropertyInfo> propsEst = new List<PropertyInfo>(typeEst.GetProperties().Where(x => x.PropertyType.Name == "Int32"));
                        IList<PropertyInfo> propsEstSub = new List<PropertyInfo>(typeEstSub.GetProperties().Where(x => x.PropertyType.Name == "Int32"));

                        // cal [Estmate]
                        foreach (PropertyInfo propEst in propsEst)
                        {
                            string properties = propEst.Name;
                            // Do something with propValue
                            reCalEstModel.Add(propEst.Name);


                            int objValue = (int)propEst.GetValue(estModel)!;

                            if (reCalEstModel.Contains(propEst.Name))
                            {
                                objValue = CommonFunction.reCalcItem(objValue, (bool)estModel.ConTaxInputKb, vTax);
                            }

                            propEst.SetValue(estModel, objValue);
                        }

                        // cal [EstmateSub]
                        foreach (PropertyInfo propEstSub in propsEstSub)
                        {
                            string properties = propEstSub.Name;
                            // Do something with propValue
                            reCalEstSubModel.Add(propEstSub.Name);

                            int objValue = (int)propEstSub.GetValue(estModel)!;

                            if (reCalEstSubModel.Contains(propEstSub.Name))
                            {
                                objValue = CommonFunction.reCalcItem(objValue, (bool)estModel.ConTaxInputKb, vTax);
                            }

                            propEstSub.SetValue(estSubModel, objValue);
                        }
                    }
                }

                // その他費用
                estSubModel.Sonota = estSubModel.RakuSatu
                                   + estSubModel.Rikusou;

                // 付属品・特別仕様
                estModel.OptionPriceAll = estModel.OptionPrice1
                                        + estModel.OptionPrice2
                                        + estModel.OptionPrice3
                                        + estModel.OptionPrice4
                                        + estModel.OptionPrice5
                                        + estModel.OptionPrice6
                                        + estModel.OptionPrice7
                                        + estModel.OptionPrice8
                                        + estModel.OptionPrice9
                                        + estModel.OptionPrice10
                                        + estModel.OptionPrice11
                                        + estModel.OptionPrice12;

                // 車両販売価格
                estModel.CarSum = estModel.CarPrice
                                - estModel.Discount
                                + estSubModel.Sonota
                                + estModel.SyakenNew
                                + estModel.SyakenZok
                                + estModel.OptionPriceAll;

                // 税金・保険料
                estModel.TaxInsAll = estModel.AutoTax
                            + estModel.AcqTax
                            + estModel.WeightTax
                            + estModel.DamageIns
                            + estModel.OptionIns;

                // 税金・保険料相当額
                estSubModel.TaxInsEquivalentAll = estSubModel.AutoTaxEquivalent
                                                + estSubModel.DamageInsEquivalent;

                // 預り法定費用
                estModel.TaxFreeAll = estModel.TaxFreeCheck
                                    + estModel.TaxFreeGarage
                                    + estModel.TaxFreeTradeIn
                                    + estModel.TaxFreeRecycle
                                    + estModel.TaxFreeOther
                                    + estSubModel.TaxFreeSet1
                                    + estSubModel.TaxFreeSet2;

                // 手続代行費用
                estModel.TaxCostAll = estModel.TaxCheck
                                    + estModel.TaxGarage
                                    + estModel.TaxTradeIn
                                    + estModel.TaxRecycle
                                    + estModel.TaxDelivery
                                    + estModel.TaxOther
                                    + estSubModel.TaxTradeInSatei
                                    + estSubModel.TaxSet1
                                    + estSubModel.TaxSet2
                                    + estSubModel.TaxSet3;

                // 消費税合計
                decimal? wkContax; // 浮動小数点の計算誤差回避のため
                if (estModel.ConTaxInputKb == false)
                    wkContax = (decimal?)((estModel.CarSum + estSubModel.TaxInsEquivalentAll + estModel.TaxCostAll) * vTax);
                else
                {
                    wkContax = (decimal?)((estModel.CarSum + estSubModel.TaxInsEquivalentAll + estModel.TaxCostAll) / (1 + vTax));
                    wkContax = Math.Ceiling((decimal)wkContax!);
                    wkContax = wkContax * vTax;
                }
                estModel.ConTax = Convert.ToInt32(Math.Floor((decimal)wkContax!));
                // 現金販売価格
                estModel.CarSaleSum = estModel.CarSum
                                    + estModel.TaxInsAll
                                    + estSubModel.TaxInsEquivalentAll
                                    + estModel.TaxFreeAll
                                    + estModel.TaxCostAll;

                // お支払総額
                if (estModel.ConTaxInputKb == false)
                    estModel.SalesSum = estModel.CarSaleSum
                                        + estModel.ConTax
                                        - estModel.TradeInPrice
                                        + estModel.Balance;
                else
                    estModel.SalesSum = estModel.CarSaleSum
                           - estModel.TradeInPrice
                           + estModel.Balance;

                string strClearMsg = "";

                if ((oldSalesSum > 0) && (estModel.SalesSum != oldSalesSum) && (estModel.PayTimes > 0))
                {
                    // 総額変更ありの場合
                    if (Convert.ToBoolean(estSubModel.LoanRecalcSettingFlag))
                    {
                        // ローンの自動再計算
                        CommonSimLon simLon = new CommonSimLon(_logger);

                        simLon.SaleSumPrice = Convert.ToInt32(estModel.SalesSum);
                        simLon.Deposit = Convert.ToInt32(estModel.Deposit);
                        simLon.MoneyRate = Convert.ToInt32(estModel.Rate);
                        simLon.PayTimes = Convert.ToInt32(estModel.PayTimes);
                        simLon.FirstMonth = Convert.ToInt32(Strings.Right(estModel.FirstPayMonth, 2));
                        if (estModel.BonusAmount > 0)
                        {
                            simLon.Bonus = Convert.ToInt32(estModel.BonusAmount);
                            simLon.BonusFirst = Convert.ToInt32(estModel.BonusFirst);
                            simLon.BonusSecond = Convert.ToInt32(estModel.BonusSecond);
                        }
                        simLon.ConTax = vTax;
                        // 計算実行
                        if (simLon.calcRegLoan() == false)
                        {
                            strClearMsg = CommonConst.def_LoanInfo_Error.ToString();
                        }
                        else
                        {
                            estModel.Rate = (double)simLon.MoneyRate;
                            estModel.Deposit = simLon.Deposit;
                            estModel.Principal = simLon.Principal;
                            estModel.PartitionFee = simLon.Fee;
                            estModel.PartitionAmount = simLon.PayTotal;
                            estModel.FirstPayMonth = simLon.FirstPayMonth.ToString();
                            estModel.LastPayMonth = simLon.LastPayMonth.ToString();
                            estModel.FirstPayAmount = simLon.FirstPay;
                            estModel.PayAmount = simLon.PayMonth;
                            estModel.BonusAmount = simLon.Bonus;
                            estModel.BonusFirst = simLon.BonusFirst.ToString();
                            estModel.BonusSecond = simLon.BonusSecond.ToString();
                            estModel.BonusTimes = simLon.BonusTimes;
                            estModel.PayTimes = simLon.PayTimes;
                            estSubModel.LoanModifyFlag = false;
                            estSubModel.LoanRecalcSettingFlag = true;
                            estSubModel.LoanInfo = CommonConst.def_LoanInfo_NormalEnd;
                        }
                    }
                    else
                    {
                        strClearMsg = CommonConst.def_LoanInfo_Clear.ToString();
                    }
                }
                else
                {
                    // 総額変更なしの場合、ローン計算情報表示区分のクリア
                    estSubModel.LoanInfo = CommonConst.def_LoanInfo_Unexecuted;
                }
                if (strClearMsg != "")
                {
                    estModel.Principal = estModel.SalesSum;
                    // EstimateSubs
                    estSubModel.LoanModifyFlag = false;
                    estSubModel.LoanRecalcSettingFlag = true;
                    estSubModel.LoanInfo = Convert.ToByte(strClearMsg);
                }
                _unitOfWork.Estimates.Update(estModel);
                _unitOfWork.EstimateSubs.Update(estSubModel);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "calcSum " + "GCMF-060D");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 見積書データ取得
        /// </summary>
        /// <param name="inEstNo"></param>
        /// <param name="inEstSubNo"></param>
        /// <returns></returns>
        public EstModel getEstData(string inEstNo, string inEstSubNo)
        {
            var responseEst = new EstModel();
            try
            {
                var estModel = _unitOfWork.Estimates.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false);
                var estSubModel = _unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false);
                valToken.sesCarImgPath = CommonFunction.chkImgFile(estModel.CarImgPath ?? "", valToken.sesCarImgPath, CommonConst.def_DmyImg);
                valToken.sesCarImgPath1 = CommonFunction.chkImgFile(estModel.CarImgPath1 ?? "", valToken.sesCarImgPath1, "");
                valToken.sesCarImgPath2 = CommonFunction.chkImgFile(estModel.CarImgPath2 ?? "", valToken.sesCarImgPath2, "");
                valToken.sesCarImgPath3 = CommonFunction.chkImgFile(estModel.CarImgPath3 ?? "", valToken.sesCarImgPath3, "");
                valToken.sesCarImgPath4 = CommonFunction.chkImgFile(estModel.CarImgPath4 ?? "", valToken.sesCarImgPath4, "");
                valToken.sesCarImgPath5 = CommonFunction.chkImgFile(estModel.CarImgPath5 ?? "", valToken.sesCarImgPath5, "");
                valToken.sesCarImgPath6 = CommonFunction.chkImgFile(estModel.CarImgPath6 ?? "", valToken.sesCarImgPath6, "");
                valToken.sesCarImgPath7 = CommonFunction.chkImgFile(estModel.CarImgPath7 ?? "", valToken.sesCarImgPath7, "");
                valToken.sesCarImgPath8 = CommonFunction.chkImgFile(estModel.CarImgPath8 ?? "", valToken.sesCarImgPath8, "");
                // その他費用の対応前のデータの場合
                if (estSubModel.Sonota == 0 && (estSubModel.RakuSatu + estSubModel.Rikusou) > 0)
                {
                    estSubModel.Sonota = estSubModel.RakuSatu + estSubModel.Rikusou;
                    estModel.CarPrice = estModel.CarPrice - estSubModel.Sonota;
                }
                if (string.IsNullOrEmpty(estSubModel.SonotaTitle))
                {
                    estSubModel.SonotaTitle = CommonConst.def_TitleSonota;
                }
                responseEst = _helperMapper.MergeInto<EstModel>(estModel, estSubModel);
                responseEst = creDispData(responseEst);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getEstData - CEST-040D");
                return responseEst;
            }
            return responseEst;
        }

        /// <summary>
        /// 見積書データ表示用整形
        /// </summary>
        public EstModel creDispData(EstModel model)
        {
            int intCornerType = 0;
            // AA情報
            if (model.Aano != "")
            {
                if (model.Mode == 1)
                {
                    intCornerType = _commonFuncHelper.GetCornerType(model.Corner);
                    if (intCornerType == 1)
                        model.AAInfo = string.Format("お問合せ番号{0}00-{1:00000}-{2:00000}", intCornerType, model.Aacount, Convert.ToInt32(model.Aano));
                    else
                        model.AAInfo = string.Format("お問合せ番号{0}00-{1}{2}", intCornerType, model.Corner, model.Aano);
                }
                else
                    model.AAInfo = model.Aaplace + "　No：" + model.Aano;
            }

            if (model.CheckCarYm != "無し" || !string.IsNullOrEmpty(model.CheckCarYm))
            {
                model.DamageInsEquivalent = 0;
                model.DamageIns = 0;
            }
            return model;
        }

        /// <summary>
        /// 新見積書番号取得
        /// </summary>
        /// <param name="outEstNo"></param>
        /// <returns></returns>
        public bool getEstNoFromDb(ref string outEstNo)
        {
            try
            {
                var dtNow = DateTime.Now;
                string iYear = Strings.Right(dtNow.Year.ToString(), 2);
                string iMonth = Strings.Format(dtNow.Month, "00");
                string iDay = Strings.Format(dtNow.Day, "00");
                string strNow = iYear + iMonth + iDay;
                var estNo = _unitOfWork.Estimates.Query(n => n.EstNo.Substring(0, 6) == strNow).Max(n => n.EstNo);
                outEstNo = estNo == null ? strNow + "00001" : strNow + Strings.Format(Convert.ToInt32(Strings.Right(estNo, 5)) + 1, "00000");            }
            catch (Exception ex)
            {
                // エラーログ書出し
                _logger.LogError(ex, "getEstNoFromDb - CEST-020D");
                return false;
            }
            return true;
        }
        public bool getEstSubNoFromDb(string inEstNo, ref string outEstSubNo)
        {
            try
            {
                var estSubNo = _unitOfWork.Estimates.Query(n => n.EstNo == inEstNo).Max(n => n.EstSubNo);
                outEstSubNo = estSubNo == null ? "01" : Strings.Format(Convert.ToInt32(estSubNo) + 1, "00");
            }
            catch (Exception ex)
            {
                // エラーログ書出し
                _logger.LogError(ex, "getEstSubNoFromDb - CEST-030D");
                return false;
            }

            return true;
        }

        public ResponseBase<EstModel> setEstData(string estNo, string estSubNo)
        {
            // 見積書データ取得
            var estData = getEstData(estNo, estSubNo);
            if (estData == null)
            {           
                return ResponseHelper.Error<EstModel>(HelperMessage.SMAL041D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAL041D));
            }
            return ResponseHelper.Ok<EstModel>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), estData);
        }

        // ******************************************
        // 会員ユーザーのお客様の情報をセッションにセット
        // または、セッションに保持していた会員ユーザーのお客様の情報をクリア
        // ******************************************
        public EstimateIdeModel setEstIDEData(ref LogToken logToken)
        {
            // get [t_EstimateIde]
            var dataEstIDE = getEstIDEData(logToken.sesEstNo, logToken.sesEstSubNo);
            if (dataEstIDE == null)
            {
                return null;
            }
            // get [MT_IDE_CONTRACT_PLAN]
            var getContractPlan = _unitOfWorkIDE.ContractPlans.GetSingleOrDefault(x => x.Id == dataEstIDE.ContractPlanId);
            dataEstIDE.ContractPlanName = getContractPlan == null ? "" : getContractPlan.PlanName;
            // get [MT_IDE_VOLUNTARY_INSURANCE]
            var getVoluntaryInsurance = _unitOfWorkIDE.VoluntaryInsurances.GetSingleOrDefault(x => x.Id == dataEstIDE.InsuranceCompanyId);
            dataEstIDE.InsuranceCompanyName = getVoluntaryInsurance == null ? "" : getVoluntaryInsurance.CompanyName;

            return dataEstIDE;
        }
        public EstimateIdeModel getEstIDEData(string inEstNo, string inEstSubNo)
        {
            var dataIDE = new EstimateIdeModel();
            try
            {
                var estIdeModel = _unitOfWork.EstimateIdes.GetSingleOrDefault(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo);
                if (estIdeModel == null)
                {
                    estIdeModel = new TEstimateIde();                    
                }
                estIdeModel.IsExtendedGuarantee = unchecked((byte)(-1));
                dataIDE = _mapper.Map<EstimateIdeModel>(estIdeModel);            }
            catch (Exception ex)
            {             
                _logger.LogError(ex, "getEstIDEData - CEST-040D");
                return null;
            }
            return dataIDE;
        }

        /// <summary>
        /// 見積書データ取得
        /// </summary>
        /// <param name="inEstNo"></param>
        /// <param name="inEstSubNo"></param>
        /// <returns></returns>
        public EstModel getEst_EstSubData(string inEstNo, string inEstSubNo)
        {
            try
            {
                // get [t_Estimate]
                var estModel = _unitOfWork.Estimates.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false);

                // get [t_EstimateSub]
                var estSubModel = _unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false);

                return _helperMapper.MergeInto<EstModel>(estModel, estSubModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getEstData - CEST-040D");
                return null;
            }
        }

        /// <summary>
        /// Get table t_EstimateSub
        /// </summary>
        /// <param name="inEstNo"></param>
        /// <param name="inEstSubNo"></param>
        /// <returns></returns>
        public TEstimateSub getEstSubData(string inEstNo, string inEstSubNo)
        {
            try
            {
                // get [t_EstimateSub]
                var estSubModel = _unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false);

                return estSubModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getEstData - CEST-040D");
                return null;
            }
        }
    }
}
