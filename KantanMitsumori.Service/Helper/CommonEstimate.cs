using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Response;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
        private readonly CommonFuncHelper _commonFuncHelper;
        private readonly List<string> reCalEstModel;
        private readonly List<string> reCalEstSubModel;

        public CommonEstimate(ILogger<CommonEstimate> logger, IUnitOfWork unitOfWork, IUnitOfWorkIDE unitOfWorkIDE, IMapper mapper, HelperMapper helperMapper, CommonFuncHelper commonFuncHelper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _unitOfWorkIDE = unitOfWorkIDE;
            _mapper = mapper;
            _commonFuncHelper = commonFuncHelper;
            _helperMapper = helperMapper;

            valToken = new LogToken();
            reCalEstModel = new List<string>() { "CarPrice", "Discount", "SyakenNew", "SyakenZok", "OptionPrice1", "OptionPrice2", "OptionPrice3", "OptionPrice4", "OptionPrice5", "OptionPrice6", "OptionPrice7", "OptionPrice8", "OptionPrice9", "OptionPrice10", "OptionPrice11", "OptionPrice12", "TaxCheck", "TaxGarage", "TaxTradeIn", "TaxRecycle", "TaxDelivery", "TaxOther" };
            reCalEstSubModel = new List<string>() { "YtiRieki", "RakuSatu", "Rikusou", "TaxTradeInSatei", "TaxSet1", "TaxSet2", "TaxSet3", "AutoTaxEquivalent", "DamageInsEquivalent" };
        }
        public async Task<bool> CalcSum(string inEstNo, string inEstSubNo, LogToken logToken)
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
                var vTax = _commonFuncHelper.getTax((DateTime)estModel.Udate!, logToken.sesTaxRatio, logToken.UserNo!);
                valToken.sesTaxRatio = vTax;
                var dtUserDef = _commonFuncHelper.getUserDefData(logToken.UserNo!);
                if (dtUserDef != null)
                {
                    if (estModel.ConTaxInputKb != dtUserDef.ConTaxInputKb)
                    {
                        estModel.ConTaxInputKb = dtUserDef.ConTaxInputKb;

                        var arrayEst = estModel.GetType().GetProperties().Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>));
                        var arrayEstSub = estSubModel.GetType().GetProperties().Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>));
                        foreach (var itemEst in arrayEst)
                        {
                            if (reCalEstModel.Contains(itemEst.Name))
                            {
                                int objValue = (int)itemEst.GetValue(estModel)!;
                                objValue = CommonFunction.reCalcItem(objValue, (bool)estModel.ConTaxInputKb, vTax);
                                itemEst.SetValue(estModel, objValue);
                            }
                        }
                        foreach (var itemSub in arrayEstSub)
                        {
                            if (reCalEstSubModel.Contains(itemSub.Name))
                            {
                                int objValue = (int)itemSub.GetValue(estModel)!;
                                objValue = CommonFunction.reCalcItem(objValue, (bool)estModel.ConTaxInputKb, vTax);
                                itemSub.SetValue(estModel, objValue);
                            }
                        }
                    }
                }
                estSubModel.Sonota = estSubModel.RakuSatu
                                   + estSubModel.Rikusou;
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
                estModel.CarSum = estModel.CarPrice
                                - estModel.Discount
                                + estSubModel.Sonota
                                + estModel.SyakenNew
                                + estModel.SyakenZok
                                + estModel.OptionPriceAll;
                estModel.TaxInsAll = estModel.AutoTax
                            + estModel.AcqTax
                            + estModel.WeightTax
                            + estModel.DamageIns
                            + estModel.OptionIns;
                estSubModel.TaxInsEquivalentAll = estSubModel.AutoTaxEquivalent
                                                + estSubModel.DamageInsEquivalent;
                estModel.TaxFreeAll = estModel.TaxFreeCheck
                                    + estModel.TaxFreeGarage
                                    + estModel.TaxFreeTradeIn
                                    + estModel.TaxFreeRecycle
                                    + estModel.TaxFreeOther
                                    + estSubModel.TaxFreeSet1
                                    + estSubModel.TaxFreeSet2;
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
                decimal? wkContax;
                if (estModel.ConTaxInputKb == false)
                {
                    wkContax = (decimal?)((estModel.CarSum + estSubModel.TaxInsEquivalentAll + estModel.TaxCostAll) * vTax);
                }
                else
                {
                    wkContax = (decimal?)((estModel.CarSum + estSubModel.TaxInsEquivalentAll + estModel.TaxCostAll) / (1 + vTax));
                    wkContax = Math.Ceiling((decimal)wkContax!);
                    wkContax *= vTax;
                }
                estModel.ConTax = Convert.ToInt32(Math.Floor((decimal)wkContax!));
                estModel.CarSaleSum = estModel.CarSum
                                    + estModel.TaxInsAll
                                    + estSubModel.TaxInsEquivalentAll
                                    + estModel.TaxFreeAll
                                    + estModel.TaxCostAll;
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
                    if (Convert.ToBoolean(estSubModel.LoanRecalcSettingFlag))
                    {
                        CommonSimLon simLon = new(_logger)
                        {
                            SaleSumPrice = Convert.ToInt32(estModel.SalesSum),
                            Deposit = Convert.ToInt32(estModel.Deposit),
                            MoneyRate = Convert.ToInt32(estModel.Rate),
                            PayTimes = Convert.ToInt32(estModel.PayTimes),
                            FirstMonth = Convert.ToInt32(CommonFunction.Right(estModel.FirstPayMonth ?? "", 2))
                        };
                        if (estModel.BonusAmount > 0)
                        {
                            simLon.Bonus = Convert.ToInt32(estModel.BonusAmount);
                            simLon.BonusFirst = Convert.ToInt32(estModel.BonusFirst);
                            simLon.BonusSecond = Convert.ToInt32(estModel.BonusSecond);
                        }
                        simLon.ConTax = vTax;
                        if (simLon.CalcRegLoan() == false)
                            strClearMsg = CommonConst.def_LoanInfo_Error.ToString();
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
                        strClearMsg = CommonConst.def_LoanInfo_Clear.ToString();
                }
                else
                {
                    estSubModel.LoanInfo = CommonConst.def_LoanInfo_Unexecuted;
                }
                if (strClearMsg != "")
                {
                    estModel.Rate = 0;
                    estModel.Deposit = 0;
                    estModel.Principal = estModel.SalesSum;
                    estModel.PartitionFee = 0;
                    estModel.PartitionAmount = 0;
                    estModel.PayTimes = 0;
                    estModel.FirstPayMonth = null;
                    estModel.LastPayMonth = null;
                    estModel.FirstPayAmount = 0;
                    estModel.PayAmount = 0;
                    estModel.BonusAmount = 0;
                    estModel.BonusFirst = null;
                    estModel.BonusSecond = null;
                    estModel.BonusTimes = 0;
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

        public EstModel GetEstData(string inEstNo, string inEstSubNo)
        {
            var responseEst = new EstModel();
            try
            {
                var estModel = _unitOfWork.Estimates.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false);
                var estSubModel = _unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false);
                valToken.sesCarImgPath = CommonFunction.chkImgFile(estModel.CarImgPath ?? "", valToken.sesCarImgPath!, CommonConst.def_DmyImg);
                valToken.sesCarImgPath1 = CommonFunction.chkImgFile(estModel.CarImgPath1 ?? "", valToken.sesCarImgPath1!, "");
                valToken.sesCarImgPath2 = CommonFunction.chkImgFile(estModel.CarImgPath2 ?? "", valToken.sesCarImgPath2!, "");
                valToken.sesCarImgPath3 = CommonFunction.chkImgFile(estModel.CarImgPath3 ?? "", valToken.sesCarImgPath3!, "");
                valToken.sesCarImgPath4 = CommonFunction.chkImgFile(estModel.CarImgPath4 ?? "", valToken.sesCarImgPath4!, "");
                valToken.sesCarImgPath5 = CommonFunction.chkImgFile(estModel.CarImgPath5 ?? "", valToken.sesCarImgPath5!, "");
                valToken.sesCarImgPath6 = CommonFunction.chkImgFile(estModel.CarImgPath6 ?? "", valToken.sesCarImgPath6!, "");
                valToken.sesCarImgPath7 = CommonFunction.chkImgFile(estModel.CarImgPath7 ?? "", valToken.sesCarImgPath7!, "");
                valToken.sesCarImgPath8 = CommonFunction.chkImgFile(estModel.CarImgPath8 ?? "", valToken.sesCarImgPath8!, "");
                if (estSubModel.Sonota == 0 && (estSubModel.RakuSatu + estSubModel.Rikusou) > 0)
                {
                    estSubModel.Sonota = estSubModel.RakuSatu + estSubModel.Rikusou;
                    estModel.CarPrice -= estSubModel.Sonota;
                }
                if (string.IsNullOrEmpty(estSubModel.SonotaTitle))
                {
                    estSubModel.SonotaTitle = CommonConst.def_TitleSonota;
                }
                responseEst = _helperMapper.MergeInto<EstModel>(estModel, estSubModel);              
                responseEst = CreDispData(responseEst);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getEstData - CEST-040D");
                return responseEst;
            }
            return responseEst;
        }
        public EstModel CreDispData(EstModel model)
        {
            if (model.Aano != "")
            {
                if (model.Mode == 1)
                {
                    int intCornerType = _commonFuncHelper.GetCornerType(model.Corner);
                    if (intCornerType == -1)
                    {
                        model.AAInfo = "";
                    }
                    else
                    if (intCornerType == 1)
                        model.AAInfo = string.Format("お問合せ番号{0}00-{1:00000}-{2:00000}", intCornerType, model.Aacount, Convert.ToInt32(model.Aano));
                    else
                        model.AAInfo = string.Format("お問合せ番号{0}00-{1}{2}", intCornerType, model.Corner, model.Aano);
                }
                else
                    model.AAInfo = model.Aaplace + "　No：" + model.Aano;
            }
            bool isCheckCarYm = model.CheckCarYm == "無し" || string.IsNullOrEmpty(model.CheckCarYm);
            if (!isCheckCarYm)
            {
                model.DamageInsEquivalent = 0;
                model.DamageIns = 0;
            }
            return model;
        }
        public bool GetEstNoFromDb(ref string outEstNo)
        {
            try
            {
                var dtNow = DateTime.Now;
                string iYear = CommonFunction.Right(dtNow.Year.ToString(), 2);
                string iMonth = string.Format("{0:D2}", dtNow.Month);
                string iDay = string.Format("{0:D2}", dtNow.Day);
                string strNow = iYear + iMonth + iDay;
                var estNo = _unitOfWork.Estimates.Query(n => n.EstNo.Substring(0, 6) == strNow).Max(n => n.EstNo);
                outEstNo = estNo == null ? strNow + "00001" : strNow + string.Format("{0:D5}", Convert.ToInt32(CommonFunction.Right(estNo, 5)) + 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getEstNoFromDb - CEST-020D");
                return false;
            }
            return true;
        }
        public bool GetEstSubNoFromDb(string inEstNo, ref string outEstSubNo)
        {
            try
            {
                var estSubNo = _unitOfWork.Estimates.Query(n => n.EstNo == inEstNo).Max(n => n.EstSubNo);
                outEstSubNo = estSubNo == null ? "01" : string.Format("{0:D2}", Convert.ToInt32(estSubNo) + 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getEstSubNoFromDb - CEST-030D");
                return false;
            }

            return true;
        }

        public ResponseBase<EstModel> SetEstData(string estNo, string estSubNo)
        {
            var estData = GetEstData(estNo, estSubNo);
           if (estData == null || string.IsNullOrEmpty(estData.AAInfo))
            {
                return ResponseHelper.Error<EstModel>(HelperMessage.SMAL041D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAL041D));
            }
            return ResponseHelper.Ok<EstModel>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), estData);
        }

        public EstimateIdeModel SetEstIDEData(LogToken logtoken)
        {
            var dataEstIDE = GetEstIDEData(logtoken.sesEstNo!, logtoken.sesEstSubNo!);
            if (dataEstIDE != null)
            {
                var dtContractPlan = _unitOfWorkIDE.ContractPlans.GetSingleOrDefault(x => x.Id == dataEstIDE.ContractPlanId);
                dataEstIDE.ContractPlanName = dtContractPlan == null ? "" : dtContractPlan.PlanName;
                var dtVoluntaryInsurance = _unitOfWorkIDE.VoluntaryInsurances.GetSingleOrDefault(x => x.Id == dataEstIDE.InsuranceCompanyId);
                dataEstIDE.InsuranceCompanyName = dtVoluntaryInsurance == null ? "" : dtVoluntaryInsurance.CompanyName;
            }
            return dataEstIDE!;
        }
        public EstimateIdeModel? GetEstIDEData(string inEstNo, string inEstSubNo)
        {
            var dataIDE = new EstimateIdeModel();
            try
            {
                var estIdeModel = _unitOfWork.EstimateIdes.GetSingleOrDefault(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo);
                if (estIdeModel == null)
                {
                    estIdeModel = new TEstimateIde();
                }
                //estIdeModel.IsExtendedGuarantee = unchecked((byte)(-1));
                dataIDE = _mapper.Map<EstimateIdeModel>(estIdeModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getEstIDEData - CEST-040D");
                return null;
            }
            return dataIDE;
        }


        public EstModel? GetEst_EstSubData(string inEstNo, string inEstSubNo)
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
        public TEstimateSub? GetEstSubData(string inEstNo, string inEstSubNo)
        {
            try
            {
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
