using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Model;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Reflection;

namespace KantanMitsumori.Service.Helper
{
    public class CommonEstimate
    {
        public EstimateModelView EstimateModelView { get; set; }

        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private LogToken valToken;
        private CommonFuncHelper _commonFuncHelper;

        private List<string> reCalEstModel;
        private List<string> reCalEstSubModel;

        public CommonEstimate(ILogger<CommonEstimate> logger, IUnitOfWork unitOfWork, IMapper mapper, CommonFuncHelper commonFuncHelper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _commonFuncHelper = commonFuncHelper;

            valToken = new LogToken();
            reCalEstModel = new List<string>();
            reCalEstSubModel = new List<string>();
        }

        public bool chkAANo(string? userNo, string AANo, string AAPlace, int CornerType, int mode)
        {
            try
            {
                var getSys = _unitOfWork.Syss.GetList(t => t.CornerType == CornerType).Select(s => new { Corner = s.Corner, Aacount = s.Aacount }).ToList();

                var getMaxEstSub = _unitOfWork.EstimateSubs.GetList(s => s.EstUserNo == userNo &&
                                                                    s.Aano == AANo &&
                                                                    s.Aaplace == AAPlace &&
                                                                    s.Mode == mode &&
                                                                    s.Dflag == false &&
                                                                    getSys.Any(m => m.Corner == s.Corner) &&
                                                                    getSys.Any(m => m.Aacount == s.Aacount)
                                                                    ).Max(a => new { maxEstNo = a.EstNo, maxEstSubNo = a.EstSubNo });

                if (getMaxEstSub?.maxEstNo != "")
                {
                    valToken.sesEstNo = getMaxEstSub!.maxEstNo;
                    valToken.sesEstSubNo = getMaxEstSub.maxEstSubNo;
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "chkAANo " + "GCMF-040D");
                return false;
                //return ResponseHelper.Error<bool>("Error", CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "GCMF-040D" + CommonConst.def_ErrCodeR);
            }
        }

        public bool addEstNextSubNo(bool flgRecreate = false)
        {
            // 見積書番号を取得
            string vEstNo = !string.IsNullOrEmpty(valToken.sesEstNo) ? valToken.sesEstNo : "";
            string vLeaseFlag = !string.IsNullOrEmpty(valToken.sesLeaseFlag) ? valToken.sesEstNo : "";
            string vEstSubNo = !string.IsNullOrEmpty(valToken.sesEstSubNo) ? valToken.sesEstNo : "";

            if (vEstNo == "" | vEstSubNo == "")
            {
                valToken.sesErrMsg = CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "CEST-050S" + CommonConst.def_ErrCodeR;
                return false;
            }

            // （諸費用設定の最新状態を反映しなければならない場合があるので必要）
            calcSum(vEstNo, vEstSubNo);

            // 見積書データ取得
            if (!getEstData(vEstNo, vEstSubNo))
            {
                valToken.sesErrMsg = CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "CEST-051D" + CommonConst.def_ErrCodeR;
                return false;
            }

            // 再作成の場合
            if (flgRecreate)
            {

            }

            return true;
        }

        /// <summary>
        /// * 見積書データ 小計・合計計算（税抜／税込切替時の調整、および小計・合計計算）
        /// </summary>
        /// <returns></returns>
        public bool calcSum(string inEstNo, string inEstSubNo)
        {
            try
            {
                var estModel = _mapper.Map<EstimateModel>(_unitOfWork.Estimates.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false));

                var estSubModel = _mapper.Map<EstimateSubModel>(_unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == estModel.EstNo && x.EstSubNo == estModel.EstSubNo && x.Dflag == false));

                valToken = new LogToken();  // TO DO: Remove

                // 再計算前の総額
                long oldSalesSum = estModel.SalesSum;

                // 消費税率取得
                var vTax = _commonFuncHelper.getTax((DateTime)estModel.Udate!, valToken.sesTaxRatio, estModel.EstUserNo);
                valToken.sesTaxRatio = vTax;

                // 会員諸費用設定取得
                string userNo = estModel.EstUserNo;

                var getUserDef = _commonFuncHelper.getUserDefData(userNo);

