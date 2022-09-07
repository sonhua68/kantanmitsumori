using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace KantanMitsumori.Service
{
    public class AppService : IAppService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        private LogToken valToken;
        private CommonFuncHelper _commonFuncHelper;
        private CommonEstimate _commonEst;

        public AppService(IMapper mapper, ILogger<AppService> logger, IUnitOfWork unitOfWork, CommonFuncHelper commonFuncHelper, CommonEstimate commonEst)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonFuncHelper = commonFuncHelper;
            _commonEst = commonEst;
        }

        public async Task<ResponseBase<int>> CreateMaker(MakerModel model)
        {
            ResponseBase<int> iResult = new ResponseBase<int>();
            try
            {

                var data = _mapper.Map<MMaker>(model);
                _unitOfWork.Makers.Add(data);
                await _unitOfWork.CommitAsync();
                iResult.Data = 0;
                iResult.MessageCode = "E0001";
                iResult.MessageCode = "";
                return ResponseHelper.Ok<int>("", "", 0);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error" + ex.Message);
                return ResponseHelper.Ok<int>("", "", 0);
            }
        }

        public ResponseBase<List<MakerModel>> GetMaker()
        {
            try
            {
                ResponseBase<List<MakerModel>> iResult = new ResponseBase<List<MakerModel>>();

                var makerList = _unitOfWork.Makers.GetAll().Select(i => _mapper.Map<MakerModel>(i)).ToList();

                return ResponseHelper.Ok<List<MakerModel>>("", "", makerList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error EstNo={0} EstSubNo={1}", "00000", "000000");
                return ResponseHelper.Error<List<MakerModel>>("Error", "Error");
            }
        }

        public UserModel getUserName(string userNo)
        {
            try
            {
                var mUser = _mapper.Map<UserModel>(_unitOfWork.Users.GetSingle(x => x.UserNo == userNo));
                //var mUser = _unitOfWork.Users.GetAll().Where(u => u.UserNo == userNo).Select(i => _mapper.Map<UserModel>(i)).FirstOrDefault();

                mUser!.UserInfo = mUser.UserNo + " " + mUser.UserNm + " 様";
                return mUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GCMF-020D" + " ◆会員認証エラー◆ 復号化後会員番号：" + userNo);
                return null;
            }
        }


        public ResponseBase<ResponEstMainModel> getEstMain(string sel, RequestHeaderModel request)
        {
            try
            {
                var resEstMainModel = new ResponEstMainModel();

                valToken = new LogToken();

                valToken.stateLoadWindow = "EstMain";

                if ((string.IsNullOrEmpty(sel) ? "0" : sel) == "0")
                {
                    valToken.sesPriDisp = "0";
                }

                valToken.stateLoadWindow = "EstMain";

                // ASNET、店頭商談NETの判定
                if (request.Mode != "" && CommonFunction.IsNumeric(request.Mode!))
                {
                    valToken.sesMode = request.Mode;
                }
                else
                {
                    valToken.sesMode = "";
                    valToken.sesErrMsg = CommonConst.def_ErrMsg4 + CommonConst.def_ErrMsg4 + "SMAI-001P" + CommonConst.def_ErrCodeR;
                    return ResponseHelper.Error<ResponEstMainModel>("Error", valToken.sesErrMsg);
                }

                // 価格表示有無の取得（店頭商談NET
                if (!string.IsNullOrEmpty(request.PriDisp) && CommonFunction.IsNumeric(request.PriDisp!))
                {
                    valToken.sesPriDisp = request.PriDisp;
                }

                if (!string.IsNullOrEmpty(request.leaseFlag) && CommonFunction.IsNumeric(request.leaseFlag!))
                {
                    valToken.sesLeaseFlag = request.leaseFlag;
                }
                else
                {
                    valToken.sesLeaseFlag = "0";
                }

                // ASNET車両詳細ページからの情報を取得・DB保存
                var getAsInfo = getAsnetInfo(request);

                resEstMainModel.Token = valToken.Token;

                if (getAsInfo.ResultStatus == (int)enResponse.isError)
                {
                    return ResponseHelper.Error<ResponEstMainModel>("Error", getAsInfo.MessageContent);
                }
                return ResponseHelper.Ok<ResponEstMainModel>("", "", data: resEstMainModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return ResponseHelper.Error<ResponEstMainModel>("Error", "Error");
            }
        }

        /// <summary>
        /// ASNET車両ページからの情報を取得、DB保存
        /// </summary>
        private ResponseBase<string> getAsnetInfo(RequestHeaderModel request)
        {
            bool isCheck = (string.IsNullOrEmpty(request.cot) || string.IsNullOrEmpty(request.cna) || string.IsNullOrEmpty(request.mem));
            if (isCheck)
            {
                valToken.sesErrMsg = CommonConst.def_ErrMsg3 + CommonConst.def_ErrCodeL + "SMAI-020P" + CommonConst.def_ErrCodeR;
                return ResponseHelper.Error<string>("Error", valToken.sesErrMsg);
            }
            string strTempImagePath;
            string strSavePath = "";

            // get user info
            var userInfo = getUserInfo(request.mem);

            // ラベルセット
            valToken.UserNo = userInfo.Data!.UserNo;
            valToken.UserNm = userInfo.Data!.UserNm;


            valToken.sesUserNo = userInfo.Data!.UserNo;
            valToken.sesUserNm = userInfo.Data!.UserNm;
            valToken.sesUserAdr = userInfo.Data!.UserAdr;
            valToken.sesUserTel = userInfo.Data!.UserTel;
            valToken.sesdispUserInf = userInfo.Data!.UserInfo;

            if (request.exh != "")          // '出品番号
            {
                string wAANo = request.exh!;
                string wAAPlace = request.aan!;
                string wConnerType = request.cot!;
                string wMode = request.Mode!;

                // 同じAA会場の出品車輌の見積書をすでに(何回か)作成していた場合は
                // 最新の見積データを取得し、新しい見積枝番でデータ作成。
                if (_commonEst.chkAANo(valToken.sesUserNo, wAANo, wAAPlace, int.Parse(wConnerType), int.Parse(wMode)))
                {
                    // 開催数追加対応 
                    if (!_commonEst.addEstNextSubNo())
                    {
                        return ResponseHelper.Error<string>("Error", "Error");
                    }
                    else
                    {
                        return ResponseHelper.Error<string>("Error", CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "GCMF-040D" + CommonConst.def_ErrCodeR);
                    }
                }
            }

            // 作成ユーザー
            _commonEst.EstMainModel.EstUserNo = valToken.sesUserNo;
            // ワンプラorワンプラ以外
            if (request.cot == "2" || request.cot == "5")
            {
                _commonEst.EstMainModel.CallKbn = "1";   // ワンプラ
            }
            else
            {
                _commonEst.EstMainModel.CallKbn = "2";
            }   // ワンプラ以外

            int vAAcount = 0;
            if (request.cot == "1" | request.cot == "2")
            {
                _commonFuncHelper.GetAACount(request.cor, ref vAAcount);
            }
            // 開催数追加対応 2015/01/16 End
            // ASNET車両見積もり
            _commonEst.EstMainModel.EstInpKbn = "1";
            // 見積日
            _commonEst.EstMainModel.TradeDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            _commonEst.EstMainModel.MakerName = request.mak; // メーカー
            _commonEst.EstMainModel.ModelName = request.cna; // 車名
            _commonEst.EstMainModel.GradeName = request.gra; // グレード
            _commonEst.EstMainModel.Case = request.carCase; // 型式
            _commonEst.EstMainModel.ChassisNo = request.pla; // 車台番号
            string vNensiki = request.mod.Trim();
            if (string.IsNullOrEmpty(vNensiki) || !Information.IsNumeric(vNensiki))    // 年式
            {
                _commonEst.EstMainModel.FirstRegYm = "";
            }
            else
            {
                _commonEst.EstMainModel.FirstRegYm = vNensiki;
            }
            string strCheckCarYm = CommonFunction.setCheckCarYm(request.ins);
            _commonEst.EstMainModel.CheckCarYm = strCheckCarYm; // 車検

            string wNowOdometer = request.mil;
            if (string.IsNullOrEmpty(wNowOdometer))     // 走行距離
            {
                _commonEst.EstMainModel.NowOdometer = 0;
            }
            else if (Information.IsNumeric(wNowOdometer))
            {
                _commonEst.EstMainModel.NowOdometer = int.Parse(wNowOdometer);
            }
            else
            {
                _commonEst.EstMainModel.NowOdometer = 0;
            }
            if (request.milUnit == default) // 走行距離 単位
            {
                // 過渡期には既定値セット
                _commonEst.EstMainModel.MilUnit = CommonConst.def_MilUnitTKM;
            }
            else if (request.milUnit.ToLower() == "null")
            {
                _commonEst.EstMainModel.MilUnit = "";
            }
            else
            {
                _commonEst.EstMainModel.MilUnit = request.milUnit;
            }

            if (request.vol == "")   // 排気量
            {
                _commonEst.EstMainModel.DispVol = "";
            }
            else
            {
                _commonEst.EstMainModel.DispVol = Strings.Trim(Strings.Replace(request.vol, "cc", ")"));
            } // 排気量（元データに "cc" が入っていた場合のガード）
            if (request.volUnit == default) // 排気量 単位
            {
                // 過渡期には既定値セット
                _commonEst.EstMainModel.DispVolUnit = CommonConst.def_DispVolUnitCC;
            }
            else if (request.volUnit.ToLower() == "null")
            {
                // "null" の場合には既定値セット
                // （ASNET/店頭商談NET 側は、コーナー15以外の連動先からも排気量単位が正しく取得できないうちは実装しないとのこと）
                _commonEst.EstMainModel.DispVolUnit = CommonConst.def_DispVolUnitCC;
            }
            else
            {
                _commonEst.EstMainModel.DispVolUnit = request.volUnit;
            }

            if (request.shi == "")    // シフト
            {
                _commonEst.EstMainModel.Mission = "";
            }
            else
            {
                _commonEst.EstMainModel.Mission = request.shi;
            }

            _commonEst.EstMainModel.AccidentHis = 2;

            if (request.his == "")  // 車歴
            {
                _commonEst.EstMainModel.BusinessHis = "";
            }
            else
            {
                _commonEst.EstMainModel.BusinessHis = request.his;
            }

            _commonEst.EstMainModel.FuelName = request.FuelName;
            _commonEst.EstMainModel.DriveName = request.DriveName;
            _commonEst.EstMainModel.CarDoors = Information.IsNumeric(request.CarDoors) ? int.Parse(request.CarDoors) : 0;
            _commonEst.EstMainModel.BodyName = request.BodyName;
            _commonEst.EstMainModel.Capacity = Information.IsNumeric(request.Capacity) ? int.Parse(request.Capacity) : 0;

            if (request.equ == "")  // オプション
            {
                _commonEst.EstMainModel.Equipment = "";
            }
            else
            {
                _commonEst.EstMainModel.Equipment = request.equ;
            }
            if (request.col == "")   // 色
            {
                _commonEst.EstMainModel.BodyColor = "";
            }
            else
            {
                _commonEst.EstMainModel.BodyColor = request.col;
            }


            string wCarImgPath = request.img;
            string outImg = "";
            string outImg1 = "";
            string outImg2 = ""; string outImg3 = ""; string outImg4 = ""; string outImg5 = ""; string outImg6 = ""; string outImg7 = ""; string outImg8 = "";
            if (string.IsNullOrEmpty(wCarImgPath))
            {
                _commonEst.EstMainModel.CarImgPath = CommonConst.def_DmyImg;
                _commonEst.EstMainModel.CarImgPath1 = "";
                _commonEst.EstMainModel.CarImgPath2 = "";
                _commonEst.EstMainModel.CarImgPath3 = "";
                _commonEst.EstMainModel.CarImgPath4 = "";
                _commonEst.EstMainModel.CarImgPath5 = "";
                _commonEst.EstMainModel.CarImgPath6 = "";
                _commonEst.EstMainModel.CarImgPath7 = "";
                _commonEst.EstMainModel.CarImgPath8 = "";
            }
            else
            {
                // 車両画像を見積システムサーバへ格納
                strTempImagePath = wCarImgPath.ToUpper();
                if (!strTempImagePath.EndsWith(".JPG") & !strTempImagePath.EndsWith(".GIF") & !strTempImagePath.EndsWith(".PNG"))
                {
                    strSavePath = request.cor + request.fex + "001.jpg";
                }
                _commonFuncHelper.DownloadImg(wCarImgPath, valToken.sesCarImgPath, CommonConst.def_DmyImg, ref outImg, strSavePath);
                _commonEst.EstMainModel.CarImgPath = outImg;

                // フロント画像以外の取得
                _commonFuncHelper.CheckImgPath(request.img1, valToken.sesCarImgPath1, "", ref outImg1, "201.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img2, valToken.sesCarImgPath2, "", ref outImg2, "202.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img3, valToken.sesCarImgPath3, "", ref outImg3, "203.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img4, valToken.sesCarImgPath4, "", ref outImg4, "204.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img5, valToken.sesCarImgPath5, "", ref outImg5, "205.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img6, valToken.sesCarImgPath6, "", ref outImg6, "206.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img7, valToken.sesCarImgPath7, "", ref outImg7, "207.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img8, valToken.sesCarImgPath8, "", ref outImg8, "208.jpg", request.cor, request.fex);

                _commonEst.EstMainModel.CarImgPath1 = outImg1;
                _commonEst.EstMainModel.CarImgPath2 = outImg2;
                _commonEst.EstMainModel.CarImgPath3 = outImg3;
                _commonEst.EstMainModel.CarImgPath4 = outImg4;
                _commonEst.EstMainModel.CarImgPath5 = outImg5;
                _commonEst.EstMainModel.CarImgPath6 = outImg6;
                _commonEst.EstMainModel.CarImgPath7 = outImg7;
                _commonEst.EstMainModel.CarImgPath8 = outImg8;
            }

            _commonEst.EstMainModel.TotalCost = 0;

            // 業販価格 又は スタート金額
            string wCarPrice = request.pri;
            decimal intCarPrice = 0m; // 浮動小数点の計算誤差回避のため
            if (Information.IsNumeric(wCarPrice))
            {
                intCarPrice = Convert.ToDecimal(wCarPrice);
            }
            else
            {
                intCarPrice = 0m;
            }

            string wRakuSatu = request.fee;  // 落札料
            if (Information.IsNumeric(wRakuSatu))
            {
                _commonEst.EstMainModel.RakuSatu = int.Parse(wRakuSatu);
                // 20190110 y.suzuki add start
                // JUCテントリ(F6)落札手数料改訂対応
                if (request.cor == "F6")
                {
                    _commonEst.EstMainModel.RakuSatu = 25000;
                }
            }
            // 20190110 y.suzuki add end
            else
            {
                _commonEst.EstMainModel.RakuSatu = 0;
            }

            string wRikusou = request.tra;  // 陸送代
            if (Information.IsNumeric(wRikusou))
            {
                _commonEst.EstMainModel.Rikusou = int.Parse(wRikusou);
            }
            else
            {
                _commonEst.EstMainModel.Rikusou = 0;
            }

            // 車両価格は後で計算
            // _commonEst.EstMainModel.CarPrice = intCarPrice + _commonEst.EstMainModel.RakuSatu + _commonEst.EstMainModel.Rikusou
            _commonEst.EstMainModel.Discount = 0;
            _commonEst.EstMainModel.NouCost = 0;
            _commonEst.EstMainModel.SyakenNew = 0;
            _commonEst.EstMainModel.SyakenZok = 0;
            _commonEst.EstMainModel.CarSum = 0;
            _commonEst.EstMainModel.OptionInputKb = true;
            _commonEst.EstMainModel.OptionName1 = "";
            _commonEst.EstMainModel.OptionPrice1 = 0;
            _commonEst.EstMainModel.OptionName2 = "";
            _commonEst.EstMainModel.OptionPrice2 = 0;
            _commonEst.EstMainModel.OptionName3 = "";
            _commonEst.EstMainModel.OptionPrice3 = 0;
            _commonEst.EstMainModel.OptionName4 = "";
            _commonEst.EstMainModel.OptionPrice4 = 0;
            _commonEst.EstMainModel.OptionName5 = "";
            _commonEst.EstMainModel.OptionPrice5 = 0;
            _commonEst.EstMainModel.OptionName6 = "";
            _commonEst.EstMainModel.OptionPrice6 = 0;
            _commonEst.EstMainModel.OptionPriceAll = 0;
            _commonEst.EstMainModel.TaxInsInputKb = true;
            _commonEst.EstMainModel.AutoTax = 0;
            _commonEst.EstMainModel.AcqTax = 0;
            _commonEst.EstMainModel.WeightTax = 0;
            _commonEst.EstMainModel.DamageIns = 0;
            _commonEst.EstMainModel.OptionIns = 0;
            _commonEst.EstMainModel.TaxInsAll = 0;
            _commonEst.EstMainModel.TaxFreeKb = true;
            _commonEst.EstMainModel.TaxFreeGarage = 0;
            _commonEst.EstMainModel.TaxFreeCheck = 0;
            _commonEst.EstMainModel.TaxFreeTradeIn = 0;

            // request.fex") は、8桁出品番号
            if (request.fex == "")
            {
                _commonEst.EstMainModel.TaxFreeRecycle = 0;
            }
            else
            {
                int intTaxFreeRecycle = 0;
                if (_commonFuncHelper.GetRecDeposit(request.cor, request.fex, ref intTaxFreeRecycle))
                {
                    _commonEst.EstMainModel.TaxFreeRecycle = intTaxFreeRecycle;
                }
                else
                {
                    _commonEst.EstMainModel.TaxFreeRecycle = 0;
                }
            }

            _commonEst.EstMainModel.TaxFreeOther = 0;
            _commonEst.EstMainModel.TaxFreeAll = 0;
            _commonEst.EstMainModel.TaxCostKb = true;
            _commonEst.EstMainModel.TaxGarage = 0;
            _commonEst.EstMainModel.TaxCheck = 0;
            _commonEst.EstMainModel.TaxTradeIn = 0;
            _commonEst.EstMainModel.TaxDelivery = 0;
            _commonEst.EstMainModel.TaxRecycle = 0;
            _commonEst.EstMainModel.TaxOther = 0;
            _commonEst.EstMainModel.TaxCostAll = 0;
            _commonEst.EstMainModel.ConTax = 0;
            _commonEst.EstMainModel.CarSaleSum = 0;
            _commonEst.EstMainModel.TradeInCarName = "";
            _commonEst.EstMainModel.TradeInFirstRegYm = "";
            _commonEst.EstMainModel.TradeInCheckCarYm = "";
            _commonEst.EstMainModel.TradeInNowOdometer = 0;
            _commonEst.EstMainModel.TradeInMilUnit = CommonConst.def_TradeInMilUnitKM; // 下取車 走行距離 単位（既定値）
            _commonEst.EstMainModel.TradeInRegNo = "";
            _commonEst.EstMainModel.TradeInChassisNo = "";
            _commonEst.EstMainModel.TradeInBodyColor = "";
            _commonEst.EstMainModel.TradeInPrice = 0;
            _commonEst.EstMainModel.Balance = 0;
            _commonEst.EstMainModel.SalesSum = 0;
            _commonEst.EstMainModel.CustKname = "";
            _commonEst.EstMainModel.Rate = 0;
            _commonEst.EstMainModel.Deposit = 0;
            _commonEst.EstMainModel.PartitionFee = 0;
            _commonEst.EstMainModel.PartitionAmount = 0;
            _commonEst.EstMainModel.PayTimes = 0;
            _commonEst.EstMainModel.FirstPayMonth = "";
            _commonEst.EstMainModel.LastPayMonth = "";
            _commonEst.EstMainModel.FirstPayAmount = 0;
            _commonEst.EstMainModel.PayAmount = 0;
            _commonEst.EstMainModel.BonusAmount = 0;
            _commonEst.EstMainModel.BonusFirst = "";
            _commonEst.EstMainModel.BonusSecond = "";
            _commonEst.EstMainModel.BonusTimes = 0;
            _commonEst.EstMainModel.ShopNm = valToken.sesUserNm;
            _commonEst.EstMainModel.ShopAdr = valToken.sesUserAdr;
            _commonEst.EstMainModel.ShopTel = valToken.sesUserTel;
            _commonEst.EstMainModel.EstTanName = "";
            _commonEst.EstMainModel.SekininName = "";
            // 開催数追加対応
            _commonEst.EstMainModel.Corner = request.cor;
            _commonEst.EstMainModel.Aacount = vAAcount;
            _commonEst.EstMainModel.Mode = Convert.ToByte(request.Mode);
            _commonEst.EstMainModel.Notes = "";

            _commonEst.EstMainModel.Aayear = vNensiki;

            string wAAHyk = request.poi;  // 評価点
            if (Information.IsNumeric(wAAHyk))
            {
                _commonEst.EstMainModel.Aahyk = wAAHyk;
            }
            else
            {
                _commonEst.EstMainModel.Aahyk = "0";
            }
            _commonEst.EstMainModel.Aaprice = (int)intCarPrice;
            _commonEst.EstMainModel.SirPrice = _commonEst.EstMainModel.CarPrice;
            _commonEst.EstMainModel.YtiRieki = 0;
            _commonEst.EstMainModel.Aaplace = request.aan == "" ? "" : request.aan; // AA会場
            _commonEst.EstMainModel.Aano = request.exh == "" ? "" : request.exh; // 出品番号
            if (request.lim == "")  // 出品期間
            {
                _commonEst.EstMainModel.Aatime = "";
            }
            else
            {
                string vAATime = request.lim.Trim();
                if (Strings.Len(vAATime) == 8)
                {
                    _commonEst.EstMainModel.Aatime = Strings.Left(vAATime, 4) + "/" + Strings.Mid(vAATime, 5, 2) + "/" + Strings.Right(vAATime, 2);
                }
                else
                {
                    _commonEst.EstMainModel.Aatime = vAATime;
                }
            }

            // ここから税金・保険料の自動計算 ==============================================

            // 排気量あれば採用（必ずあるはず）
            int intHaiki = 0;
            if (Information.IsNumeric(_commonEst.EstMainModel.DispVol))
            {
                intHaiki = int.Parse(_commonEst.EstMainModel.DispVol);
            }

            bool flgTaxAutoCalc = enableTaxCalc(request.mak, this);

            // 自動車税----------------------------------------------
            string strCarTax = 0.ToString();
            int intFirstMonth = Convert.ToInt32(Strings.Format(DateTime.Now, "MM"));
            // 排気量が入力されている場合に限り、計算する。
            if (flgTaxAutoCalc & intHaiki > 0 && _commonEst.EstMainModel.DispVolUnit == CommonConst.def_DispVolUnitCC)
            {
                if (!getCarTax(intFirstMonth, intHaiki, strCarTax))
                {
                    valToken.sesErrMsg = CommonConst.def_ErrMsg1_Maker + CommonConst.def_ErrCodeL + "SMAI-023D" + CommonConst.def_ErrCodeR;

                }
                _commonEst.EstMainModel.AutoTax = string.IsNullOrEmpty(strCarTax) ? 0 : int.Parse(strCarTax);
                _commonEst.EstMainModel.AutoTaxMonth = intFirstMonth.ToString();
            }
            else
            {
                _commonEst.EstMainModel.AutoTax = 0;
                _commonEst.EstMainModel.AutoTaxMonth = "";
            }

            SetvalueToken();

            return ResponseHelper.Ok<string>("OK", "OK", "");

        }

        public ResponseBase<UserModel> getUserInfo(string mem)
        {
            // 会員番号取得
            string decUsrNo = "";

            if (!_commonFuncHelper.DecUserNo(mem.Trim(), ref decUsrNo))
            {
                valToken.sesErrMsg = CommonConst.def_ErrMsg3 + CommonConst.def_ErrCodeL + "SMAI-021C" + CommonConst.def_ErrCodeR;
                //Redirect(CommonConst.def_ErrPage);
                return ResponseHelper.Error<UserModel>("Error", valToken.sesErrMsg);
            }

            var responseUser = getUserName(decUsrNo);

            if (responseUser == null)
            {
                return ResponseHelper.Error<UserModel>("Error", CommonConst.def_ErrMsg3 + CommonConst.def_ErrCodeL + "SMAI-042D" + CommonConst.def_ErrCodeR);
            }

            return ResponseHelper.Ok<UserModel>("OK", "OK", responseUser);
        }

        #region fuc private

        /// <summary>
        /// 見積書データ表示用整形
        /// </summary>
        private void SetvalueToken()
        {
            var token = HelperToken.GenerateJsonToken(valToken);
            valToken.Token = token;
        }

        #endregion fuc private
    }
}