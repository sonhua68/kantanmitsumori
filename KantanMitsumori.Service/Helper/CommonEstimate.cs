using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
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
        public EstimateModelView EstimateModelView { get; set; }

        public ResponEstMainModel EstMainModel { get; set; }

        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfWorkIDE _unitOfWorkIDE;
        private readonly IMapper _mapper;

        private LogToken valToken;
        private CommonFuncHelper _commonFuncHelper;
        private List<string> reCalEstModel;
        private List<string> reCalEstSubModel;

        public CommonEstimate(ILogger<CommonEstimate> logger, IUnitOfWork unitOfWork, IUnitOfWorkIDE unitOfWorkIDE, IMapper mapper, CommonFuncHelper commonFuncHelper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _unitOfWorkIDE = unitOfWorkIDE;
            _mapper = mapper;
            _commonFuncHelper = commonFuncHelper;

            valToken = new LogToken();
            reCalEstModel = new List<string>();
            reCalEstSubModel = new List<string>();

            EstimateModelView = new EstimateModelView();
        }

        /// <summary>
        /// 出品No等が同一の見積書が存在するか否かチェック
        /// 存在 = True　無し = False
        /// </summary>
        /// <param name="userNo"></param>
        /// <param name="AANo"></param>
        /// <param name="AAPlace"></param>
        /// <param name="CornerType"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool chkAANo(string? userNo, string AANo, string AAPlace, int CornerType, int mode)
        {
            try
            {
                //var getSys = _unitOfWork.Syss.GetList(t => t.CornerType == CornerType).Select(s => new { Corner = s.Corner, Aacount = s.Aacount }).ToList();

                //var getMaxEstSub = _unitOfWork.EstimateSubs.GetList(s => s.EstUserNo == userNo &&
                //                                                    s.Aano == AANo &&
                //                                                    s.Aaplace == AAPlace &&
                //                                                    s.Mode == mode &&
                //                                                    s.Dflag == false &&
                //                                                    getSys.Any(m => m.Corner == s.Corner) &&
                //                                                    getSys.Any(m => m.Aacount == s.Aacount)
                //                                                    ).Max(a => new { maxEstNo = a.EstNo, maxEstSubNo = a.EstSubNo });


                var getMaxEstSub = (from sub in _unitOfWork.DbContext.TEstimateSubs
                                    join sys in _unitOfWork.DbContext.TbSys
                                    on new { sub.Corner, sub.Aacount } equals
                                       new { sys.Corner, sys.Aacount }
                                    into x
                                    from sys in x.DefaultIfEmpty()
                                    where sub.EstUserNo == userNo &&
                                           sub.Aano == AANo &&
                                           sub.Aaplace == AAPlace &&
                                           sys.CornerType == CornerType &&
                                           sub.Mode == mode &&
                                           sub.Dflag == false
                                    group sub by sub.EstNo into g
                                    select new
                                    {
                                        maxEstNo = g.Max(x => x.EstNo),
                                        maxEstSubNo = g.Max(x => x.EstSubNo),
                                    }).FirstOrDefault();

                if (getMaxEstSub != null)
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

        /// <summary>
        /// 見積新枝番データ作成
        /// </summary>
        /// <param name="flgRecreate"></param>
        /// <returns></returns>
        public bool addEstNextSubNo(LogToken logToken, bool flgRecreate = false)
        {
            try
            {
                valToken = logToken;

                // 見積書番号を取得
                string vEstNo = !string.IsNullOrEmpty(valToken.sesEstNo) ? valToken.sesEstNo : "";
                string vLeaseFlag = !string.IsNullOrEmpty(valToken.sesLeaseFlag) ? valToken.sesLeaseFlag : "";
                string vEstSubNo = !string.IsNullOrEmpty(valToken.sesEstSubNo) ? valToken.sesEstSubNo : "";

                if (vEstNo == "" || vEstSubNo == "")
                {
                    valToken.sesErrMsg = CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "CEST-050S" + CommonConst.def_ErrCodeR;
                    return false;
                }

                // （諸費用設定の最新状態を反映しなければならない場合があるので必要）
                calcSum(vEstNo, vEstSubNo, valToken);

                // 見積書データ取得
                if (!getEstData(vEstNo, vEstSubNo))
                {
                    valToken.sesErrMsg = CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "CEST-051D" + CommonConst.def_ErrCodeR;
                    return false;
                }

                // 再作成の場合
                if (flgRecreate)
                {
                    // 新見積書番号取得
                    vEstNo = "";
                    if (!getEstNoFromDb(ref vEstNo))
                    {
                        return false;
                    }
                }

                // 新枝番取得
                string vNextSubNo = "";
                if (!getEstSubNoFromDb(vEstNo, ref vNextSubNo))
                {
                    return false;
                }
                valToken.sesEstSubNo = vNextSubNo;

                // 見積書登録SQL
                TEstimate entityEst = new TEstimate();
                entityEst = _mapper.Map<TEstimate>(EstMainModel);
                entityEst.EstNo = vEstNo;
                entityEst.EstSubNo = vNextSubNo;
                entityEst.TradeDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
                entityEst.OptionInputKb = true;
                entityEst.TaxInsInputKb = true;
                entityEst.TaxFreeKb = true;
                entityEst.TaxCostKb = true;
                entityEst.Rdate = DateTime.Now;
                entityEst.Udate = DateTime.Now;
                entityEst.Dflag = false;

                _unitOfWork.Estimates.Add(entityEst);

                TEstimateSub entityEstSub = new TEstimateSub();
                entityEstSub = _mapper.Map<TEstimateSub>(EstMainModel);
                entityEstSub.EstNo = vEstNo;
                entityEstSub.EstSubNo = vNextSubNo;
                entityEstSub.Rdate = DateTime.Now;
                entityEstSub.Udate = DateTime.Now;
                entityEstSub.Dflag = false;

                _unitOfWork.EstimateSubs.Add(entityEstSub);

                _unitOfWork.CommitAsync();

                valToken.sesEstNo = vEstNo;
                valToken.sesEstSubNo = vNextSubNo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "addEstNextSubNo " + "CEST-052D");
                return false;
            }

            return true;
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

                if (estModel == null)
                {
                    return false;
                }

                var estSubModel = _unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == estModel.EstNo && x.EstSubNo == estModel.EstSubNo && x.Dflag == false);

                if (estSubModel == null)
                {
                    return false;
                }

                // 再計算前の総額
                long oldSalesSum = (long)estModel.SalesSum;

                // 消費税率取得
                var vTax = _commonFuncHelper.getTax((DateTime)estModel.Udate!, logToken.sesTaxRatio, logToken.sesUserNo);
                valToken.sesTaxRatio = vTax;

                // 会員諸費用設定取得
                var getUserDef = _commonFuncHelper.getUserDefData(logToken.sesUserNo);

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


                            int objValue = (int)propEst.GetValue(estModel);

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


                            int objValue = (int)propEstSub.GetValue(estModel);

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
                decimal wkContax; // 浮動小数点の計算誤差回避のため
                if (estModel.ConTaxInputKb == false)
                    wkContax = (decimal)((estModel.CarSum + estSubModel.TaxInsEquivalentAll + estModel.TaxCostAll) * vTax);
                else
                {
                    wkContax = (decimal)((estModel.CarSum + estSubModel.TaxInsEquivalentAll + estModel.TaxCostAll) / (decimal)(1 + vTax));
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

                string strClearMsg = "";

                if ((oldSalesSum > 0) && (estModel.SalesSum != oldSalesSum) && (estModel.PayTimes > 0))
                {
                    // 総額変更ありの場合
                    if (Convert.ToBoolean(estSubModel.LoanRecalcSettingFlag))
                    {
                        // ローンの自動再計算
                        CommonSimLon simLon = new CommonSimLon(_logger);

                        simLon.SaleSumPrice = Convert.ToInt16(estModel.SalesSum);
                        simLon.Deposit = Convert.ToInt16(estModel.Deposit);
                        simLon.MoneyRate = Convert.ToInt16(estModel.Rate);
                        simLon.PayTimes = Convert.ToInt16(estModel.PayTimes);
                        simLon.FirstMonth = Convert.ToInt32(Strings.Right(estModel.FirstPayMonth, 2));

                        if (estModel.BonusAmount > 0)
                        {
                            simLon.Bonus = Convert.ToInt16(estModel.BonusAmount);
                            simLon.BonusFirst = Convert.ToInt16(estModel.BonusFirst);
                            simLon.BonusSecond = Convert.ToInt16(estModel.BonusSecond);
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


                            //TEstimateSub estimateSub = _mapper.Map<TEstimateSub>(estSubModel);
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
                    // ローンの再計算失敗、または総額変更に伴うローンの自動再計算を行わない場合、ローン情報クリア
                    // update Estimates
                    estModel.Rate = 0;
                    estModel.Deposit = 0;
                    estModel.Principal = estModel.SalesSum;
                    estModel.PartitionFee = 0;
                    estModel.PartitionAmount = 0;
                    estModel.FirstPayMonth = "NULL";
                    estModel.LastPayMonth = "NULL";
                    estModel.FirstPayAmount = 0;
                    estModel.PayAmount = 0;
                    estModel.BonusAmount = 0;
                    estModel.BonusFirst = "NULL";
                    estModel.BonusSecond = "NULL";
                    estModel.BonusTimes = 0;
                    estModel.PayTimes = 0;
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
        public bool getEstData(string inEstNo, string inEstSubNo)
        {
            try
            {
                var estModel = _unitOfWork.Estimates.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false);

                CommonFunction.chkImgFile(estModel.CarImgPath!, valToken.sesCarImgPath, CommonConst.def_DmyImg);
                CommonFunction.chkImgFile(estModel.CarImgPath1!, valToken.sesCarImgPath1, "");
                CommonFunction.chkImgFile(estModel.CarImgPath2!, valToken.sesCarImgPath2, "");
                CommonFunction.chkImgFile(estModel.CarImgPath3!, valToken.sesCarImgPath3, "");
                CommonFunction.chkImgFile(estModel.CarImgPath4!, valToken.sesCarImgPath4, "");
                CommonFunction.chkImgFile(estModel.CarImgPath5!, valToken.sesCarImgPath5, "");
                CommonFunction.chkImgFile(estModel.CarImgPath6!, valToken.sesCarImgPath6, "");
                CommonFunction.chkImgFile(estModel.CarImgPath7!, valToken.sesCarImgPath7, "");
                CommonFunction.chkImgFile(estModel.CarImgPath8!, valToken.sesCarImgPath8, "");

                var estSubModel = _unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false);

                // その他費用の対応前のデータの場合
                if (estSubModel.Sonota == 0 && estSubModel.RakuSatu + estSubModel.Rikusou > 0)
                {
                    estSubModel.Sonota = estSubModel.RakuSatu + estSubModel.Rikusou;
                    estModel.CarPrice = estModel.CarPrice - estSubModel.Sonota;
                }
                if (estSubModel.SonotaTitle == "")
                    estSubModel.SonotaTitle = CommonConst.def_TitleSonota;

                EstMainModel = _mapper.Map<ResponEstMainModel>(estModel);

                EstMainModel = _mapper.Map<ResponEstMainModel>(estSubModel);
                //_unitOfWork.Estimates.Add(estModel);
                //_unitOfWork.EstimateSubs.Add(estSubModel);
                //_unitOfWork.CommitAsync();

                var estModelMap = _mapper.Map<EstimateModel>(estModel);
                var estSubModelMap = _mapper.Map<EstimateSubModel>(estSubModel);

                creDispData(estModelMap, estSubModelMap);
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

            // 見積番号
            EstimateModelView.dspEstNo = est.EstNo + "-" + est.EstSubNo;
            // 見積日
            //DateTime edate = (DateTime)est.TradeDate;
            EstimateModelView.dspTradeDate = CommonFunction.japaneseFormat(est.TradeDate);
            EstimateModelView.csvTradeDate = est.TradeDate.ToString("yyyyMMdd"); //EstimateModelView.csv用
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
            if (est.CheckCarYm == "無し" || string.IsNullOrEmpty(est.CheckCarYm))
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

            if (est.TradeInCheckCarYm == "無し" || string.IsNullOrEmpty(est.TradeInCheckCarYm))
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
            EstimateModelView.dspTradeInRegNo = string.IsNullOrEmpty(est.TradeInRegNo) ? "" : est.TradeInRegNo.Replace("/", "");
            EstimateModelView.dspTradeInBodyColor = est.TradeInBodyColor;
            EstimateModelView.dspCarPrice = CommonFunction.setFormatCurrency(est.CarPrice);

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
            EstimateModelView.dspSyakenNew = CommonFunction.setFormatCurrency(wSyakenNew);

            // 車両計
            // Dim wCarSum As Long = wCarPrice + wSyakenNew
            EstimateModelView.dspCarSum = CommonFunction.setFormatCurrency(est.CarSum);
            // Call CreateLog(CarSum & ":" &EstimateModelView.dspCarSum, "clsEstimate")
            // その他費用
            EstimateModelView.dspSonotaTitle = sub.SonotaTitle;
            EstimateModelView.dspSonota = CommonFunction.setFormatCurrency(sub.Sonota);
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
                EstimateModelView.dspDiscount = "▲" + Convert.ToString(CommonFunction.setFormatCurrency(est.Discount));
            }

            // 付属品
            EstimateModelView.dspOptionPriceAll = CommonFunction.setFormatCurrency(est.OptionPriceAll);
            // 税金・保険料
            EstimateModelView.dspTaxInsAll = CommonFunction.setFormatCurrency(est.TaxInsAll);
            // 税金・保険料相当額
            EstimateModelView.dspTaxInsEquivalentAll = CommonFunction.setFormatCurrency(sub.TaxInsEquivalentAll);
            // 預り法定費
            EstimateModelView.dspTaxFreeAll = CommonFunction.setFormatCurrency(est.TaxFreeAll);
            // 手続代行費用
            EstimateModelView.dspTaxCostAll = CommonFunction.setFormatCurrency(est.TaxCostAll);
            // 消費税
            EstimateModelView.dspConTax = CommonFunction.setFormatCurrency(est.ConTax);
            // 車両販売総額
            EstimateModelView.dspCarSaleSum = CommonFunction.setFormatCurrency(est.CarSaleSum);
            // 下取車有無
            EstimateModelView.dspTradeInUM = sub.TradeInUm.ToString();
            // 下取車価格
            EstimateModelView.dspTradeInPrice = (est.TradeInPrice == 0 ? "" : "▲" + Convert.ToString(CommonFunction.setFormatCurrency(est.TradeInPrice)));
            // 下取車残債
            if (est.Balance == 0)
            {
                EstimateModelView.dspBalanceT = "下取車残債";
                EstimateModelView.dspBalance = "";
            }
            else
            {
                EstimateModelView.dspBalanceT = "下取車残債";
                EstimateModelView.dspBalance = CommonFunction.setFormatCurrency(est.Balance);
            }
            // 合計
            EstimateModelView.dspSalesSum = CommonFunction.setFormatCurrency(est.SalesSum);
            // 頭金
            EstimateModelView.dspDeposit = CommonFunction.setFormatCurrency(est.Deposit);
            // 割賦元金
            EstimateModelView.dspPrincipal = CommonFunction.setFormatCurrency(est.SalesSum - est.Deposit);

            // ローン内容
            // -- 分割払手数料
            if (est.PartitionFee > 0)
                EstimateModelView.dspPartitionFee = CommonFunction.setFormatCurrency((long)est.PartitionFee);
            else
                EstimateModelView.dspPartitionFee = "";

            // -- 分割払金合計
            if (est.PartitionAmount > 0)
                EstimateModelView.dspPartitionAmount = CommonFunction.setFormatCurrency(est.PartitionAmount);
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
                EstimateModelView.dspFirstPayAmount = CommonFunction.setFormatCurrency(est.FirstPayAmount);
            else
                EstimateModelView.dspFirstPayAmount = "";

            // -- 2回目以降支払額
            if (est.PayAmount > 0)
                EstimateModelView.dspPayAmount = CommonFunction.setFormatCurrency(est.PayAmount);
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
                EstimateModelView.dspBonusAmount = CommonFunction.setFormatCurrency(est.BonusAmount);

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
            if (est.OptionInputKb)
            {
                EstimateModelView.dspOptionName1 = est.OptionName1;
                EstimateModelView.dspOptionName2 = est.OptionName2;
                EstimateModelView.dspOptionName3 = est.OptionName3;
                EstimateModelView.dspOptionName4 = est.OptionName4;
                EstimateModelView.dspOptionName5 = est.OptionName5;
                EstimateModelView.dspOptionName6 = est.OptionName6;
                EstimateModelView.dspOptionName7 = est.OptionName7;
                EstimateModelView.dspOptionName8 = est.OptionName8;
                EstimateModelView.dspOptionName9 = est.OptionName9;
                EstimateModelView.dspOptionName10 = est.OptionName10;
                EstimateModelView.dspOptionName11 = est.OptionName11;
                EstimateModelView.dspOptionName12 = est.OptionName12;
                EstimateModelView.dspOptionPrice1 = CommonFunction.setFormatCurrency(est.OptionPrice1);
                EstimateModelView.dspOptionPrice2 = CommonFunction.setFormatCurrency(est.OptionPrice2);
                EstimateModelView.dspOptionPrice3 = CommonFunction.setFormatCurrency(est.OptionPrice3);
                EstimateModelView.dspOptionPrice4 = CommonFunction.setFormatCurrency(est.OptionPrice4);
                EstimateModelView.dspOptionPrice5 = CommonFunction.setFormatCurrency(est.OptionPrice5);
                EstimateModelView.dspOptionPrice6 = CommonFunction.setFormatCurrency(est.OptionPrice6);
                EstimateModelView.dspOptionPrice7 = CommonFunction.setFormatCurrency(est.OptionPrice7);
                EstimateModelView.dspOptionPrice8 = CommonFunction.setFormatCurrency(est.OptionPrice8);
                EstimateModelView.dspOptionPrice9 = CommonFunction.setFormatCurrency(est.OptionPrice9);
                EstimateModelView.dspOptionPrice10 = CommonFunction.setFormatCurrency(est.OptionPrice10);
                EstimateModelView.dspOptionPrice11 = CommonFunction.setFormatCurrency(est.OptionPrice11);
                EstimateModelView.dspOptionPrice12 = CommonFunction.setFormatCurrency(est.OptionPrice12);
            }
            // 税金・保険料明細
            if (est.TaxInsInputKb)
            {
                EstimateModelView.dspAutoTax = CommonFunction.setFormatCurrency(est.AutoTax);
                EstimateModelView.dspAutoTaxEquivalent = CommonFunction.setFormatCurrency(sub.AutoTaxEquivalent);
                if (sub.AutoTaxEquivalent > 0)
                    EstimateModelView.dspAutoTaxMonth = CommonConst.def_TitleAutoTaxEquivalent;
                else
                    EstimateModelView.dspAutoTaxMonth = CommonConst.def_TitleAutoTax + (est.AutoTax == 0 ? "" : "（" + sub.AutoTaxMonth + "月中登録）");
                EstimateModelView.csvAutoTaxMonth = sub.AutoTaxMonth; //EstimateModelView.csv用
                EstimateModelView.dspAcqTax = CommonFunction.setFormatCurrency(est.AcqTax);
                EstimateModelView.dspWeightTax = CommonFunction.setFormatCurrency(est.WeightTax);
                EstimateModelView.dspDamageIns = CommonFunction.setFormatCurrency(est.DamageIns);
                EstimateModelView.dspDamageInsEquivalent = CommonFunction.setFormatCurrency(sub.DamageInsEquivalent);

                if (sub.DamageInsEquivalent > 0)
                    EstimateModelView.dspDamageInsMonth = CommonConst.def_TitleDamageInsEquivalent;
                else
                    EstimateModelView.dspDamageInsMonth = CommonConst.def_TitleDamageIns + (est.DamageIns == 0 ? "" : "（" + sub.DamageInsMonth + "ヶ月）");
                EstimateModelView.csvDamageInsMonth = sub.DamageInsMonth; //EstimateModelView.csv用
                EstimateModelView.dspOptionIns = CommonFunction.setFormatCurrency(est.OptionIns);
            }
            // 預り法定費明細
            if (est.TaxFreeKb)
            {
                EstimateModelView.dspTaxFreeGarage = CommonFunction.setFormatCurrency(est.TaxFreeGarage);
                EstimateModelView.dspTaxFreeCheck = CommonFunction.setFormatCurrency(est.TaxFreeCheck);
                EstimateModelView.dspTaxFreeTradeIn = CommonFunction.setFormatCurrency(est.TaxFreeTradeIn);
                EstimateModelView.dspTaxFreeRecycle = CommonFunction.setFormatCurrency(est.TaxFreeRecycle);
                EstimateModelView.dspTaxFreeOther = CommonFunction.setFormatCurrency(est.TaxFreeOther);
                EstimateModelView.dspTaxFreeSet1Title = sub.TaxFreeSet1Title;
                EstimateModelView.dspTaxFreeSet1 = CommonFunction.setFormatCurrency(sub.TaxFreeSet1);
                EstimateModelView.dspTaxFreeSet2Title = sub.TaxFreeSet2Title;
                EstimateModelView.dspTaxFreeSet2 = CommonFunction.setFormatCurrency(sub.TaxFreeSet2);
            }
            // 手続代行費明細
            if (est.TaxCostKb)
            {
                EstimateModelView.dspTaxGarage = CommonFunction.setFormatCurrency(est.TaxGarage);
                EstimateModelView.dspTaxCheck = CommonFunction.setFormatCurrency(est.TaxCheck);
                EstimateModelView.dspTaxTradeIn = CommonFunction.setFormatCurrency(est.TaxTradeIn);
                EstimateModelView.dspTaxDelivery = CommonFunction.setFormatCurrency(est.TaxDelivery);
                EstimateModelView.dspTaxRecycle = CommonFunction.setFormatCurrency(est.TaxRecycle);
                EstimateModelView.dspTaxOther = CommonFunction.setFormatCurrency(est.TaxOther);

                EstimateModelView.dspTaxTradeInSatei = CommonFunction.setFormatCurrency(sub.TaxTradeInSatei);
                EstimateModelView.dspTaxSet1Title = sub.TaxSet1Title;
                EstimateModelView.dspTaxSet1 = CommonFunction.setFormatCurrency(sub.TaxSet1);
                EstimateModelView.dspTaxSet2Title = sub.TaxSet2Title;
                EstimateModelView.dspTaxSet2 = CommonFunction.setFormatCurrency(sub.TaxSet2);
                EstimateModelView.dspTaxSet3Title = sub.TaxSet3Title;
                EstimateModelView.dspTaxSet3 = CommonFunction.setFormatCurrency(sub.TaxSet3);
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

        /// <summary>
        /// 新見積書番号取得
        /// </summary>
        /// <param name="outEstNo"></param>
        /// <returns></returns>
        public bool getEstNoFromDb(ref string outEstNo)
        {
            try
            {
                // 現在日付YYMMDDを設定
                var dtNow = DateTime.Now;
                // 年を取得する
                string iYear = Strings.Right(dtNow.Year.ToString(), 2);
                // 月を取得する
                string iMonth = Strings.Format(dtNow.Month, "00");
                // 日を取得する
                string iDay = Strings.Format(dtNow.Day, "00");

                string strNow = iYear + iMonth + iDay;

                // 現在の最大Noを取得
                var getMaxEstNo = (from est in _unitOfWork.DbContext.TEstimates
                                   where est.EstNo.Substring(0, 7) == strNow
                                   select new { MaxEstNo = string.IsNullOrEmpty(est.EstNo) ? est.EstNo.Max() : '0' }).FirstOrDefault();

                if (getMaxEstNo == null || getMaxEstNo.MaxEstNo.ToString() == "0")
                    outEstNo = strNow + "00001";
                else
                    outEstNo = strNow + Strings.Format(Convert.ToInt16(Strings.Right(getMaxEstNo.MaxEstNo.ToString(), 5)) + 1, "00000");
            }
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
                // 現在の最大SubNoを取得
                var getMaxEstSubNo = (from est in _unitOfWork.DbContext.TEstimates
                                      where est.EstNo == inEstNo
                                      select new { MaxEstSubNo = string.IsNullOrEmpty(est.EstSubNo) ? est.EstSubNo.Max() : '0' }).FirstOrDefault();

                // 当番号が1件もない場合
                if (getMaxEstSubNo == null || getMaxEstSubNo.MaxEstSubNo.ToString() == "0")
                    outEstSubNo = "01";
                else
                    outEstSubNo = Strings.Format(Convert.ToInt16(getMaxEstSubNo.MaxEstSubNo.ToString()) + 1, "00");
            }
            catch (Exception ex)
            {
                // エラーログ書出し
                _logger.LogError(ex, "getEstSubNoFromDb - CEST-030D");
                return false;
            }

            return true;
        }


        public void setEstData(ref LogToken logToken)
        {
            const string LOAN_RECALC_CLEAR = "※ご確認ください<br />お支払い総合計が変更になりましたが、自動計算を行わない設定になっている為、ローン計算情報をクリアしました";
            const string LOAN_RECALC_NORMAL_END = "※ご確認ください<br />お支払い総合計が変更になりましたので、入力済の金利、支払い条件<br />ボーナス加算条件等のローン条件を元に、ローン再計算を行いました";
            const string LOAN_RECALC_ERROR = "※ご確認ください<br />お支払い総合計が変更になりましたが、ローン再計算でエラーが発生した為、ローン計算情報をクリアしました";

            // 見積書番号を取得
            if (string.IsNullOrEmpty(logToken.sesEstNo) || string.IsNullOrEmpty(logToken.sesEstSubNo))
            {
                logToken.sesErrMsg = CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "SMAI-040S" + CommonConst.def_ErrCodeR;
                return;
            }

            // 見積書データ取得
            if (!getEstData(logToken.sesEstNo, logToken.sesEstSubNo))
            {
                valToken.sesErrMsg = CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "CEST-051D" + CommonConst.def_ErrCodeR;
                return;
            }

            // 走行距離
            if (Information.IsNumeric(EstimateModelView.dspNowOdometer))
                EstimateModelView.dspNowRun = CommonFunction.setFormatCurrency(EstimateModelView.dspNowOdometer, EstimateModelView.dspMilUnit);

            // 排気量
            if (Information.IsNumeric(EstimateModelView.dspDispVol))
                EstimateModelView.dspVol = CommonFunction.setFormatCurrency(EstimateModelView.dspDispVol, EstimateModelView.dspDispVolUnit);

            // 修復歴
            switch (EstMainModel.AccidentHis)
            {
                case 1:
                    {
                        EstimateModelView.dspRaJikoHisU = true;
                        EstimateModelView.dspRaJikoHisMu = false;
                        break;
                    }

                case 0:
                    {
                        EstimateModelView.dspRaJikoHisU = false;
                        EstimateModelView.dspRaJikoHisMu = true;
                        break;
                    }

                case 2:
                    {
                        EstimateModelView.dspRaJikoHisU = false;
                        EstimateModelView.dspRaJikoHisMu = false;
                        break;
                    }
            }

            // 下取車走行距離
            if (Information.IsNumeric(EstimateModelView.dspTradeInNowOdometer))
                EstimateModelView.dspSitaRun = CommonFunction.setFormatCurrency(EstimateModelView.dspTradeInNowOdometer, EstimateModelView.dspTradeInMilUnit);

            EstimateModelView.dspWarningRecalc = EstMainModel.LoanInfo == CommonConst.def_LoanInfo_Clear ? LOAN_RECALC_CLEAR :
                                                 EstMainModel.LoanInfo == CommonConst.def_LoanInfo_NormalEnd ? LOAN_RECALC_NORMAL_END :
                                                  EstMainModel.LoanInfo == CommonConst.def_LoanInfo_Error ? LOAN_RECALC_ERROR : "";

            EstimateModelView.dspPrincipalTxt = EstMainModel.Principal == 0 ? EstimateModelView.dspSalesSum : EstimateModelView.dspPrincipal;

            EstimateModelView.dspAAInfoText = valToken.sesMode == "1" ? EstimateModelView.dspAAInfo : "";
        }

        /// <summary>
        /// セッションに保持していた会員ユーザーのお客様の情報を画面にセット
        /// </summary>
        public void setCustInfo()
        {
            EstimateModelView.dspCustNm_forPrint = string.IsNullOrEmpty(valToken.sesCustNm_forPrint) ? "" : valToken.sesCustNm_forPrint;
            EstimateModelView.dspCustZip_forPrint = string.IsNullOrEmpty(valToken.sesCustZip_forPrint) ? "" : valToken.sesCustZip_forPrint;
            EstimateModelView.dspCustAdr_forPrint = string.IsNullOrEmpty(valToken.sesCustAdr_forPrint) ? "" : valToken.sesCustAdr_forPrint;
            EstimateModelView.dspCustTel_forPrint = string.IsNullOrEmpty(valToken.sesCustTel_forPrint) ? "" : valToken.sesCustTel_forPrint;
        }

        // ******************************************
        // 会員ユーザーのお客様の情報をセッションにセット
        // または、セッションに保持していた会員ユーザーのお客様の情報をクリア
        // ******************************************
        public void setSesCustInfo(LogToken logToken, bool flgRemove = false)
        {
            if (flgRemove)
            {

                logToken.sesCustNm_forPrint = "";
                logToken.sesCustZip_forPrint = "";
                logToken.sesCustAdr_forPrint = "";
                logToken.sesCustTel_forPrint = "";
            }
            else
            {
                logToken.sesCustNm_forPrint = EstimateModelView.dspCustNm_forPrint;
                logToken.sesCustZip_forPrint = EstimateModelView.dspCustZip_forPrint;
                logToken.sesCustAdr_forPrint = EstimateModelView.dspCustAdr_forPrint;
                logToken.sesCustTel_forPrint = EstimateModelView.dspCustTel_forPrint;
            }
        }

        // ******************************************
        // 会員ユーザーのお客様の情報をセッションにセット
        // または、セッションに保持していた会員ユーザーのお客様の情報をクリア
        // ******************************************
        public void setEstIDEData(ref LogToken logToken)
        {
            // 見積書番号を取得
            if (string.IsNullOrEmpty(logToken.sesEstNo) || string.IsNullOrEmpty(logToken.sesEstSubNo))
            {
                logToken.sesErrMsg = CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "SMAI-040S" + CommonConst.def_ErrCodeR;
                return;
            }

            // get [t_EstimateIde]
            if (!getEstIDEData(logToken.sesEstNo, logToken.sesEstSubNo))
            {
                valToken.sesErrMsg = CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "SMAI-041D" + CommonConst.def_ErrCodeR;
                return;
            }

            // get [MT_IDE_CONTRACT_PLAN]
            var getContractPlan = _unitOfWorkIDE.ContractPlans.GetSingleOrDefault(x => x.Id == EstimateModelView.dspInsuranceCompanyID);
            EstimateModelView.dspContractPlanName = getContractPlan == null ? "" : getContractPlan.PlanName;

            // get [MT_IDE_VOLUNTARY_INSURANCE]
            var getVoluntaryInsurance = _unitOfWorkIDE.VoluntaryInsurances.GetSingleOrDefault(x => x.Id == EstimateModelView.dspInsuranceCompanyID);
            EstimateModelView.dspInsuranceCompanyName = getVoluntaryInsurance == null ? "" : getVoluntaryInsurance.CompanyName;
        }

        /// <summary>
        /// 見積書データ取得
        /// </summary>
        /// <param name="inEstNo"></param>
        /// <param name="inEstSubNo"></param>
        /// <returns></returns>
        public bool getEstIDEData(string inEstNo, string inEstSubNo)
        {
            try
            {
                var estIdeModel = _unitOfWork.EstimateIdes.GetSingleOrDefault(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo);

                if (estIdeModel == null)
                {
                    estIdeModel = new TEstimateIde
                    {
                        EstNo = "",
                        EstSubNo = "",
                        EstUserNo = "",
                        CarType = 0,
                        IsElectricCar = 0,
                        FirstRegistration = "",
                        InspectionExpirationDate = "",
                        LeaseStartMonth = "",
                        LeasePeriod = 0,
                        LeaseExpirationDate = "",
                        ContractPlanId = 0,
                        //IsExtendedGuarantee = Convert.ToByte("-1"),
                        InsuranceCompanyId = 0,
                        InsuranceFee = 0,
                        DownPayment = 0,
                        TradeInPrice = 0,
                        FeeAdjustment = 0,
                        MonthlyLeaseFee = 0,
                        IdemitsuKosanFee = 0,
                        SalesStoreFee = 0,
                        Smasfee = 0,
                        IdemitsuCreditFee = 0,
                        Promotion = 0,
                        PromotionFee = 0,
                        ConsumptionTax = 0,
                        NameChange = 0,
                        FeeAdjustmentMax = 0,
                        FeeAdjustmentMin = 0,
                        Interest = 0,
                        GuaranteeCharge = 0,
                        MyMaintenancePrice = 0,
                        CarTax = 0,
                        LiabilityInsurance = 0,
                        WeightTax = 0,
                        LeaseProgress = 0,
                        IsApplyLease = 0,
                    };
                }

                estIdeModel.IsExtendedGuarantee = unchecked((byte)(-1));

                var estIdeModelMap = _mapper.Map<EstimateIdeModel>(estIdeModel);

                creDispDataIDE(estIdeModelMap);
            }
            catch (Exception ex)
            {
                // エラーログ書出し
                _logger.LogError(ex, "getEstIDEData - CEST-040D");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 見積書データ表示用整形
        /// </summary>
        public void creDispDataIDE(EstimateIdeModel estIde)
        {
            EstimateModelView.dspFirstRegistration = CommonFunction.getFormatDayYMD(estIde.FirstRegistration);

            EstimateModelView.dspInspectionExpirationDate = !string.IsNullOrEmpty(estIde.InspectionExpirationDate) ? CommonFunction.getFormatDayYMD(estIde.InspectionExpirationDate) : "";

            EstimateModelView.dspLeaseStartMonth = CommonFunction.getFormatDayYMD(estIde.LeaseStartMonth);

            EstimateModelView.dspLeasePeriod = string.IsNullOrEmpty(estIde.LeaseStartMonth) || estIde.LeaseStartMonth == "0" ? "" : estIde.LeasePeriod + "ヶ月";

            EstimateModelView.dspLeaseExpirationDate = CommonFunction.getFormatDayYMD(estIde.LeaseExpirationDate);

            var monthlyLeaseFee = estIde.MonthlyLeaseFee == 0 ? "" : CommonFunction.setFormatCurrency(estIde.MonthlyLeaseFee);
            EstimateModelView.dspLeaseTotalMsg = string.IsNullOrEmpty(EstimateModelView.dspLeasePeriod) ? "月額リース料(税込)" : "月額リース料(税込) " + monthlyLeaseFee + " (" + EstimateModelView.dspLeasePeriod + ")";

            EstimateModelView.dspContractPlanID = estIde.ContractPlanId;

            EstimateModelView.dspIsExtendedGuarantee = estIde.IsExtendedGuarantee == unchecked((byte)(-1)) ? "" : estIde.IsExtendedGuarantee == 0 ? "あり" : "なし";

            EstimateModelView.dspInsuranceCompanyID = estIde.InsuranceCompanyId;

            EstimateModelView.dspInsuranceFee = estIde.InsuranceFee == 0 ? "" : CommonFunction.setFormatCurrency(estIde.InsuranceFee);
            EstimateModelView.dspDownPayment = estIde.DownPayment == 0 ? "" : CommonFunction.setFormatCurrency(estIde.DownPayment);
            EstimateModelView.dspIdeTradeInPrice = estIde.TradeInPrice == 0 ? "" : CommonFunction.setFormatCurrency(estIde.TradeInPrice);
        }

    }
}