                if (getUserDef.ResultStatus == 0)
                {
                    if (estModel.ConTaxInputKb != getUserDef.Data!.ConTaxInputKb)
                    {
                        // 消費税区分（税込／税抜）がデータと設定値で不一致の場合、データの各項目を再設定
                        estModel.ConTaxInputKb = getUserDef.Data!.ConTaxInputKb;

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


                            int objValue = (int)propEst.GetValue(estModel);

                            if (reCalEstModel.Contains(propEst.Name))
                            {
                                objValue = CommonFunction.reCalcItem(objValue, estModel.ConTaxInputKb, vTax);
                            }

                            propEst.SetValue(estModel, objValue);
                        }

                        // cal [EstmateSub]
                        foreach (PropertyInfo propEstSub in propsEstSub)
                        {
                            string properties = propEstSub.Name;
                            // Do something with propValue
                            reCalEstSubModel.Add(propEstSub.Name);


                            int objValue = (int)propEstSub.GetValue(estModel);

                            if (reCalEstSubModel.Contains(propEstSub.Name))
                            {
                                objValue = CommonFunction.reCalcItem(objValue, estModel.ConTaxInputKb, vTax);
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
                decimal wkContax; // 浮動小数点の計算誤差回避のため
                if (estModel.ConTaxInputKb == false)
                    wkContax = (estModel.CarSum + estSubModel.TaxInsEquivalentAll + estModel.TaxCostAll) * vTax;
                else
                {
                    wkContax = (estModel.CarSum + estSubModel.TaxInsEquivalentAll + estModel.TaxCostAll) / (decimal)(1 + vTax);
                    wkContax = Math.Ceiling(wkContax);
                    wkContax = wkContax * vTax;
                }

                estModel.ConTax = Convert.ToInt32(Math.Floor(wkContax));

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

                // update [t_Estimate]
                _unitOfWork.Estimates.UpdateEstCalSum(estModel);

                // update [t_EstimateSub]
                _unitOfWork.EstimateSubs.UpdateEstSubCalSum(estSubModel);

                string strClearMsg = "";

                if ((oldSalesSum > 0) && (estModel.SalesSum != oldSalesSum) && (estModel.PayTimes > 0))
                {
                    // 総額変更ありの場合
                    if (estSubModel.LoanRecalcSettingFlag)
                    {
                        // ローンの自動再計算
                        CommonSimLon simLon = new CommonSimLon(_logger);

                        simLon.SaleSumPrice = estModel.SalesSum;
                        simLon.Deposit = estModel.Deposit;
                        simLon.MoneyRate = estModel.Rate;
                        simLon.PayTimes = estModel.PayTimes;
                        simLon.FirstMonth = Convert.ToInt32(Strings.Right(estModel.FirstPayMonth, 2));

                        if (estModel.BonusAmount > 0)
                        {
                            simLon.Bonus = estModel.BonusAmount;
                            simLon.BonusFirst = int.Parse(estModel.BonusFirst);
                            simLon.BonusSecond = int.Parse(estModel.BonusSecond);
                        }
                        else
                        {
                            simLon.Bonus = 0;
                            simLon.BonusFirst = 0;
                            simLon.BonusSecond = 0;
                        }
                        simLon.ConTax = vTax;

                        // 計算実行
                        if (simLon.calcRegLoan() == false)
                        {
                            strClearMsg = CommonConst.def_LoanInfo_Error.ToString();
                        }
                        else
                        {
                            TEstimate estimate = _mapper.Map<TEstimate>(estModel);
                            estimate.Rate = (double)simLon.MoneyRate;
                            estimate.Deposit = simLon.Deposit;
                            estimate.Principal = simLon.Principal;
                            estimate.PartitionFee = simLon.Fee;
                            estimate.PartitionAmount = simLon.PayTotal;
                            estimate.FirstPayMonth = simLon.FirstPayMonth.ToString();
                            estimate.LastPayMonth = simLon.LastPayMonth.ToString();
                            estimate.FirstPayAmount = simLon.FirstPay;
                            estimate.PayAmount = simLon.PayMonth;
                            estimate.BonusAmount = simLon.Bonus;
                            estimate.BonusFirst = simLon.BonusFirst.ToString();
                            estimate.BonusSecond = simLon.BonusSecond.ToString();
                            estimate.BonusTimes = simLon.BonusTimes;
                            estimate.PayTimes = simLon.PayTimes;

                            _unitOfWork.Estimates.Update(estimate);

                            TEstimateSub estimateSub = _mapper.Map<TEstimateSub>(estSubModel);
                            estimateSub.LoanModifyFlag = false;
                            estimateSub.LoanRecalcSettingFlag = true;
                            estimateSub.LoanInfo = CommonConst.def_LoanInfo_NormalEnd;

                            _unitOfWork.EstimateSubs.Update(estimateSub);
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
                    TEstimateSub estimateSub = _mapper.Map<TEstimateSub>(estSubModel);
                    estimateSub.LoanInfo = CommonConst.def_LoanInfo_Unexecuted;
                    _unitOfWork.EstimateSubs.Update(estimateSub);
                }


                if (strClearMsg != "")
                {
                    // ローンの再計算失敗、または総額変更に伴うローンの自動再計算を行わない場合、ローン情報クリア
                    TEstimate estimate = _mapper.Map<TEstimate>(estModel);
                    estimate.Rate = 0;
                    estimate.Deposit = 0;
                    estimate.Principal = estModel.SalesSum;
                    estimate.PartitionFee = 0;
                    estimate.PartitionAmount = 0;
                    estimate.FirstPayMonth = "NULL";
                    estimate.LastPayMonth = "NULL";
                    estimate.FirstPayAmount = 0;
                    estimate.PayAmount = 0;
                    estimate.BonusAmount = 0;
                    estimate.BonusFirst = "NULL";
                    estimate.BonusSecond = "NULL";
                    estimate.BonusTimes = 0;
                    estimate.PayTimes = 0;

                    _unitOfWork.Estimates.Update(estimate);

                    TEstimateSub estimateSub = _mapper.Map<TEstimateSub>(estSubModel);
                    estimateSub.LoanModifyFlag = false;
                    estimateSub.LoanRecalcSettingFlag = true;
                    estimateSub.LoanInfo = Convert.ToByte(strClearMsg);

                    _unitOfWork.EstimateSubs.Update(estimateSub);
                }

                _unitOfWork.CommitAsync();
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
        public bool getEstData(string inEstNo, string inEstSubNo)
        {
            try
            {
                var estModel = _mapper.Map<EstimateModel>(_unitOfWork.Estimates.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false));

                CommonFunction.chkImgFile(estModel.CarImgPath, valToken.sesCarImgPath, "D:/asest2/CarImg/CarImgDummy.jpg");
                CommonFunction.chkImgFile(estModel.CarImgPath1, valToken.sesCarImgPath1, "");
                CommonFunction.chkImgFile(estModel.CarImgPath2, valToken.sesCarImgPath2, "");
                CommonFunction.chkImgFile(estModel.CarImgPath3, valToken.sesCarImgPath3, "");
                CommonFunction.chkImgFile(estModel.CarImgPath4, valToken.sesCarImgPath4, "");
                CommonFunction.chkImgFile(estModel.CarImgPath5, valToken.sesCarImgPath5, "");
                CommonFunction.chkImgFile(estModel.CarImgPath6, valToken.sesCarImgPath6, "");
                CommonFunction.chkImgFile(estModel.CarImgPath7, valToken.sesCarImgPath7, "");
                CommonFunction.chkImgFile(estModel.CarImgPath8, valToken.sesCarImgPath8, "");

                var estSubModel = _mapper.Map<EstimateSubModel>(_unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false));

                // その他費用の対応前のデータの場合
                if (estSubModel.Sonota == 0 && estSubModel.RakuSatu + estSubModel.Rikusou > 0)
                {
                    estSubModel.Sonota = estSubModel.RakuSatu + estSubModel.Rikusou;
                    estModel.CarPrice = estModel.CarPrice - estSubModel.Sonota;
                }
                if (estSubModel.SonotaTitle == "")
                    estSubModel.SonotaTitle = CommonConst.def_TitleSonota;

                // 取得データを表示用に整形
                creDispData(estModel, estSubModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getEstData - CEST-040D");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 見積書データ表示用整形
        /// </summary>
        public void creDispData(EstimateModel est, EstimateSubModel sub)
        {
            int intCornerType = 0;
            int logline = 0;

            // 見積番号
            EstimateModelView.dspEstNo = est.EstNo + "-" + est.EstSubNo;
            // 見積日
            DateTime edate = (DateTime)est.TradeDate;
            EstimateModelView.dspTradeDate = Strings.Format(edate, "D");
            EstimateModelView.csvTradeDate = edate.ToString("yyyyMMdd"); //EstimateModelView.csv用
                                                                         // 車名
            EstimateModelView.dspCarName = est.MakerName + " " + est.ModelName;
            EstimateModelView.csvMakerName = est.MakerName; //EstimateModelView.csv用
            EstimateModelView.csvModelName = est.ModelName; //EstimateModelView.csv用
                                                            // グレード
            EstimateModelView.dspGradeName = est.GradeName;
            // 型式
            EstimateModelView.dspCase = est.Case;
            // 車台番号
            EstimateModelView.dspChassisNo = est.ChassisNo;
            // 初年度登録
            if (Strings.Len(Strings.Trim(est.FirstRegYm)) == 4)
            {
                EstimateModelView.dspFirstRegYm = CommonFunction.getWareki(Strings.Mid(est.FirstRegYm, 1, 4)) + "年";
                EstimateModelView.csvFirstRegYm = Strings.Mid(est.FirstRegYm, 1, 4); //EstimateModelView.csv用
            }
            else if (Strings.Len(Strings.Trim(est.FirstRegYm)) == 6)
            {
                EstimateModelView.dspFirstRegYm = CommonFunction.getWareki(Strings.Mid(est.FirstRegYm, 1, 4)) + "年" + System.Convert.ToInt32(Strings.Mid(est.FirstRegYm, 5, 2)) + "月";
                EstimateModelView.csvFirstRegYm = est.FirstRegYm; //EstimateModelView.csv用
            }
            else
            {
                EstimateModelView.dspFirstRegYm = "";
                EstimateModelView.csvFirstRegYm = ""; //EstimateModelView.csv用
            }
            // 車検
            if (est.CheckCarYm == "無し" || est.CheckCarYm == "")
            {
                EstimateModelView.dspCheckCarYm = est.CheckCarYm;
                EstimateModelView.csvCheckCarYm = est.CheckCarYm; //EstimateModelView.csv用
            }
            else
            {
                string Ysc = "";
                // 年のみ("YYYY")のデータは、トップ画面および見積書/注文書上で、敢えて「xxxx年  月」のままとする（仕様確認済）
                string Msc = "";
                CommonFunction.FormatDay(est.CheckCarYm, Ysc, Msc);
                EstimateModelView.dspCheckCarYm = CommonFunction.getWareki(Ysc) + "年" + Msc + "月";
                EstimateModelView.csvCheckCarYm = est.CheckCarYm; //EstimateModelView.csv用

                sub.DamageInsEquivalent = 0;
                est.DamageIns = 0;
            }
            // 走行距離
            EstimateModelView.dspNowOdometer = est.NowOdometer == 0 ? "" : est.NowOdometer.ToString();
            EstimateModelView.dspMilUnit = sub.MilUnit;
            // 修復歴
            switch (est.AccidentHis.ToString())
            {
                case "1":
                    EstimateModelView.dspAccidentHis = "有り";
                    break;
                case "0":
                    EstimateModelView.dspAccidentHis = "無し";
                    break;
                case "2":
                    EstimateModelView.dspAccidentHis = "";
                    break;
            }

            // 車歴
            EstimateModelView.dspBusinessHis = est.BusinessHis;
            // シフト
            EstimateModelView.dspMission = est.Mission;
            // 排気量
            EstimateModelView.dspDispVol = Strings.Trim(Strings.Replace(est.DispVol, "cc", "")); // 過去データで "cc" が入っていた場合のガード
            EstimateModelView.dspDispVol = int.Parse(EstimateModelView.dspDispVol) == 0 ? "" : EstimateModelView.dspDispVol;
            EstimateModelView.dspDispVolUnit = sub.DispVolUnit;
            // 色
            EstimateModelView.dspBodyColor = est.BodyColor;
            // 装備
            EstimateModelView.dspEquipment = est.Equipment;
            // 車両画像
            if (est.CarImgPath != "")
                EstimateModelView.dspCarImgPath = est.CarImgPath;
            // 車両画像サブ
            if (est.CarImgPath1 != "")
                EstimateModelView.dspCarImgPath1 = est.CarImgPath1;
            if (est.CarImgPath2 != "")
                EstimateModelView.dspCarImgPath2 = est.CarImgPath2;
            if (est.CarImgPath3 != "")
                EstimateModelView.dspCarImgPath3 = est.CarImgPath3;
            if (est.CarImgPath4 != "")
                EstimateModelView.dspCarImgPath4 = est.CarImgPath4;
            if (est.CarImgPath5 != "")
                EstimateModelView.dspCarImgPath5 = est.CarImgPath5;
            if (est.CarImgPath6 != "")
                EstimateModelView.dspCarImgPath6 = est.CarImgPath6;
            if (est.CarImgPath7 != "")
                EstimateModelView.dspCarImgPath7 = est.CarImgPath7;
            if (est.CarImgPath8 != "")
                EstimateModelView.dspCarImgPath8 = est.CarImgPath8;
            // 下取車
            EstimateModelView.dspTradeInCarName = est.TradeInCarName;

            if (Strings.Len(Strings.Trim(est.TradeInFirstRegYm)) == 4)
            {
                EstimateModelView.dspTradeInFirstRegYm = CommonFunction.getWareki(Strings.Mid(est.TradeInFirstRegYm, 1, 4)) + "年";
                EstimateModelView.csvTradeInFirstRegYm = Strings.Mid(est.TradeInFirstRegYm, 1, 4); //EstimateModelView.csv用
            }
            else if (Strings.Len(Strings.Trim(est.TradeInFirstRegYm)) == 6)
            {
                EstimateModelView.dspTradeInFirstRegYm = CommonFunction.getWareki(Strings.Mid(est.TradeInFirstRegYm, 1, 4)) + "年" + System.Convert.ToInt32(Strings.Mid(est.TradeInFirstRegYm, 5, 2)) + "月";
                EstimateModelView.csvTradeInFirstRegYm = est.TradeInFirstRegYm; //EstimateModelView.csv用
            }
            else
            {
                EstimateModelView.dspTradeInFirstRegYm = "";
                EstimateModelView.csvTradeInFirstRegYm = ""; //EstimateModelView.csv用
            }

            if (est.TradeInCheckCarYm == "無し" || est.TradeInCheckCarYm == "")
            {
                EstimateModelView.dspTradeInCheckCarYm = est.TradeInCheckCarYm;
                EstimateModelView.csvTradeInCheckCarYm = est.TradeInCheckCarYm; //EstimateModelView.csv用
            }
            else
            {
                string Ysc = "";
                string Msc = "";
                CommonFunction.FormatDay(est.TradeInCheckCarYm, Ysc, Msc);
                // 年のみ("YYYY")のデータは、トップ画面および見積書/注文書上で、敢えて「xxxx年  月」のままとする（仕様確認済）
                EstimateModelView.dspTradeInCheckCarYm = CommonFunction.getWareki(Ysc) + "年" + Msc + "月";
                EstimateModelView.csvTradeInCheckCarYm = est.TradeInCheckCarYm; //EstimateModelView.csv用
            }

            EstimateModelView.dspTradeInNowOdometer = est.TradeInNowOdometer == 0 ? "" : est.TradeInNowOdometer.ToString();
            EstimateModelView.dspTradeInMilUnit = sub.TradeInMilUnit;
            EstimateModelView.dspTradeInChassisNo = est.TradeInChassisNo;
            EstimateModelView.dspTradeInRegNo = est.TradeInRegNo.Replace("/", "");
            EstimateModelView.dspTradeInBodyColor = est.TradeInBodyColor;
            EstimateModelView.dspCarPrice = (est.CarPrice == 0 ? "" : CommonFunction.setFormat(est.CarPrice, " 円", ""));

            // 整備費用
            long wSyakenNew;
            if (est.SyakenNew > 0 & est.SyakenZok == 0)
            {
                wSyakenNew = est.SyakenNew;
                EstimateModelView.dspSyakenNewZokT = CommonConst.def_TitleSyakenNew;
            }
            else if (est.SyakenNew == 0 && est.SyakenZok > 0)
            {
                wSyakenNew = est.SyakenZok;
                EstimateModelView.dspSyakenNewZokT = CommonConst.def_TitleSyakenZok;
            }
            else
            {
                wSyakenNew = 0;
                EstimateModelView.dspSyakenNewZokT = CommonConst.def_TitleSyakenNew;
                if ((Strings.Len(est.CheckCarYm) == 6 &&
                    DateAndTime.DateDiff(DateInterval.Month, DateTime.Today, DateTime.Parse(Strings.Left(est.CheckCarYm, 4) + "/" + Strings.Right(est.CheckCarYm, 2) + "/01")) > 0)
                    || (Strings.Len(est.CheckCarYm) == 4 && DateAndTime.DateDiff(DateInterval.Year, DateTime.Today, DateTime.Parse(est.CheckCarYm + "/01")) > 0))
                    EstimateModelView.dspSyakenNewZokT = CommonConst.def_TitleSyakenZok;
            }
            EstimateModelView.dspSyakenNew = (wSyakenNew == 0 ? "" : CommonFunction.setFormat(wSyakenNew, " 円", ""));

            // 車両計
            // Dim wCarSum As Long = wCarPrice + wSyakenNew
            EstimateModelView.dspCarSum = (est.CarSum == 0 ? "" : CommonFunction.setFormat(est.CarSum, " 円", ""));
            // Call CreateLog(CarSum & ":" &EstimateModelView.dspCarSum, "clsEstimate")
            // その他費用
            EstimateModelView.dspSonotaTitle = sub.SonotaTitle;
            EstimateModelView.dspSonota = (sub.Sonota == 0 ? "" : CommonFunction.setFormat(sub.Sonota, " 円", ""));
            // 落札料
            EstimateModelView.csvRakuSatu = sub.RakuSatu.ToString(); //EstimateModelView.csv用
                                                                     // 陸送代
            EstimateModelView.csvRikusou = sub.Rikusou.ToString(); //EstimateModelView.csv用

            // 値引
            if (est.Discount == 0)
            {
                EstimateModelView.dspDiscountT = "";
                EstimateModelView.dspDiscount = "";
            }
            else
            {
                EstimateModelView.dspDiscountT = CommonConst.def_TitleDisCount;
                EstimateModelView.dspDiscount = "▲" + Convert.ToString(CommonFunction.setFormat(est.Discount, " 円", ""));
            }

            // 付属品
            EstimateModelView.dspOptionPriceAll = (est.OptionPriceAll == 0 ? "" : CommonFunction.setFormat(est.OptionPriceAll, " 円", ""));
            // 税金・保険料
            EstimateModelView.dspTaxInsAll = (est.TaxInsAll == 0 ? "" : CommonFunction.setFormat(est.TaxInsAll, " 円", ""));
            // 税金・保険料相当額
            EstimateModelView.dspTaxInsEquivalentAll = (sub.TaxInsEquivalentAll == 0 ? "" : CommonFunction.setFormat(sub.TaxInsEquivalentAll, " 円", ""));
            // 預り法定費
            EstimateModelView.dspTaxFreeAll = (est.TaxFreeAll == 0 ? "" : CommonFunction.setFormat(est.TaxFreeAll, " 円", ""));
            // 手続代行費用
            EstimateModelView.dspTaxCostAll = (est.TaxCostAll == 0 ? "" : CommonFunction.setFormat(est.TaxCostAll, " 円", ""));
            // 消費税
            EstimateModelView.dspConTax = CommonFunction.setFormat(est.ConTax, " 円", "");
            // 車両販売総額
            EstimateModelView.dspCarSaleSum = CommonFunction.setFormat(est.CarSaleSum, " 円", "");
            // 下取車有無
            EstimateModelView.dspTradeInUM = sub.TradeInUm.ToString();
            // 下取車価格
            EstimateModelView.dspTradeInPrice = (est.TradeInPrice == 0 ? "" : "▲" + Convert.ToString(CommonFunction.setFormat(est.TradeInPrice, " 円", "")));
            // 下取車残債
            if (est.Balance == 0)
            {
                EstimateModelView.dspBalanceT = "下取車残債";
                EstimateModelView.dspBalance = "";
            }
            else
            {
                EstimateModelView.dspBalanceT = "下取車残債";
                EstimateModelView.dspBalance = CommonFunction.setFormat(est.Balance, " 円", "");
            }
            // 合計
            EstimateModelView.dspSalesSum = CommonFunction.setFormat(est.SalesSum, " 円", "");
            // 頭金
            EstimateModelView.dspDeposit = (est.Deposit == 0 ? "" : CommonFunction.setFormat(est.Deposit, " 円", ""));
            // 割賦元金
            EstimateModelView.dspPrincipal = CommonFunction.setFormat(est.SalesSum - est.Deposit, " 円", "");

            // ローン内容
            // -- 分割払手数料
            if (est.PartitionFee > 0)
                EstimateModelView.dspPartitionFee = CommonFunction.setFormat((long)est.PartitionFee, " 円", "");
            else
                EstimateModelView.dspPartitionFee = "";

            // -- 分割払金合計
            if (est.PartitionAmount > 0)
                EstimateModelView.dspPartitionAmount = CommonFunction.setFormat(est.PartitionAmount, " 円", "");
            else
                EstimateModelView.dspPartitionAmount = "";

            // -- 支払回数
            if (est.PayTimes > 0)
                EstimateModelView.dspPayTimes = est.PayTimes + " 回";
            else
                EstimateModelView.dspPayTimes = "";

            // -- 支払期間
            string fromdt = "";
            if (est.FirstPayMonth != "")
                fromdt = Strings.Mid(est.FirstPayMonth, 1, 4) + "年" + System.Convert.ToString(Strings.Mid(est.FirstPayMonth, 5, 2)) + "月";
            string todt = "";
            if (est.LastPayMonth != "")
                todt = Strings.Mid(est.LastPayMonth, 1, 4) + "年" + System.Convert.ToString(Strings.Mid(est.LastPayMonth, 5, 2)) + "月";
            if (fromdt != "" | todt != "")
            {
                EstimateModelView.dspKikan = fromdt + " - " + todt;
                EstimateModelView.csvFirstPayMonth = est.FirstPayMonth; //EstimateModelView.csv用
                EstimateModelView.csvLastPayMonth = est.LastPayMonth; //EstimateModelView.csv用
            }
            else
            {
                EstimateModelView.dspKikan = "";
                EstimateModelView.csvFirstPayMonth = ""; //EstimateModelView.csv用
                EstimateModelView.csvLastPayMonth = ""; //EstimateModelView.csv用
            }

            // -- 初回支払額
            if (est.FirstPayAmount > 0)
                EstimateModelView.dspFirstPayAmount = CommonFunction.setFormat(est.FirstPayAmount, " 円", "");
            else
                EstimateModelView.dspFirstPayAmount = "";

            // -- 2回目以降支払額
            if (est.PayAmount > 0)
                EstimateModelView.dspPayAmount = CommonFunction.setFormat(est.PayAmount, " 円", "");
            else
                EstimateModelView.dspPayAmount = "";
            if (est.PayTimes > 0)
                EstimateModelView.dspPayTimes2 = "（×" + Convert.ToString(est.PayTimes - 1) + "回）";
            else
                EstimateModelView.dspPayTimes2 = "";

            if (est.BonusAmount > 0)
            {
                // -- ボーナス加算月
                if (est.BonusFirst != "")
                {
                    EstimateModelView.dspBonusMonth = est.BonusFirst + "月";
                    EstimateModelView.csvBonusFirst = est.BonusFirst; //EstimateModelView.csv用
                }
                else
                {
                    EstimateModelView.dspBonusMonth = "";
                    EstimateModelView.csvBonusFirst = ""; //EstimateModelView.csv用
                }
                if (est.BonusSecond != "")
                {
                    EstimateModelView.dspBonusMonth += "・" + est.BonusSecond + "月";
                    EstimateModelView.csvBonusSecond = est.BonusSecond; //EstimateModelView.csv用
                }
                else
                {
                    EstimateModelView.dspBonusMonth += "";
                    EstimateModelView.csvBonusSecond = ""; //EstimateModelView.csv用
                }

                // -- ボーナス加算額
                EstimateModelView.dspBonusAmount = CommonFunction.setFormat(est.BonusAmount, " 円", "0");

                // -- ボーナス加算回数
                if (est.BonusTimes > 0)
                    EstimateModelView.dspBonusTimes = "（×" + est.BonusTimes + "回）";
                else
                    EstimateModelView.dspBonusTimes = "";
            }
            else
            {
                // -- ボーナス加算月
                EstimateModelView.dspBonusMonth = "";
                EstimateModelView.csvBonusFirst = ""; //EstimateModelView.csv用
                EstimateModelView.csvBonusSecond = ""; //EstimateModelView.csv用

                // -- ボーナス加算額
                EstimateModelView.dspBonusAmount = "";

                // -- ボーナス加算回数
                EstimateModelView.dspBonusTimes = "";
            }

            // -- 実質年率
            if (est.Rate > 0)
            {
                EstimateModelView.dspRate = "分割払手数料は実質年率 " + est.Rate + "% で計算しています";
                EstimateModelView.csvRate = est.Rate.ToString(); //EstimateModelView.csv用
            }
            else
            {
                EstimateModelView.dspRate = "";
                EstimateModelView.csvRate = ""; //EstimateModelView.csv用
            }

            // 付属品明細
            if (est.OptionInputKb == true)
            {
                EstimateModelView.dspOptionName1 = est.OptionName1;
                EstimateModelView.dspOptionPrice1 = (int.Parse(est.OptionName1) == 0 ? "" : CommonFunction.setFormat(int.Parse(est.OptionName1), " 円", ""));
                EstimateModelView.dspOptionName2 = est.OptionName2;
                EstimateModelView.dspOptionPrice2 = (int.Parse(est.OptionName2) == 0 ? "" : CommonFunction.setFormat(int.Parse(est.OptionName2), " 円", ""));
                EstimateModelView.dspOptionName3 = est.OptionName3;
                EstimateModelView.dspOptionPrice3 = (int.Parse(est.OptionName3) == 0 ? "" : CommonFunction.setFormat(int.Parse(est.OptionName3), " 円", ""));
                EstimateModelView.dspOptionName4 = est.OptionName4;
                EstimateModelView.dspOptionPrice4 = (int.Parse(est.OptionName4) == 0 ? "" : CommonFunction.setFormat(int.Parse(est.OptionName4), " 円", ""));
                EstimateModelView.dspOptionName5 = est.OptionName5;
                EstimateModelView.dspOptionPrice5 = (int.Parse(est.OptionName5) == 0 ? "" : CommonFunction.setFormat(int.Parse(est.OptionName5), " 円", ""));
                EstimateModelView.dspOptionName6 = est.OptionName6;
                EstimateModelView.dspOptionPrice6 = (int.Parse(est.OptionName6) == 0 ? "" : CommonFunction.setFormat(int.Parse(est.OptionName6), " 円", ""));
                EstimateModelView.dspOptionName7 = est.OptionName7;
                EstimateModelView.dspOptionPrice7 = (int.Parse(est.OptionName7) == 0 ? "" : CommonFunction.setFormat(int.Parse(est.OptionName7), " 円", ""));
                EstimateModelView.dspOptionName8 = est.OptionName8;
                EstimateModelView.dspOptionPrice8 = (int.Parse(est.OptionName8) == 0 ? "" : CommonFunction.setFormat(int.Parse(est.OptionName8), " 円", ""));
                EstimateModelView.dspOptionName9 = est.OptionName9;
                EstimateModelView.dspOptionPrice9 = (int.Parse(est.OptionName9) == 0 ? "" : CommonFunction.setFormat(int.Parse(est.OptionName9), " 円", ""));
                EstimateModelView.dspOptionName10 = est.OptionName10;
                EstimateModelView.dspOptionPrice10 = (int.Parse(est.OptionName10) == 0 ? "" : CommonFunction.setFormat(int.Parse(est.OptionName10), " 円", ""));
                EstimateModelView.dspOptionName11 = est.OptionName11;
                EstimateModelView.dspOptionPrice11 = (int.Parse(est.OptionName11) == 0 ? "" : CommonFunction.setFormat(int.Parse(est.OptionName11), " 円", ""));
                EstimateModelView.dspOptionName12 = est.OptionName12;
                EstimateModelView.dspOptionPrice12 = (int.Parse(est.OptionName12) == 0 ? "" : CommonFunction.setFormat(int.Parse(est.OptionName12), " 円", ""));
            }
            // 税金・保険料明細
            if (est.TaxInsInputKb == true)
            {
                EstimateModelView.dspAutoTax = (est.AutoTax == 0 ? "" : CommonFunction.setFormat(est.AutoTax, " 円", ""));
                EstimateModelView.dspAutoTaxEquivalent = (sub.AutoTaxEquivalent == 0 ? "" : CommonFunction.setFormat(sub.AutoTaxEquivalent, " 円", ""));
                if (sub.AutoTaxEquivalent > 0)
                    EstimateModelView.dspAutoTaxMonth = CommonConst.def_TitleAutoTaxEquivalent;
                else
                    EstimateModelView.dspAutoTaxMonth = CommonConst.def_TitleAutoTax + (est.AutoTax == 0 ? "" : "（" + sub.AutoTaxMonth + "月中登録）");
                EstimateModelView.csvAutoTaxMonth = sub.AutoTaxMonth; //EstimateModelView.csv用
                EstimateModelView.dspAcqTax = (est.AcqTax == 0 ? "" : CommonFunction.setFormat(est.AcqTax, " 円", ""));
                EstimateModelView.dspWeightTax = (est.WeightTax == 0 ? "" : CommonFunction.setFormat(est.WeightTax, " 円", ""));
                EstimateModelView.dspDamageIns = (est.DamageIns == 0 ? "" : CommonFunction.setFormat(est.DamageIns, " 円", ""));
                EstimateModelView.dspDamageInsEquivalent = (sub.DamageInsEquivalent == 0 ? "" : CommonFunction.setFormat(sub.DamageInsEquivalent, " 円", ""));

                if (sub.DamageInsEquivalent > 0)
                    EstimateModelView.dspDamageInsMonth = CommonConst.def_TitleDamageInsEquivalent;
                else
                    EstimateModelView.dspDamageInsMonth = CommonConst.def_TitleDamageIns + (est.DamageIns == 0 ? "" : "（" + sub.DamageInsMonth + "ヶ月）");
                EstimateModelView.csvDamageInsMonth = sub.DamageInsMonth; //EstimateModelView.csv用
                EstimateModelView.dspOptionIns = (est.OptionIns == 0 ? "" : CommonFunction.setFormat(est.OptionIns, " 円", ""));
            }
            // 預り法定費明細
            if (est.TaxFreeKb == true)
            {
                EstimateModelView.dspTaxFreeGarage = (est.TaxFreeGarage == 0 ? "" : CommonFunction.setFormat(est.TaxFreeGarage, " 円", ""));
                EstimateModelView.dspTaxFreeCheck = (est.TaxFreeCheck == 0 ? "" : CommonFunction.setFormat(est.TaxFreeCheck, " 円", ""));
                EstimateModelView.dspTaxFreeTradeIn = (est.TaxFreeTradeIn == 0 ? "" : CommonFunction.setFormat(est.TaxFreeTradeIn, " 円", ""));
                EstimateModelView.dspTaxFreeRecycle = (est.TaxFreeRecycle == 0 ? "" : CommonFunction.setFormat(est.TaxFreeRecycle, " 円", ""));
                EstimateModelView.dspTaxFreeOther = (est.TaxFreeOther == 0 ? "" : CommonFunction.setFormat(est.TaxFreeOther, " 円", ""));
                EstimateModelView.dspTaxFreeSet1Title = sub.TaxFreeSet1Title;
                EstimateModelView.dspTaxFreeSet1 = (sub.TaxFreeSet1 == 0 ? "" : CommonFunction.setFormat(sub.TaxFreeSet1, " 円", ""));
                EstimateModelView.dspTaxFreeSet2Title = sub.TaxFreeSet2Title;
                EstimateModelView.dspTaxFreeSet2 = (sub.TaxFreeSet2 == 0 ? "" : CommonFunction.setFormat(sub.TaxFreeSet2, " 円", ""));
            }
            // 手続代行費明細
            if (est.TaxCostKb == true)
            {
                EstimateModelView.dspTaxGarage = (est.TaxGarage == 0 ? "" : CommonFunction.setFormat(est.TaxGarage, " 円", ""));
                EstimateModelView.dspTaxCheck = (est.TaxCheck == 0 ? "" : CommonFunction.setFormat(est.TaxCheck, " 円", ""));
                EstimateModelView.dspTaxTradeIn = (est.TaxTradeIn == 0 ? "" : CommonFunction.setFormat(est.TaxTradeIn, " 円", ""));
                EstimateModelView.dspTaxDelivery = (est.TaxDelivery == 0 ? "" : CommonFunction.setFormat(est.TaxDelivery, " 円", ""));
                EstimateModelView.dspTaxRecycle = (est.TaxRecycle == 0 ? "" : CommonFunction.setFormat(est.TaxRecycle, " 円", ""));
                EstimateModelView.dspTaxOther = (est.TaxOther == 0 ? "" : CommonFunction.setFormat(est.TaxOther, " 円", ""));

                EstimateModelView.dspTaxTradeInSatei = (sub.TaxTradeInSatei == 0 ? "" : CommonFunction.setFormat(sub.TaxTradeInSatei, " 円", ""));
                EstimateModelView.dspTaxSet1Title = sub.TaxSet1Title;
                EstimateModelView.dspTaxSet1 = (sub.TaxSet1 == 0 ? "" : CommonFunction.setFormat(sub.TaxSet1, " 円", ""));
                EstimateModelView.dspTaxSet2Title = sub.TaxSet2Title;
                EstimateModelView.dspTaxSet2 = (sub.TaxSet2 == 0 ? "" : CommonFunction.setFormat(sub.TaxSet2, " 円", ""));
                EstimateModelView.dspTaxSet3Title = sub.TaxSet3Title;
                EstimateModelView.dspTaxSet3 = (sub.TaxSet3 == 0 ? "" : CommonFunction.setFormat(sub.TaxSet3, " 円", ""));
            }
            // 販売店
            EstimateModelView.dspShopNm = est.ShopNm;
            EstimateModelView.dspShopAdr = est.ShopAdr;

            EstimateModelView.dspEstTanName = "";
            EstimateModelView.dspSekininName = "";
            EstimateModelView.dspShopTel = "";

            if (Strings.Trim(est.EstTanName) != "")
                EstimateModelView.dspEstTanName += "担当 : " + est.EstTanName + "　　";

            if (Strings.Trim(est.SekininName) != "")
                EstimateModelView.dspSekininName += "責任者 : " + est.SekininName + "　　";

            EstimateModelView.dspShopTel += "TEL : " + est.ShopTel;

            EstimateModelView.csvShopTel = est.ShopTel; //EstimateModelView.csv用
            EstimateModelView.csvShopTanName = est.EstTanName; //EstimateModelView.csv用

            EstimateModelView.csvSekininName = est.SekininName;    //EstimateModelView.csv用
                                                                   // 顧客
            EstimateModelView.dspCustKName = est.CustKname;
            EstimateModelView.dspCustMemo = sub.CustMemo;
            // AA情報
            if (sub.Aano != "")
            {
                // 開催数追加対応 2015/01/16 Start
                if (sub.Mode == 1)
                {
                    _commonFuncHelper.GetCornerType(sub.Corner, intCornerType);
                    if (intCornerType == 1)
                        EstimateModelView.dspAAInfo = string.Format("お問合せ番号{0}00-{1:00000}-{2:00000}", intCornerType, sub.Aacount, Convert.ToInt32(sub.Aano));
                    else
                        EstimateModelView.dspAAInfo = string.Format("お問合せ番号{0}00-{1}{2}", intCornerType, sub.Corner, sub.Aano);
                }
                else
                    EstimateModelView.dspAAInfo = sub.Aaplace + "　No：" + sub.Aano;
            }

            // 備考
            EstimateModelView.dspNotes = sub.Notes;
            // LeaseFlag
            EstimateModelView.dspLeaseFlag = est.LeaseFlag;
            // タイトル部 項目名（税抜／税込）
            if (est.ConTaxInputKb == true)
            {
                EstimateModelView.dspCarPriceTitle = CommonConst.def_TitleCarPrice + CommonConst.def_TitleInTax;
                if (EstimateModelView.dspDiscountT != "")
                    EstimateModelView.dspDiscountT = EstimateModelView.dspDiscountT + CommonConst.def_TitleInTax;
                EstimateModelView.dspSonotaTitle = EstimateModelView.dspSonotaTitle + CommonConst.def_TitleInTax;
                EstimateModelView.dspSyakenNewZokTc = EstimateModelView.dspSyakenNewZokT;
                EstimateModelView.dspSyakenNewZokT = EstimateModelView.dspSyakenNewZokT + CommonConst.def_TitleInTax;
                EstimateModelView.dspOpSpeCialTitle = CommonConst.def_TitleOpSpeCial + CommonConst.def_TitleInTax;
                EstimateModelView.dspTaxInsEquivalentTitle = CommonConst.def_TitleTaxInsEquivalent + CommonConst.def_TitleInTax;
                EstimateModelView.dspDaikoTitle = CommonConst.def_TitleDaiko + CommonConst.def_TitleInTax;
                EstimateModelView.dspTaxName = CommonConst.def_TitleConTaxTotalInTax;
                EstimateModelView.dspCarSaleSumTitle = CommonConst.def_TitleCarKeiInTax;
                EstimateModelView.dspSalesSumTitle = CommonConst.def_TitleSalesSumInTax;
            }
            else
            {
                EstimateModelView.dspCarPriceTitle = CommonConst.def_TitleCarPrice + CommonConst.def_TitleOutTax;
                if (EstimateModelView.dspDiscountT != "")
                    EstimateModelView.dspDiscountT = EstimateModelView.dspDiscountT + CommonConst.def_TitleOutTax;
                EstimateModelView.dspSonotaTitle = EstimateModelView.dspSonotaTitle + CommonConst.def_TitleOutTax;
                EstimateModelView.dspSyakenNewZokTc = EstimateModelView.dspSyakenNewZokT;
                EstimateModelView.dspSyakenNewZokT = EstimateModelView.dspSyakenNewZokT + CommonConst.def_TitleOutTax;
                EstimateModelView.dspOpSpeCialTitle = CommonConst.def_TitleOpSpeCial + CommonConst.def_TitleOutTax;
                EstimateModelView.dspTaxInsEquivalentTitle = CommonConst.def_TitleTaxInsEquivalent + CommonConst.def_TitleOutTax;
                EstimateModelView.dspDaikoTitle = CommonConst.def_TitleDaiko + CommonConst.def_TitleOutTax;
                EstimateModelView.dspTaxName = CommonConst.def_TitleConTaxTotalOutTax;
                EstimateModelView.dspCarSaleSumTitle = CommonConst.def_TitleCarKeiOutTax;
                EstimateModelView.dspSalesSumTitle = CommonConst.def_TitleSalesSumOutTax;
            }
        }

    }
}
