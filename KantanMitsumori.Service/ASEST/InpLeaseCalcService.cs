using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KantanMitsumori.Service
{
    public class InpLeaseCalcService : IInpLeaseCalcService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfWorkIDE _unitOfWorkIDE;
        private CommonEstimate _commonEst;
        private CommonCalLease _calLease;
        private readonly TestSettings _testSettings;
        private List<string> lstWriteLog = new List<string>();

        public InpLeaseCalcService(IMapper mapper, CommonEstimate commonEst, IOptions<TestSettings> testSettings, IUnitOfWorkIDE unitOfWorkIDE, ILogger<InpLeaseCalcService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonEst = commonEst;
            _unitOfWorkIDE = unitOfWorkIDE;
            _testSettings = testSettings.Value;
        }
        public ResponseBase<ResponseInpLease> GetDataInpLease(LogSession LogSession)
        {
            try
            {
                var response = new ResponseInpLease
                {
                    EstIDEModel = new EstimateIdeModel(),
                    EstModel = new EstModel(),
                    EstModelView = new InpLeaseModelView()

                };
                var estData = _commonEst.SetEstData(LogSession.sesEstNo!, LogSession.sesEstSubNo!);
                if (estData.ResultStatus == (int)enResponse.isSuccess)
                    response.EstModel = estData.Data!;
                if (response.EstModel.LeaseFlag == "1")
                {
                    response.EstIDEModel = _commonEst.SetEstIDEData(LogSession);
                    if (response.EstIDEModel == null)
                        return ResponseHelper.Error<ResponseInpLease>(HelperMessage.SMAL041D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
                }
                response = BindingData(response);
                return ResponseHelper.Ok(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetDataEstimate");
                return ResponseHelper.Error<ResponseInpLease>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
            }
        }


        public ResponseBase<List<ResponseCarType>> GetCarType()
        {
            try
            {
                var data = _unitOfWorkIDE.CarTypes.GetAll().Select(i => _mapper.Map<ResponseCarType>(i)).ToList();
                return ResponseHelper.Ok<List<ResponseCarType>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCarType");
                return ResponseHelper.Error<List<ResponseCarType>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public ResponseBase<List<ResponseContractPlan>> GetContractPlan()
        {
            try
            {
                var data = _unitOfWorkIDE.ContractPlans.GetAll().Select(i => _mapper.Map<ResponseContractPlan>(i)).ToList();
                return ResponseHelper.Ok<List<ResponseContractPlan>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetContractPlan");
                return ResponseHelper.Error<List<ResponseContractPlan>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public ResponseBase<List<ResponseFirstAfterSecondTerm>> GetFirstAfterSecondTerm(int carType)
        {
            try
            {
                var data = _unitOfWorkIDE.Inspections.Query(n => n.CarType == carType).Select(i => _mapper.Map<ResponseFirstAfterSecondTerm>(i)).ToList();
                return ResponseHelper.Ok<List<ResponseFirstAfterSecondTerm>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFirstAfterSecondTerm");
                return ResponseHelper.Error<List<ResponseFirstAfterSecondTerm>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public ResponseBase<ResponseUnitPriceRatesLimit> GetUnitPriceRatesLimit()
        {
            try
            {

                var data = new ResponseUnitPriceRatesLimit();
                var UnitPrice = _unitOfWorkIDE.UnitPrices.GetAll().FirstOrDefault()!.UnitPrice;
                var dt = _unitOfWorkIDE.FeeAdjustments.GetAll().FirstOrDefault()!;
                data.UnitPrice = UnitPrice;
                data.LowerLimit = dt.LowerLimit;
                data.UpperLimit = dt.UpperLimit;
                return ResponseHelper.Ok<ResponseUnitPriceRatesLimit>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCarType");
                return ResponseHelper.Error<ResponseUnitPriceRatesLimit>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public ResponseBase<List<ResponseVolInsurance>> GetVolInsurance()
        {
            try
            {
                var data = _unitOfWorkIDE.VoluntaryInsurances.GetAll().Select(i => _mapper.Map<ResponseVolInsurance>(i)).ToList();
                return ResponseHelper.Ok<List<ResponseVolInsurance>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetVolInsurance");
                return ResponseHelper.Error<List<ResponseVolInsurance>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }
        /// <summary>
        /// InpLeaseCal
        /// </summary>
        /// <param name="model"></param>
        /// <param name="LogSession"></param>
        /// <returns></returns>
        /// Create Date [20221008] by HoaiPhong
        public ResponseBase<ResponseInpLeaseCalc> InpLeaseCal(RequestInpLeaseCalc model, LogSession LogSession)
        {
            try
            {
                decimal dPriceEnd = 0; decimal dPriceMonthly = 0;
                decimal dPriceMonthNoTax = 0; decimal dPriceProcedure = 0;
                decimal dPricePrincipalInterest = 0;
                var data = new ResponseInpLeaseCalc();
                var estimates = _unitOfWork.Estimates.GetSingle(n => n.EstNo == LogSession.sesEstNo && n.EstSubNo == LogSession.sesEstSubNo);
                var estimateSubs = _unitOfWork.EstimateSubs.GetSingle(n => n.EstNo == LogSession.sesEstNo && n.EstSubNo == LogSession.sesEstSubNo);
                if (estimates == null || estimateSubs == null)
                {
                    return ResponseHelper.Error<ResponseInpLeaseCalc>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                if (!IsFirstRegOverLeaseSttMonth(model))
                {
                    data.IsError = true;
                    return ResponseHelper.Ok<ResponseInpLeaseCalc>(HelperMessage.I0003, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0003), data);

                }
                _logger.LogDebug("\"-------************* EstNo:{0} EstSubNo: {1} *************-------\"", estimates.EstNo, estimates.EstSubNo);
                _calLease = new CommonCalLease(_logger, _unitOfWorkIDE, lstWriteLog, model, _testSettings);
                var consumptionTax = (decimal)_calLease.GetConsumptionTax();
                var price = _calLease.GetPrice(estimates.SalesSum, estimates.TaxInsAll, estimates.TaxFreeAll);  // '4-2 get Price
                var vehicleTaxPrice = _calLease.GetVehicleTaxWithinTheTerm((int)estimates.AutoTax!, estimateSubs.DispVolUnit!, Convert.ToInt32(estimates.DispVol!));  // '4-3 VehicleTaxPrice
                var priceInsurance = _calLease.GetPriceinsurance();   // '4-4 getPriceinsurance()
                var priceWeighTax = _calLease.GetPriceWeighTax();  // '4-5 getPriceWeighTax()
                var pricePromotional = _calLease.GetPricePromotional((int)estimates.SalesSum!);   // 4-6 getPricePromotional()
                var pricetPropertyFeeIdemitsu = _calLease.GetPropertyFeeIdemitsu();   // '4-7 getPropertyFeeIdemitsu()
                var priceGuaranteeFee = _calLease.GetGuaranteeFee();     // '4-8 getGuaranteeFee()
                var priceNameChange = _calLease.GetPriceNameChange();    // '4-9 getPriceNameChange()
                var priceMantance = _calLease.GetPriceMantance();  // '4-10 getPriceMantance()
                var interest = _calLease.GetInterest();  // '4-11 getInterest() 
                dPriceMonthNoTax = (price + vehicleTaxPrice + priceInsurance + priceWeighTax + pricePromotional + pricetPropertyFeeIdemitsu + priceGuaranteeFee + priceNameChange + priceMantance + model.InsuranceFee);
                dPriceMonthNoTax = dPriceMonthNoTax - (model.TradeIn / (1 + consumptionTax)) - (model.PrePay / (1 + consumptionTax));
                // 'tien goc chiu lai
                dPricePrincipalInterest = dPriceMonthNoTax;
                dPriceMonthNoTax = CommonFunction.ToRoundDown(dPriceMonthNoTax, 0);
                //'dPriceMonthNoTax phi lease 1 thang khong bao gom thue
                var pricePTM = CommonFunction.PMT(interest, model.ContractTimes, dPriceMonthNoTax);
                dPriceMonthNoTax = (decimal)CommonFunction.ToRoundUp(Convert.ToDouble(pricePTM), -2);
                //'dPriceProcedure phi thu tuc
                dPriceProcedure = (model.AdjustFee / (1 + consumptionTax)) / model.ContractTimes;
                dPriceProcedure = CommonFunction.ToRoundDown(dPriceProcedure, -2);
                //'dPriceMonthly phi lease moi thang
                dPriceMonthly = (decimal)CommonFunction.ToRoundUp(Convert.ToDouble(dPriceMonthNoTax + dPriceProcedure), -2);
                // 'dPriceEnd phi lease hang thang cuoi cung 
                dPriceEnd = CommonFunction.ToRoundDown(dPriceMonthly + (dPriceMonthly * consumptionTax), 0);
                data.ListUILog = _calLease._lstWriteLogUI;
                data.PriceEnd = CommonFunction.setFormatCurrency(dPriceEnd, "");
                saveLog(consumptionTax, model, estimates, price, vehicleTaxPrice, priceInsurance, priceWeighTax, pricePromotional, pricetPropertyFeeIdemitsu, priceGuaranteeFee, priceNameChange, priceMantance, interest);
               _logger.LogDebug("金利対象元本(A)= {0}", dPricePrincipalInterest);
                _calLease.addLogUI("金利対象元本(A)= " + dPricePrincipalInterest);
                saveLogFinal(consumptionTax, dPriceEnd, dPriceMonthly, dPriceMonthNoTax, dPriceProcedure);
                var priceLeaseFeeLowerLimit = PriceLeaseFeeLowerLimit();
                if (dPriceEnd < priceLeaseFeeLowerLimit)
                {
                    data.IsError = true;
                    data.PriceLeaseFeeLowerLimit = priceLeaseFeeLowerLimit;
                    return ResponseHelper.Ok<ResponseInpLeaseCalc>(HelperMessage.I0003, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0003), data);

                }
                else
                {
                    var result = EstInsertUpdateData(model, estimates, LogSession, dPriceEnd, pricePromotional, priceNameChange,
                        interest, priceGuaranteeFee, priceMantance, vehicleTaxPrice, priceInsurance, priceWeighTax, 0, pricePromotional);
                    if (result)
                        data.IsShowButton = 1;
                    else
                        return ResponseHelper.Error<ResponseInpLeaseCalc>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));

                }
                return ResponseHelper.Ok<ResponseInpLeaseCalc>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InpLeaseCal");
                return ResponseHelper.Error<ResponseInpLeaseCalc>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

            }
        }
        public async Task<ResponseBase<int>> UpdateLeaseProgressIde(int leaseProgress, LogSession LogSession)
        {
            try
            {
                var dtEstimateIdes = _unitOfWork.EstimateIdes.GetSingle(n => n.EstNo == LogSession.sesEstNo && n.EstSubNo == LogSession.sesEstSubNo);
                if (dtEstimateIdes == null)
                {
                    return ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                dtEstimateIdes.LeaseProgress = leaseProgress;
                dtEstimateIdes.IsApplyLease = 0;
                dtEstimateIdes.CreateDate = DateTime.Now;
                dtEstimateIdes.UpdateDate = DateTime.Now;
                dtEstimateIdes.UpdateUser = LogSession.UserNo;
                _unitOfWork.EstimateIdes.Update(dtEstimateIdes);
                await _unitOfWork.CommitAsync();
                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateLeaseProgressIde");
                return ResponseHelper.Error<int>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }
        #region Func  private
        private void saveLog(decimal consumptionTax, RequestInpLeaseCalc model, TEstimate oEst, decimal dPrice,
            decimal dVehicleTaxPrice, decimal priceInsurance, decimal priceWeighTax, decimal pricePromotional, decimal pricetPropertyFeeIdemitsu,
            decimal priceGuaranteeFee, decimal priceNameChange, decimal priceMantance, double interest)
        {
           _logger.LogDebug("-------**********LOG FILE EXCEL************-------");
           _logger.LogDebug("15.お支払総額: {0}", oEst.SalesSum.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("7.税金・保険料(非課税): {0}", oEst.TaxInsAll.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("9.預り法定費用(非課税): {0}", oEst.TaxFreeAll.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("期間中 自動車税: {0}", dVehicleTaxPrice.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("期間中 重量税: {0}", priceWeighTax.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("期間中 自賠責保険: {0}", priceInsurance.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("自動車保険料: {0}", model.InsuranceFee.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("販売促進費 設定計数: {0}", _calLease.promotion.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("出光興産手数料（税抜): {0} ", _calLease.pricePropertyFee1.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("出光クレジット月額手数料（税抜）※: ", _calLease.logCreditFee.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("特販店手数料（税抜): {0} ", _calLease.pricePropertyFee2.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("SMAS手数料（税抜）額: {0}", _calLease.pricePropertyFee3.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("保証料（税抜) : {0}", priceGuaranteeFee.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("名義変更費用（税抜）: {0}", priceNameChange.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("月額メンテナンス料（税抜）:{0} ", _calLease.myMaintancePrice.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("頭金（税込）:{0} ", model.PrePay.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("下取（税込）:{0} ", model.TradeIn.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("手数料調整額（税込）:{0} ", model.AdjustFee.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("適用金利(年利):{0} ", interest.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("リース期間:{0} ", model.ContractTimes.ToString());
           _logger.LogDebug("--------------");
           _logger.LogDebug("初度登録月:{0} ", CommonFunction.getFormatDayYMD(model.FirstReg!));
           _logger.LogDebug("--------------");
           _logger.LogDebug("車検満了日（予定日）: {0}", CommonFunction.getFormatDayYMD(model.ExpiresDate!));
           _logger.LogDebug("--------------");
           _logger.LogDebug("リース開始月: {0}", CommonFunction.getFormatDayYMD(CommonFunction.Left(model.LeaseSttMonth!, 6)));
           _logger.LogDebug("--------------");
           _logger.LogDebug("リース開始日: {0}{1}", model.LeaseSttDay.ToString(), "日");
           _logger.LogDebug("--------------");
           _logger.LogDebug("リース満了日: {0}", CommonFunction.getFormatDayYMD(model.LeaseExpirationDate!));
           _logger.LogDebug("--------------");
           _logger.LogDebug("消費税率: {0}", _calLease.consumptionTax.ToString());
            if (_testSettings.IsShowLogUI == "True")
            {
                // Save log ui
                _calLease.addLogUI("-------**********LOG FILE EXCEL************-------");
                _calLease.addLogUI("15.お支払総額: " + oEst.SalesSum.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("7.税金・保険料(非課税): " + oEst.TaxInsAll.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("9.預り法定費用(非課税): " + oEst.TaxFreeAll.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("期間中 自動車税: " + dVehicleTaxPrice.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("期間中 重量税: " + priceWeighTax.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("期間中 自賠責保険: " + priceInsurance.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("自動車保険料: " + model.InsuranceFee.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("販売促進費 設定計数: " + _calLease.promotion.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("出光興産手数料（税抜): " + _calLease.pricePropertyFee1.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("出光クレジット月額手数料（税抜）※: " + _calLease.logCreditFee.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("特販店手数料（税抜): " + _calLease.pricePropertyFee2.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("SMAS手数料（税抜）額: " + _calLease.pricePropertyFee3.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("保証料（税抜): " + priceGuaranteeFee.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("名義変更費用（税抜): " + priceNameChange.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("月額メンテナンス料（税抜): " + _calLease.myMaintancePrice.ToString().ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("頭金（税込): " + model.PrePay.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("下取（税込): " + model.TradeIn.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("手数料調整額（税込): " + model.AdjustFee.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("適用金利(年利): " + interest.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("リース期間: " + model.ContractTimes.ToString());
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("初度登録月: " + CommonFunction.getFormatDayYMD(model.FirstReg!));
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("車検満了日（予定日): " + CommonFunction.getFormatDayYMD(model.ExpiresDate!));
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("リース開始月: " + CommonFunction.getFormatDayYMD(CommonFunction.Left(model.LeaseSttMonth!, 6)));
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("リース開始日: " + model.LeaseSttDay.ToString() + "日");
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("リース満了日: " + CommonFunction.getFormatDayYMD(model.LeaseExpirationDate!));
                _calLease.addLogUI("--------------");
                _calLease.addLogUI("消費税率: " + _calLease.consumptionTax.ToString());
            }

        }
        private void saveLogFinal(decimal consumptionTax, decimal dPriceEnd, decimal dPriceMonthly, decimal dPriceMonthNoTax, decimal dPriceProcedure)
        {
            var priceMonthNoTax = CommonFunction.ToRoundDown(dPriceMonthNoTax + (dPriceMonthNoTax * consumptionTax), 0);
           _logger.LogDebug("1【税別】月額リース料(B)（10円単位切上）={0}", dPriceMonthNoTax);
           _logger.LogDebug("2【税込】月額リース料（Ｃ）　小数点第一位を四捨五入={0} ", priceMonthNoTax);
           _logger.LogDebug("3【税別】手数料調整額 月額増・減算リース料(D)　(10円単位切捨)={0} ", dPriceProcedure);
           _logger.LogDebug("4手数料調整あり　【税別】最終リース料(E)　(10円単位切上)={0} ", dPriceMonthly);
           _logger.LogDebug("5手数料調整あり　【税込】最終リ―ス料（F）　小数点第一位四捨五入 ={0} ", dPriceEnd);
           _logger.LogDebug("-----------------************* END LEASE *************-------------------------");
            if (_testSettings.IsShowLogUI == "True")
            {
                _calLease.addLogUI("1【税別】月額リース料(B)（10円単位切上= " + dPriceMonthNoTax);
                _calLease.addLogUI("2【税込】月額リース料（Ｃ）　小数点第一位を四捨五入= " + priceMonthNoTax);
                _calLease.addLogUI("3【税別】手数料調整額 月額増・減算リース料(D)　(10円単位切捨)= " + dPriceProcedure);
                _calLease.addLogUI("4手数料調整あり　【税別】最終リース料(E)　(10円単位切上)= " + dPriceMonthly);
                _calLease.addLogUI("5手数料調整あり　【税込】最終リ―ス料（F）　小数点第一位四捨五入 = " + dPriceEnd);
                _calLease.addLogUI("-----------------************* END LEASE *************-------------------------");
            }

        }

        private int PriceLeaseFeeLowerLimit()
        {
            var dt = _unitOfWorkIDE.LeaseFeeLowerLimits.GetAll().FirstOrDefault();
            if (dt != null)
            {
                return dt.LeaseFeeLowerLimit;
            }
            return 0;
        }

        private bool EstInsertUpdateData(RequestInpLeaseCalc requestModel, TEstimate oEst, LogSession LogSession,
            decimal iMonthlyLeaseFee, decimal iPromotionFee, decimal iNameChange, double iInterest,
            decimal iGuaranteeCharge, decimal iMaintenancePrice, decimal iCarTax, decimal iLiabilityInsurance,
            decimal iWeightTax, decimal iLeaseProgress, decimal pricePromotional)
        {
            try
            {
                var oEstIde = new TEstimateIde();
                oEstIde.EstNo = LogSession.sesEstNo!;
                oEstIde.EstSubNo = LogSession.sesEstSubNo!;
                oEstIde.EstUserNo = LogSession.UserNo!;
                oEstIde.CarType = requestModel.CarType;
                oEstIde.IsElectricCar = (byte)requestModel.ElectricCar;
                oEstIde.FirstRegistration = CommonFunction.Left(requestModel.FirstReg!, 6);
                oEstIde.InspectionExpirationDate = requestModel.ExpiresDate!;
                oEstIde.LeaseStartMonth = CommonFunction.Left(requestModel.LeaseSttMonth!, 6);
                oEstIde.LeasePeriod = requestModel.ContractTimes;
                oEstIde.LeaseExpirationDate = requestModel.LeaseExpirationDate!;
                oEstIde.ContractPlanId = requestModel.ContractPlan;
                oEstIde.IsExtendedGuarantee = (byte)requestModel.InsurExpanded;
                oEstIde.InsuranceCompanyId = requestModel.InsuranceCompany;
                oEstIde.InsuranceFee = requestModel.InsuranceFee;
                oEstIde.DownPayment = requestModel.PrePay;
                oEstIde.TradeInPrice = requestModel.TradeIn;
                oEstIde.FeeAdjustment = requestModel.AdjustFee;
                oEstIde.MonthlyLeaseFee = (int)iMonthlyLeaseFee;
                oEstIde.IdemitsuKosanFee = _calLease.pricePropertyFee1;
                oEstIde.SalesStoreFee = _calLease.pricePropertyFee2;
                oEstIde.Smasfee = _calLease.pricePropertyFee3;
                oEstIde.IdemitsuCreditFee = _calLease.pricePropertyFee4;
                oEstIde.Promotion = _calLease.promotion;
                oEstIde.PromotionFee = (int)iPromotionFee;
                oEstIde.ConsumptionTax = _calLease.consumptionTax;
                oEstIde.NameChange = (int)iNameChange;
                oEstIde.FeeAdjustmentMax = _unitOfWorkIDE.FeeAdjustments.GetAll().FirstOrDefault()!.UpperLimit;
                oEstIde.FeeAdjustmentMin = _unitOfWorkIDE.FeeAdjustments.GetAll().FirstOrDefault()!.LowerLimit;
                oEstIde.Interest = iInterest;
                oEstIde.GuaranteeCharge = (int)iGuaranteeCharge;
                oEstIde.MyMaintenancePrice = (int)iMaintenancePrice;
                oEstIde.CarTax = (int)iCarTax;
                oEstIde.LiabilityInsurance = (int)iLiabilityInsurance;
                oEstIde.WeightTax = (int)iWeightTax;
                oEstIde.LeaseProgress = (int)iLeaseProgress;
                oEstIde.IsApplyLease = 0;
                oEstIde.CreateDate = DateTime.Now;
                oEstIde.UpdateDate = DateTime.Now;
                oEstIde.UpdateUser = LogSession.UserNo;
                var isCheckData = _unitOfWork.EstimateIdes.GetSingle(n => n.EstNo == oEst.EstNo && n.EstSubNo == oEst.EstSubNo);
                if (isCheckData != null)
                {
                    _unitOfWork.EstimateIdes.Update(oEstIde);
                    _unitOfWork.Commit();
                    return true;
                }
                else
                {
                    _unitOfWork.EstimateIdes.Add(oEstIde);
                    _unitOfWork.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("EstInsertUpdateData: {0}", ex.ToString());
                return false;
            }
        }

        private bool IsFirstRegOverLeaseSttMonth(RequestInpLeaseCalc model)
        {
            var restriction = getRestriction();
            DateTime _firstReg = DateTime.Parse(CommonFunction.ConvertDate(model.FirstReg!));
            DateTime _leaseSttMonth = DateTime.Parse(CommonFunction.ConvertDate(model.LeaseSttMonth!));
            _firstReg = _firstReg.AddYears(restriction);
            if (_firstReg < _leaseSttMonth)
            {
                return false;
            }
            return true;
        }


        private int getRestriction()
        {
            int dRestriction = 0;
            var leaseTargets = _unitOfWorkIDE.LeaseTargets.GetSingle(n => n.Id == 1);
            if (leaseTargets != null)
            {
                dRestriction = leaseTargets.Restriction;
            }
            return dRestriction;

        }
        private ResponseInpLease BindingData(ResponseInpLease Model)
        {
            Model.EstModelView.FirstRegistration = string.IsNullOrEmpty(Model.EstIDEModel!.FirstRegistration) ? Model.EstModel!.FirstRegYm : Model.EstIDEModel.FirstRegistration;
            var IdeModel = Model.EstIDEModel;
            Model.EstModelView.IsData = !string.IsNullOrEmpty(IdeModel.EstNo) ? "1" : "0";
            Model.EstModelView.lbl_MonthlyLease = IdeModel.MonthlyLeaseFee != 0 ? CommonFunction.setFormatCurrency(IdeModel.MonthlyLeaseFee, "") : "";
            Model.EstModelView.Label15 = IdeModel.MonthlyLeaseFee != 0 ? "円" : "";
            Model.EstModelView.CheckCarYm = string.IsNullOrEmpty(Model.EstModel.CheckCarYm) || Model.EstModel.CheckCarYm.Contains("無し") ? "99999999" : Model.EstModel.CheckCarYm;
            Model.EstModelView.InspectionExpirationDate = string.IsNullOrEmpty(IdeModel.InspectionExpirationDate) ? Model.EstModelView.CheckCarYm : IdeModel.InspectionExpirationDate;
            Model.EstModelView.LeaseStartMonth = string.IsNullOrEmpty(IdeModel.LeaseStartMonth) ? DateTime.Now.ToString("yyyyMM") : IdeModel.LeaseStartMonth;
            Model.EstModelView.InsuranceCompanyId = IdeModel.InsuranceCompanyId == -1 ? 0 : IdeModel.InsuranceCompanyId;
            return Model;
        }

        #endregion Func  private
    }

}
