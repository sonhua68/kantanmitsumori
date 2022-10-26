using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using KantanMitsumori.Service.Mapper.MapperConverter;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KantanMitsumori.Service.ASEST
{
    public class EstMainService : IEstMainService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfWorkIDE _unitOfWorkIDE;
        private readonly CommonFuncHelper _commonFuncHelper;
        private readonly CommonEstimate _commonEst;
        private readonly JwtSettings _jwtSettings;
        private readonly PhysicalPathSettings _jwtPhysicalSettings;
        private readonly DataSettings _dataSettings;
        private LogToken valToken;

        public EstMainService(IMapper mapper, ILogger<EstMainService> logger, IUnitOfWork unitOfWork, IUnitOfWorkIDE unitOfWorkIDE, CommonFuncHelper commonFuncHelper, CommonEstimate commonEst,
            IOptions<DataSettings> dataSettings, IOptions<JwtSettings> jwtSettings, IOptions<PhysicalPathSettings> jwtPhysicalSettings)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _unitOfWorkIDE = unitOfWorkIDE;
            _commonFuncHelper = commonFuncHelper;
            _commonEst = commonEst;
            _unitOfWorkIDE = unitOfWorkIDE;
            _jwtSettings = jwtSettings.Value;
            _dataSettings = dataSettings.Value;
            _jwtPhysicalSettings = jwtPhysicalSettings.Value;
        }

        public UserModel? getUserName(string userNo)
        {

            try
            {
                var dtMUser = _mapper.Map<UserModel>(_unitOfWork.Users.GetSingle(x => x.UserNo == userNo));
                dtMUser!.UserInfo = dtMUser.UserNo + " " + dtMUser.UserNm + " 様";
                return dtMUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "◆会員認証エラー◆ 復号化後会員番号：", userNo);
                return null;
            }
        }
        public async Task<ResponseBase<ResponseEstMainModel>> getEstMain(RequestActionModel requestAction, RequestHeaderModel request)
        {
            try
            {
                valToken = new LogToken();
                var response = new ResponseEstMainModel
                {
                    EstCustomerModel = new EstCustomerModel(),
                    EstModelView = new EstModelView()
                };
                bool isSesPriDisp = requestAction.IsInpBack != 1 && requestAction.Sel == 0;
                valToken.sesPriDisp = isSesPriDisp ? "0" : "";
                valToken.stateLoadWindow = "EstMain";
                if (CommonFunction.IsNumeric(request.Mode))
                {
                    valToken.sesMode = request.Mode;
                }
                else
                {
                    return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAI001P, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI001P));

                }
                if (CommonFunction.IsNumeric(request.PriDisp))
                {
                    valToken.sesPriDisp = request.PriDisp;
                }
                var dtAsnetInfo = await GetAsnetInfo(request);
                if (dtAsnetInfo.ResultStatus == (int)enResponse.isError)
                    return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
                dtAsnetInfo.Data!.EstNo = valToken.sesEstNo!;
                dtAsnetInfo.Data.EstSubNo = valToken.sesEstSubNo!;
                SetvalueToken();
                response.AccessToken = valToken.Token!;
                response.EstCustomerModel.CustNm = valToken.sesCustNm_forPrint ?? "";
                response.EstCustomerModel.CustZip = valToken.sesCustZip_forPrint ?? "";
                response.EstCustomerModel.CustAdr = valToken.sesCustAdr_forPrint ?? "";
                response.EstCustomerModel.CustTel = valToken.sesCustTel_forPrint ?? "";
                var estData = _commonEst.SetEstData(valToken.sesEstNo!, valToken.sesEstSubNo!);
                if (estData.ResultStatus != (int)enResponse.isSuccess)
                {
                    return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAL041D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

                }
                response.EstModel = estData.Data!;
                response.EstIDEModel = new EstimateIdeModel();
                if (response.EstModel.LeaseFlag == "1")
                {
                    response.EstIDEModel = _commonEst.SetEstIDEData(valToken);
                    if (response.EstIDEModel == null)
                        return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAL041D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAL041D));
                }
                response = BindingDataEsmain(response);
                //show Mess error 
                if (response.EstModel.CallKbn == "2")
                {
                    response.EstModel.IsError = 1;

                } //show Mess error 
                else if (request.mod == "1" && request.PriDisp == "1")
                {
                    response.EstModel.IsError = 1;
                }
                return ResponseHelper.Ok(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getEstMain");
                return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
            }
        }
        public ResponseBase<ResponseEstMainModel> ReloadGetEstMain(LogToken logtoken)
        {
            try
            {
                var response = new ResponseEstMainModel
                {
                    EstCustomerModel = new EstCustomerModel(),
                    EstIDEModel = new EstimateIdeModel(),
                    EstModel = new EstModel(),
                    EstModelView = new EstModelView()
                };
                var estData = _commonEst.SetEstData(logtoken.sesEstNo!, logtoken.sesEstSubNo!);
                if (estData.ResultStatus != (int)enResponse.isSuccess)
                {
                    return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAL041D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

                }
                response.EstModel = estData.Data!;
                response.EstCustomerModel.CustNm = logtoken.sesCustNm_forPrint ?? "";
                response.EstCustomerModel.CustZip = logtoken.sesCustZip_forPrint ?? "";
                response.EstCustomerModel.CustAdr = logtoken.sesCustAdr_forPrint ?? "";
                response.EstCustomerModel.CustTel = logtoken.sesCustTel_forPrint ?? "";
                if (response.EstModel.LeaseFlag == "1")
                {
                    response.EstIDEModel = _commonEst.SetEstIDEData(logtoken);
                    if (response.EstIDEModel == null)
                        return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAL041D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
                }
                //show Mess error 
                if (response.EstModel.CallKbn == "2")
                {
                    response.EstModel.IsError = 1;
                }
                response = BindingDataEsmain(response);
                return ResponseHelper.Ok(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReloadGetEstMain");
                return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
            }
        }
        public ResponseBase<UserModel> getUserInfo(string mem)
        {
            string decUsrNo = "";
            if (!_commonFuncHelper.DecUserNo(mem.Trim(), ref decUsrNo))
            {
                return ResponseHelper.Error<UserModel>(HelperMessage.SMAI002S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI002S));
            }
            var responseUser = getUserName(decUsrNo);
            if (responseUser == null)
            {
                return ResponseHelper.Error<UserModel>("Error", CommonConst.def_ErrMsg3 + CommonConst.def_ErrCodeL + "SMAI-042D" + CommonConst.def_ErrCodeR);
            }
            else
            {
                return ResponseHelper.Ok(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), responseUser);
            }
        }
        public async Task<ResponseBase<ResponseEstMainModel>> setFreeEst(RequestSelGrdFreeEst model, LogToken logtoken)
        {
            try
            {


                valToken = logtoken;
                var estModel = new EstModel
                {
                    CallKbn = "3",
                    EstInpKbn = "2",
                    TradeDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")),
                    MakerName = model.MakerName!,
                    ModelName = model.ModelName!,
                    GradeName = model.GradeName!,
                    Case = model.CarCase!,
                    MilUnit = CommonConst.def_MilUnitTKM,
                    DispVol = model.DispVol!,
                    DispVolUnit = CommonConst.def_DispVolUnitCC,
                    AccidentHis = 2,
                    CarImgPath = CommonConst.def_DmyImg,
                    SonotaTitle = CommonConst.def_TitleSonota,
                    OptionInputKb = true,
                    TaxInsInputKb = true,
                    TaxFreeKb = true,
                    TaxCostKb = true,
                    TradeInMilUnit = CommonConst.def_TradeInMilUnitKM,
                    LeaseFlag = logtoken.sesLeaseFlag!
                };
                int intHaiki = CommonFunction.IsNumeric(estModel.DispVol) ? int.Parse(estModel.DispVol) : 0;
                bool flgTaxAutoCalc = _commonFuncHelper.enableTaxCalc(model.MakerName!);
                int intFirstMonth = Convert.ToInt32(string.Format("{0:D2}", DateTime.Now.Month));
                if (flgTaxAutoCalc && intHaiki > 0)
                {
                    var carTax = _commonFuncHelper.getCarTax(intFirstMonth, intHaiki);
                    if (carTax == -1)
                        return ResponseHelper.Error<ResponseEstMainModel>("Error", CommonConst.def_ErrMsg1_Maker + CommonConst.def_ErrCodeL + "SMAI-023D" + CommonConst.def_ErrCodeR);

                    estModel.AutoTax = carTax;
                    estModel.AutoTaxMonth = intFirstMonth.ToString();
                }
                int userDefDamageInsMonth = 0;
                var dtUserDef = _commonFuncHelper.getUserDefData(valToken.UserNo!);
                if (dtUserDef != null)
                {
                    estModel.EstUserNo = dtUserDef.UserNo;
                    estModel.ConTaxInputKb = dtUserDef.ConTaxInputKb;
                    estModel.ShopNm = dtUserDef.ShopNm;
                    estModel.ShopAdr = dtUserDef.ShopAdr;
                    estModel.ShopTel = dtUserDef.ShopTel;
                    estModel.EstTanName = dtUserDef.EstTanName;
                    estModel.SekininName = dtUserDef.SekininName;
                    userDefDamageInsMonth = Convert.ToInt32(dtUserDef.DamageInsMonth);
                    bool isIntHaiki = intHaiki > 0 && intHaiki <= 660;
                    estModel.SyakenZok = 0;
                    estModel.SyakenNew = isIntHaiki ? dtUserDef.SyakenNewK : dtUserDef.SyakenNewH;
                    estModel.TaxFreeCheck = isIntHaiki ? dtUserDef.TaxFreeCheckK : dtUserDef.TaxFreeCheckH;
                    estModel.TaxFreeGarage = isIntHaiki ? dtUserDef.TaxFreeGarageK : dtUserDef.TaxFreeGarageH;
                    estModel.TaxCheck = isIntHaiki ? dtUserDef.TaxCheckK : dtUserDef.TaxCheckH;
                    estModel.TaxGarage = isIntHaiki ? dtUserDef.TaxGarageK : dtUserDef.TaxGarageH;
                    estModel.TaxRecycle = isIntHaiki ? dtUserDef.TaxRecycleK : dtUserDef.TaxRecycleH;
                    estModel.TaxDelivery = isIntHaiki ? dtUserDef.TaxDeliveryK : dtUserDef.TaxDeliveryH;
                    estModel.TaxSet1Title = dtUserDef.TaxSet1Title;
                    estModel.TaxSet1 = isIntHaiki ? dtUserDef.TaxSet1K : dtUserDef.TaxSet1H;
                    estModel.TaxSet2Title = dtUserDef.TaxSet2Title;
                    estModel.TaxSet2 = isIntHaiki ? dtUserDef.TaxSet2K : dtUserDef.TaxSet2H;
                    estModel.TaxSet3Title = dtUserDef.TaxSet3Title;
                    estModel.TaxSet3 = isIntHaiki ? dtUserDef.TaxSet3K : dtUserDef.TaxSet3H;
                    estModel.TaxFreeSet1Title = dtUserDef.TaxFreeSet1Title;
                    estModel.TaxFreeSet1 = isIntHaiki ? int.Parse(dtUserDef.TaxFreeSet1K) : dtUserDef.TaxFreeSet1H;
                    estModel.TaxFreeSet2Title = dtUserDef.TaxFreeSet2Title;
                    estModel.TaxFreeSet2 = isIntHaiki ? dtUserDef.TaxFreeSet2K : dtUserDef.TaxFreeSet2H;
                }
                else
                {
                    estModel.ConTaxInputKb = true;
                }
                int intSelfIns = 0; int intRemIns = 0;
                if (flgTaxAutoCalc && intHaiki > 0)
                {
                    if (!_commonFuncHelper.getSelfInsurance(intHaiki, "", "", userDefDamageInsMonth, ref intSelfIns, ref intRemIns))
                    {
                        return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAI013D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI013D));

                    }
                    else if (intSelfIns > 0)
                    {
                        estModel.DamageIns = intSelfIns;
                        estModel.DamageInsMonth = intRemIns.ToString();
                    }
                }
                estModel.YtiRieki = 0;
                estModel.CarPrice = 0;
                estModel.Mode = (byte)(string.IsNullOrEmpty(valToken.sesMode) ? 0 : Convert.ToByte(valToken.sesMode));
                if (!await RegEstData(estModel))
                {
                    return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAI028D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI028D));

                }
                var response = new ResponseEstMainModel
                {
                    EstModel = estModel
                };
                if (string.IsNullOrEmpty(valToken.sesEstNo) || string.IsNullOrEmpty(valToken.sesEstSubNo))
                {
                    return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAI028D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI028D));
                }
                var estData = _commonEst.GetEstData(valToken.sesEstNo, valToken.sesEstSubNo);
                if (estData == null)
                {
                    return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAL041D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

                }
                response.EstModel = estData;
                response.EstCustomerModel = new EstCustomerModel
                {
                    CustNm = valToken.sesCustNm_forPrint ?? "",
                    CustZip = valToken.sesCustZip_forPrint ?? "",
                    CustAdr = valToken.sesCustAdr_forPrint ?? "",
                    CustTel = valToken.sesCustTel_forPrint ?? ""
                };
                response.EstIDEModel = new EstimateIdeModel();
                if (response.EstModel.LeaseFlag == "1")
                {
                    response.EstIDEModel = _commonEst.SetEstIDEData(valToken);
                    if (response.EstIDEModel == null)
                        return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAI028D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI028D));
                }
                SetvalueToken();
                response.AccessToken = valToken.Token!;
                response.EstModel.IsError = 1;// alert("最初に車両本体価格をご確認下さい")
                return ResponseHelper.Ok(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "setFreeEst");
                return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
            }
        }
        public async Task<ResponseBase<string>> AddEstimate(RequestSerEst model, LogToken logToken)
        {
            try
            {
                valToken = logToken;
                var res = await AddEstNextSubNo(model.EstNo!, model.EstSubNo!, true);
                if (res.ResultStatus == (int)enResponse.isSuccess)
                {
                    SetvalueToken();
                    string AccessToken = valToken.Token!;
                    return ResponseHelper.Ok<string>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), AccessToken);
                }
                else
                {
                    return ResponseHelper.Error<string>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddEstimate");
                return ResponseHelper.Error<string>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
            }
        }
        public async Task<ResponseBase<string>> CalcSum(RequestSerEst model, LogToken logToken)
        {
            try
            {
                valToken = logToken;
                valToken.sesEstNo = model.EstNo!;
                valToken.sesEstSubNo = model.EstSubNo!;
                var res = await _commonEst.CalcSum(model.EstNo!, model.EstSubNo!, valToken);
                if (res)
                {
                    SetvalueToken();
                    return ResponseHelper.Ok<string>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), valToken.Token!);
                }
                else
                {
                    return ResponseHelper.Error<string>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CalcSum");
                return ResponseHelper.Error<string>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
            }
        }

        public ResponseBase<int> CheckGoPageLease(string firstRegYm, string makerName, int nowOdometer)
        {
            var LeaseTargetsID2 = _unitOfWorkIDE.LeaseTargets.Query(n => n.Id == 2).FirstOrDefault();
            var LeaseTargetsID1 = _unitOfWorkIDE.LeaseTargets.Query(n => n.Id == 1).FirstOrDefault();
            var year = DateTime.Now.Year;
            var regYear = int.Parse(CommonFunction.Left(firstRegYm, 4));
            var firstYear = regYear + LeaseTargetsID1!.Restriction;
            var zenkaku = StringWidthHelper.ToFullWidth(makerName);
            var arrayMakerName = _dataSettings.MakerName;
            var cmakerName = arrayMakerName.Contains(zenkaku);
            if (nowOdometer > LeaseTargetsID2!.Restriction || firstYear < year || cmakerName == false)
            {
                return ResponseHelper.Ok<int>(HelperMessage.I0003, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0003));
            }
            return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
        }

        public ResponseBase<string> ExportDataCSV(LogToken logToken)
        {
            // 見積書番号を取得
            if (string.IsNullOrEmpty(logToken.sesEstNo) || string.IsNullOrEmpty(logToken.sesEstSubNo))
                return ResponseHelper.Error<string>(HelperMessage.SMAI000S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI000S));

            // 見積書データを取得
            var estData = _commonEst.GetEstData(logToken.sesEstNo, logToken.sesEstSubNo);
            if (estData == null)
                return ResponseHelper.LogicError<string>(HelperMessage.SMAI000D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI000D));

            // CSVファイルを編集
            string strHeading;
            string strOutdata;
            // デリミタ文字列を定義
            string CSV_DELIMITER = ",";

            // （見出し行）
            strHeading = "";
            strHeading += "CSV出力書式版数" + CSV_DELIMITER;
            strHeading += "見積書番号" + CSV_DELIMITER;
            strHeading += "見積書番号 枝番" + CSV_DELIMITER;
            strHeading += "見積入力者ＩＤ" + CSV_DELIMITER;
            strHeading += "見積日" + CSV_DELIMITER;
            strHeading += "お客様名" + CSV_DELIMITER;
            strHeading += "お客様郵便番号" + CSV_DELIMITER;
            strHeading += "お客様住所" + CSV_DELIMITER;
            strHeading += "お客様電話番号" + CSV_DELIMITER;
            strHeading += "お客様カナ名" + CSV_DELIMITER;
            strHeading += "メモ" + CSV_DELIMITER;
            strHeading += "備考" + CSV_DELIMITER;
            strHeading += "販売店名" + CSV_DELIMITER;
            strHeading += "販売店住所" + CSV_DELIMITER;
            strHeading += "販売店電話番号" + CSV_DELIMITER;
            strHeading += "見積担当者" + CSV_DELIMITER;
            strHeading += "責任者" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "メーカー名" + CSV_DELIMITER;
            strHeading += "車名" + CSV_DELIMITER;
            strHeading += "グレード" + CSV_DELIMITER;
            strHeading += "型式" + CSV_DELIMITER;
            strHeading += "車台番号" + CSV_DELIMITER;
            strHeading += "初年度登録" + CSV_DELIMITER;
            strHeading += "車検" + CSV_DELIMITER;
            strHeading += "走行距離" + CSV_DELIMITER;
            strHeading += "走行距離単位" + CSV_DELIMITER;
            strHeading += "修復歴" + CSV_DELIMITER;
            strHeading += "車歴" + CSV_DELIMITER;
            strHeading += "シフト" + CSV_DELIMITER;
            strHeading += "排気量" + CSV_DELIMITER;
            strHeading += "排気量単位" + CSV_DELIMITER;
            strHeading += "色" + CSV_DELIMITER;
            strHeading += "装備" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "課税対象金額の入力方法" + CSV_DELIMITER;

            string titleTax = estData.ConTaxInputKb ? CommonConst.def_TitleInTax : CommonConst.def_TitleOutTax;
            strHeading += CommonConst.def_TitleCarPrice + titleTax + CSV_DELIMITER;
            strHeading += CommonConst.def_TitleDisCount + titleTax + CSV_DELIMITER;
            strHeading += estData.SonotaTitle + titleTax + CSV_DELIMITER; // [その他費用タイトル]
            strHeading += "落札料" + CSV_DELIMITER;
            strHeading += "陸送代" + CSV_DELIMITER;
            string titleInOutTax = estData.ConTaxInputKb ? CommonConst.def_TitleInTax : CommonConst.def_TitleOutTax;
            strHeading += SetSyakenNewZokT(estData) + titleInOutTax + CSV_DELIMITER; // 車検整備費用／納車整備費用
            strHeading += CommonConst.def_TitleOpSpeCial + titleTax + CSV_DELIMITER;
            strHeading += estData.OptionName1 + CSV_DELIMITER;
            strHeading += estData.OptionName2 + CSV_DELIMITER;
            strHeading += estData.OptionName3 + CSV_DELIMITER;
            strHeading += estData.OptionName4 + CSV_DELIMITER;
            strHeading += estData.OptionName5 + CSV_DELIMITER;
            strHeading += estData.OptionName6 + CSV_DELIMITER;
            strHeading += estData.OptionName7 + CSV_DELIMITER;
            strHeading += estData.OptionName8 + CSV_DELIMITER;
            strHeading += estData.OptionName9 + CSV_DELIMITER;
            strHeading += estData.OptionName10 + CSV_DELIMITER;
            strHeading += estData.OptionName11 + CSV_DELIMITER;
            strHeading += estData.OptionName12 + CSV_DELIMITER;
            strHeading += "車両販売価格" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "リザーブ" + CSV_DELIMITER;
            strHeading += "税金・保険料（非課税）" + CSV_DELIMITER;
            strHeading += CommonConst.def_TitleAutoTax + CSV_DELIMITER;
            strHeading += "自動車税基準月" + CSV_DELIMITER;
            strHeading += "環境性能割" + CSV_DELIMITER;
            strHeading += "重量税" + CSV_DELIMITER;
            strHeading += CommonConst.def_TitleDamageIns + CSV_DELIMITER;
            strHeading += "自賠責保険基準月数" + CSV_DELIMITER;
            strHeading += "任意保険料" + CSV_DELIMITER;
            strHeading += CommonConst.def_TitleTaxInsEquivalent + titleTax + CSV_DELIMITER;
            strHeading += CommonConst.def_TitleAutoTaxEquivalent + CSV_DELIMITER;
            strHeading += CommonConst.def_TitleDamageInsEquivalent + CSV_DELIMITER;
            strHeading += "預り法定費用（非課税）" + CSV_DELIMITER;
            strHeading += "検査登録" + CSV_DELIMITER;
            strHeading += "車庫証明" + CSV_DELIMITER;
            strHeading += "下取車手続・処分" + CSV_DELIMITER;
            strHeading += "リサイクル預託金" + CSV_DELIMITER;
            strHeading += (estData.TaxFreeKb ? estData.TaxFreeSet1Title : "") + CSV_DELIMITER; // [項目1タイトル]
            strHeading += (estData.TaxFreeKb ? estData.TaxFreeSet2Title : "") + CSV_DELIMITER; // [項目2タイトル]
            strHeading += "その他非課税費用" + CSV_DELIMITER;
            strHeading += CommonConst.def_TitleDaiko + titleTax + CSV_DELIMITER;
            strHeading += "検査登録手続" + CSV_DELIMITER;
            strHeading += "車庫証明手続" + CSV_DELIMITER;
            strHeading += "下取車手続・処分" + CSV_DELIMITER;
            strHeading += "下取車査定料" + CSV_DELIMITER;
            strHeading += "資金管理料金" + CSV_DELIMITER;
            strHeading += "納車費用" + CSV_DELIMITER;
            strHeading += (estData.TaxCostKb ? estData.TaxSet1Title : "") + CSV_DELIMITER; // [項目1タイトル]
            strHeading += (estData.TaxCostKb ? estData.TaxSet2Title : "") + CSV_DELIMITER; // [項目2タイトル]
            strHeading += (estData.TaxCostKb ? estData.TaxSet3Title : "") + CSV_DELIMITER; // [項目3タイトル]
            strHeading += "その他費用" + CSV_DELIMITER;
            strHeading += (estData.ConTaxInputKb ? CommonConst.def_TitleConTaxTotalInTax : CommonConst.def_TitleConTaxTotalOutTax) + CSV_DELIMITER;
            strHeading += formatCsvTitle(CommonConst.def_TitleCarKei) + CSV_DELIMITER;

            strHeading += "下取車価格" + CSV_DELIMITER;
            strHeading += "下取車残債" + CSV_DELIMITER;
            strHeading += "下取車有無" + CSV_DELIMITER;
            strHeading += "下取車車名" + CSV_DELIMITER;
            strHeading += "下取車初年度登録" + CSV_DELIMITER;
            strHeading += "下取車車検" + CSV_DELIMITER;
            strHeading += "下取車走行距離" + CSV_DELIMITER;
            strHeading += "下取車走行距離単位" + CSV_DELIMITER;
            strHeading += "下取車車台番号" + CSV_DELIMITER;
            strHeading += "下取車登録NO" + CSV_DELIMITER;
            strHeading += "下取車色" + CSV_DELIMITER;
            strHeading += formatCsvTitle(CommonConst.def_TitleSalesSumOutTax) + CSV_DELIMITER;
            strHeading += "金利" + CSV_DELIMITER;
            strHeading += "頭金" + CSV_DELIMITER;
            strHeading += "現金・割賦元金" + CSV_DELIMITER;
            strHeading += "分割払手数料" + CSV_DELIMITER;
            strHeading += "分割支払金合計" + CSV_DELIMITER;
            strHeading += "支払回数" + CSV_DELIMITER;
            strHeading += "初回支払月" + CSV_DELIMITER;
            strHeading += "最終回支払月" + CSV_DELIMITER;
            strHeading += "第1回目分割支払金" + CSV_DELIMITER;
            strHeading += "第2回目以降分割支払金" + CSV_DELIMITER;
            strHeading += "ボーナス月加算額" + CSV_DELIMITER;
            strHeading += "ボーナス支払月1" + CSV_DELIMITER;
            strHeading += "ボーナス支払月2" + CSV_DELIMITER;
            strHeading += "ボーナス回数" + CSV_DELIMITER;
            strHeading += "\r\n";

            // （データ行）
            strOutdata = "";
            strOutdata += "ASEST-Ver01-1" + CSV_DELIMITER;
            // -- 見積書番号
            strOutdata += logToken.sesEstNo + CSV_DELIMITER;
            // -- 見積書番号 枝番
            strOutdata += logToken.sesEstSubNo + CSV_DELIMITER;
            // -- 見積入力者ＩＤ
            strOutdata += estData.EstUserNo + CSV_DELIMITER;
            // -- 見積日
            strOutdata += estData.TradeDate.ToString("yyyyMMdd") + CSV_DELIMITER;
            // -- お客様名
            strOutdata += formatCsvItem(logToken.sesCustNm_forPrint!) + CSV_DELIMITER;
            // -- お客様郵便番号
            strOutdata += formatCsvItem(logToken.sesCustZip_forPrint!) + CSV_DELIMITER;
            // -- お客様住所
            strOutdata += formatCsvItem(logToken.sesCustAdr_forPrint!) + CSV_DELIMITER;
            // -- お客様電話番号
            strOutdata += formatCsvItem(logToken.sesCustTel_forPrint!) + CSV_DELIMITER;
            // -- お客様カナ名
            strOutdata += formatCsvItem(estData.CustKname) + CSV_DELIMITER;
            // -- メモ
            strOutdata += formatCsvItem(estData.CustMemo) + CSV_DELIMITER;
            // -- 備考
            strOutdata += formatCsvItem(estData.Notes) + CSV_DELIMITER;
            // -- 販売店名
            strOutdata += formatCsvItem(estData.ShopNm) + CSV_DELIMITER;
            // -- 販売店住所
            strOutdata += formatCsvItem(estData.ShopAdr) + CSV_DELIMITER;
            // -- 販売店電話番号
            strOutdata += formatCsvItem(estData.ShopTel) + CSV_DELIMITER;
            // -- 見積担当者
            strOutdata += formatCsvItem(estData.EstTanName) + CSV_DELIMITER;
            // -- 責任者
            strOutdata += formatCsvItem(estData.SekininName) + CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- メーカー名
            strOutdata += formatCsvItem(estData.MakerName) + CSV_DELIMITER;
            // -- 車名
            strOutdata += formatCsvItem(estData.ModelName) + CSV_DELIMITER;
            // -- グレード
            strOutdata += formatCsvItem(estData.GradeName) + CSV_DELIMITER;
            // -- 型式
            strOutdata += formatCsvItem(estData.Case) + CSV_DELIMITER;
            // -- 車台番号
            strOutdata += formatCsvItem(estData.ChassisNo) + CSV_DELIMITER;
            // -- 初年度登録
            strOutdata += formatCsvItem(estData.FirstRegYm) + CSV_DELIMITER;
            // -- 車検
            strOutdata += formatCsvItem(estData.CheckCarYm) + CSV_DELIMITER;
            // -- 走行距離、走行距離単位
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.NowOdometer)) + CSV_DELIMITER;
            strOutdata += (string.IsNullOrEmpty(estData.NowOdometer.ToString()) ? "" : formatCsvItem(estData.MilUnit)) + CSV_DELIMITER;
            // -- 修復歴
            string csvAccidentHis = estData.AccidentHis == 0 ? "無し" : estData.AccidentHis == 1 ? "有り" : "";
            strOutdata += csvAccidentHis + CSV_DELIMITER;
            // -- 車歴
            strOutdata += formatCsvItem(estData.BusinessHis) + CSV_DELIMITER;
            // -- シフト
            strOutdata += formatCsvItem(estData.Mission) + CSV_DELIMITER;
            // -- 排気量、排気量単位
            strOutdata += formatCsvItem(estData.DispVol) + CSV_DELIMITER;
            strOutdata += (string.IsNullOrEmpty(estData.DispVol) ? "" : estData.DispVolUnit) + CSV_DELIMITER;
            // -- 色
            strOutdata += formatCsvItem(estData.BodyColor) + CSV_DELIMITER;
            // -- 装備
            strOutdata += formatCsvItem(estData.Equipment) + CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- 課税対象金額の入力方法
            strOutdata += (estData.ConTaxInputKb ? "1" : "0") + CSV_DELIMITER;
            // -- 車両本体価格（税込）／（税抜）
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.CarPrice)) + CSV_DELIMITER;
            // -- 値引き（税込）／（税抜）
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.Discount)) + CSV_DELIMITER;
            // -- [その他費用]金額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.Sonota)) + CSV_DELIMITER;
            // -- 落札料
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.RakuSatu)) + CSV_DELIMITER;
            // -- 陸送代
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.Rikusou)) + CSV_DELIMITER;
            // -- 車検整備費用／納車整備費用（税込）／（税抜）
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.SyakenNew)) + CSV_DELIMITER;
            // -- 付属品・特別仕様（税込）／（税抜）
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionPriceAll)) + CSV_DELIMITER;
            // -- [品名1]金額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionPrice1)) + CSV_DELIMITER;
            // -- [品名2]金額             
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionPrice2)) + CSV_DELIMITER;
            // -- [品名3]金額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionPrice3)) + CSV_DELIMITER;
            // -- [品名4]金額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionPrice4)) + CSV_DELIMITER;
            // -- [品名5]金額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionPrice5)) + CSV_DELIMITER;
            // -- [品名6]金額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionPrice6)) + CSV_DELIMITER;
            // -- [品名7]金額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionPrice7)) + CSV_DELIMITER;
            // -- [品名8]金額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionPrice8)) + CSV_DELIMITER;
            // -- [品名9]金額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionPrice9)) + CSV_DELIMITER;
            // -- [品名10]金額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionPrice10)) + CSV_DELIMITER;
            // -- [品名11]金額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionPrice11)) + CSV_DELIMITER;
            // -- [品名12]金額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionPrice12)) + CSV_DELIMITER;
            // -- 車両販売価格
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.CarSum)) + CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- リザーブ
            strOutdata += CSV_DELIMITER;
            // -- 税金・保険料（非課税）
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxInsAll)) + CSV_DELIMITER;
            // -- 自動車税、自動車税基準月
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.AutoTax)) + CSV_DELIMITER;
            strOutdata += (estData.AutoTax == 0 ? "" : formatCsvItem(estData.AutoTaxMonth)) + CSV_DELIMITER;
            // -- 取得税
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.AcqTax)) + CSV_DELIMITER;
            // -- 重量税
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.WeightTax)) + CSV_DELIMITER;
            // -- 自賠責保険料、自賠責保険基準月数
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.DamageIns)) + CSV_DELIMITER;
            strOutdata += (estData.DamageIns == 0 ? "" : formatCsvItem(estData.DamageInsMonth)) + CSV_DELIMITER;
            // -- 任意保険料
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.OptionIns)) + CSV_DELIMITER;
            // -- 税金・保険料相当額（税込）／（税抜）
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxInsEquivalentAll)) + CSV_DELIMITER;
            // -- 自動車税相当額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.AutoTaxEquivalent)) + CSV_DELIMITER;
            // -- 自賠責保険料相当額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.DamageInsEquivalent)) + CSV_DELIMITER;
            // -- 預り法定費用(非課税)
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxFreeAll)) + CSV_DELIMITER;
            // -- 検査登録
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxFreeCheck)) + CSV_DELIMITER;
            // -- 車庫証明
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxFreeGarage)) + CSV_DELIMITER;
            // -- 下取車手続・処分
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxFreeTradeIn)) + CSV_DELIMITER;
            // -- リサイクル預託金
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxFreeRecycle)) + CSV_DELIMITER;
            // -- [項目1]費用
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxFreeSet1)) + CSV_DELIMITER;
            // -- [項目2]費用
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxFreeSet2)) + CSV_DELIMITER;
            // -- その他非課税費用
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxFreeOther)) + CSV_DELIMITER;
            // -- 手続代行費用（税込）／（税抜）
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxCostAll)) + CSV_DELIMITER;
            // -- 検査登録手続
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxCheck)) + CSV_DELIMITER;
            // -- 車庫証明手続
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxGarage)) + CSV_DELIMITER;
            // -- 下取車手続・処分
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxTradeIn)) + CSV_DELIMITER;
            // -- 下取車査定料
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxTradeInSatei)) + CSV_DELIMITER;
            // -- 資金管理料金
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxRecycle)) + CSV_DELIMITER;
            // -- 納車費用
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxDelivery)) + CSV_DELIMITER;
            // -- [項目1]費用
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxSet1)) + CSV_DELIMITER;
            // -- [項目2]費用
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxSet2)) + CSV_DELIMITER;
            // -- [項目3]費用
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxSet3)) + CSV_DELIMITER;
            // -- その他費用
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TaxOther)) + CSV_DELIMITER;
            // -- （内消費税合計）／消費税合計
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.ConTax)) + CSV_DELIMITER;
            // -- 現金販売価格
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.CarSaleSum)) + CSV_DELIMITER;
            // -- 下取車価格
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TradeInPrice)) + CSV_DELIMITER;
            // -- 下取車残債
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.Balance)) + CSV_DELIMITER;
            // -- 下取車有無
            if (estData.TradeInUm > 0)
            {
                strOutdata += "1" + CSV_DELIMITER;
                // -- 下取車車名
                strOutdata += formatCsvItem(estData.TradeInCarName) + CSV_DELIMITER;
                // -- 下取車初年度登録
                strOutdata += formatCsvItem(estData.TradeInFirstRegYm) + CSV_DELIMITER;
                // -- 下取車車検
                strOutdata += formatCsvItem(estData.TradeInCheckCarYm) + CSV_DELIMITER;
                // -- 下取車走行距離、下取車走行距離単位
                strOutdata += formatCsvItem(CommonFunction.FormatString(estData.TradeInNowOdometer)) + CSV_DELIMITER;
                strOutdata += (estData.TradeInNowOdometer > 0 ? formatCsvItem(estData.TradeInMilUnit) : "") + CSV_DELIMITER;
                // -- 下取車登録No
                strOutdata += formatCsvItem(estData.TradeInChassisNo) + CSV_DELIMITER;
                // -- 下取車車台番号
                strOutdata += formatCsvItem(estData.TradeInRegNo.Replace("/", "")) + CSV_DELIMITER;
                // -- 下取車色
                strOutdata += formatCsvItem(estData.TradeInBodyColor) + CSV_DELIMITER;
            }
            else
            {
                strOutdata += "0" + CSV_DELIMITER;
                for (int i = 1; i <= 8; i++)
                    strOutdata += CSV_DELIMITER;
            }

            // -- お支払総額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.SalesSum)) + CSV_DELIMITER;
            // -- （ローン計算情報）金利
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.Rate)) + CSV_DELIMITER;
            // -- （ローン計算情報）頭金
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.Deposit)) + CSV_DELIMITER;
            // -- （ローン計算情報）現金・割賦元金     
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.SalesSum - estData.Deposit)) + CSV_DELIMITER;
            // -- （ローン計算情報）分割払手数料
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.PartitionFee)) + CSV_DELIMITER;
            // -- （ローン計算情報）分割支払金合計
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.PartitionAmount)) + CSV_DELIMITER;
            // -- （ローン計算情報）支払回数
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.PayTimes)) + CSV_DELIMITER;
            // -- （ローン計算情報）初回支払月
            strOutdata += formatCsvItem(estData.FirstPayMonth) + CSV_DELIMITER;
            // -- （ローン計算情報）最終回支払月
            strOutdata += formatCsvItem(estData.LastPayMonth) + CSV_DELIMITER;
            // -- （ローン計算情報）第1回目分割支払金
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.FirstPayAmount)) + CSV_DELIMITER;
            // -- （ローン計算情報）第2回目以降分割支払金
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.PayAmount)) + CSV_DELIMITER;
            // -- （ローン計算情報）ボーナス月加算額
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.BonusAmount)) + CSV_DELIMITER;
            // -- （ローン計算情報）ボーナス支払月1
            strOutdata += formatCsvItem(estData.BonusFirst) + CSV_DELIMITER;
            // -- （ローン計算情報）ボーナス支払月2
            strOutdata += formatCsvItem(estData.BonusSecond) + CSV_DELIMITER;
            // -- （ローン計算情報）ボーナス回数
            strOutdata += formatCsvItem(CommonFunction.FormatString(estData.BonusTimes)) + CSV_DELIMITER;
            strOutdata += "\r\n";

            string data = strHeading + strOutdata;

            return ResponseHelper.Ok(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data);
        }


        public async Task<ResponseBase<int>> UpdateJiko(RequestUpdateJiko model)
        {
            try
            {
                TEstimate dtEstimates = _unitOfWork.Estimates.GetSingle(n => n.EstNo == model.EstNo && n.EstSubNo == model.EstSubNo && n.Dflag == false);
                if (dtEstimates == null)
                {
                    return ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                dtEstimates.AccidentHis = Convert.ToByte(model.raJrk);
                _unitOfWork.Estimates.Update(dtEstimates);
                await _unitOfWork.CommitAsync();
                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "updateJiko");
                return ResponseHelper.Error<int>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        #region fuc private     
        private int ChkAANo(string? userNo, string AANo, string AAPlace, int CornerType, int mode)
        {
            try
            {
                var dtMaxEstSub = (from sub in _unitOfWork.DbContext.TEstimateSubs
                                   join sys in _unitOfWork.DbContext.TbSys
                                   on new { sub.Corner, sub.Aacount } equals
                                      new { sys.Corner, sys.Aacount }
                                   into x
                                   from joinGroup in x.DefaultIfEmpty()
                                   where sub.EstUserNo == userNo &&
                                          sub.Aano == AANo &&
                                          sub.Aaplace == AAPlace &&
                                          joinGroup.CornerType == CornerType &&
                                          sub.Mode == mode &&
                                          sub.Dflag == false
                                   group sub by sub.EstNo into g
                                   select new
                                   {
                                       maxEstNo = g.Max(x => x.EstNo),
                                       maxEstSubNo = g.Max(x => x.EstSubNo),
                                   }).FirstOrDefault();

                if (dtMaxEstSub != null)
                {
                    valToken.sesEstNo = dtMaxEstSub!.maxEstNo;
                    valToken.sesEstSubNo = dtMaxEstSub.maxEstSubNo;
                }
                else
                {
                    return 0;
                }

                return 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "chkAANo " + "GCMF-040D");
                return -1;
            }
        }

        private async Task<ResponseBase<EstModel>> AddEstNextSubNo(string estNo, string estSubNo, bool flgRecreate = false)
        {
            try
            {
                if (!await _commonEst.CalcSum(estNo, estSubNo, valToken))
                    return ResponseHelper.LogicError<EstModel>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));

                var estData = _commonEst.GetEstData(estNo, estSubNo);
                if (estData == null)
                    return ResponseHelper.LogicError<EstModel>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));

                if (flgRecreate)
                {
                    estNo = "";
                    if (!_commonEst.GetEstNoFromDb(ref estNo))
                        return ResponseHelper.LogicError<EstModel>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
                }

                string vNextSubNo = "";
                if (!_commonEst.GetEstSubNoFromDb(estNo, ref vNextSubNo))
                    return ResponseHelper.LogicError<EstModel>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));

                TEstimate entityEst = new();
                entityEst = _mapper.Map<TEstimate>(estData);
                entityEst.EstNo = estNo;
                entityEst.EstSubNo = vNextSubNo;
                entityEst.TradeDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
                entityEst.OptionInputKb = true;
                entityEst.TaxInsInputKb = true;
                entityEst.TaxFreeKb = true;
                entityEst.TaxCostKb = true;
                entityEst.Rdate = DateTime.Now;
                entityEst.Udate = DateTime.Now;
                entityEst.Dflag = false;
                TEstimateSub entityEstSub = new();
                entityEstSub = _mapper.Map<TEstimateSub>(estData);
                entityEstSub.EstNo = estNo;
                entityEstSub.EstSubNo = vNextSubNo;
                entityEstSub.Rdate = DateTime.Now;
                entityEstSub.Udate = DateTime.Now;
                entityEstSub.Dflag = false;
                _unitOfWork.Estimates.Add(entityEst);
                _unitOfWork.EstimateSubs.Add(entityEstSub);
                await _unitOfWork.CommitAsync();
                valToken.sesEstNo = estNo;
                valToken.sesEstSubNo = vNextSubNo;

                estData.EstNo = estNo;
                estData.EstSubNo = vNextSubNo;

                return ResponseHelper.Ok<EstModel>("", "", estData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "addEstNextSubNo " + "CEST-052D");
                return ResponseHelper.LogicError<EstModel>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
            }
        }

        private async Task<ResponseBase<EstModel>> GetAsnetInfo(RequestHeaderModel request)
        {
            try
            {
                var estModel = new EstModel();
                string leaseFlag = string.IsNullOrEmpty(request.leaseFlag) ? "0" : request.leaseFlag;
                bool isCheck = string.IsNullOrEmpty(request.cot) || string.IsNullOrEmpty(request.cna) || string.IsNullOrEmpty(request.mem);
                if (isCheck)
                    return ResponseHelper.Error<EstModel>(HelperMessage.SMAI020P, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI020P));

                string strTempImagePath;
                string strSavePath = "";
                estModel.LeaseFlag = leaseFlag;
                var userInfo = getUserInfo(request.mem);
                valToken.UserNo = userInfo.Data!.UserNo;
                valToken.UserNm = userInfo.Data!.UserNm;
                valToken.sesLeaseFlag = leaseFlag;
                if (!string.IsNullOrEmpty(request.exh))
                {
                    string wAANo = request.exh!;
                    string wAAPlace = request.aan!;
                    string wConnerType = request.cot!;
                    string wMode = request.Mode!;
                    var checkAANo = ChkAANo(userInfo.Data.UserNo, wAANo, wAAPlace, int.Parse(wConnerType), int.Parse(wMode));
                    if (checkAANo == 1)
                    {
                        // check condition addEstNextSubNo 
                        var result = await AddEstNextSubNo(valToken.sesEstNo!, valToken.sesEstSubNo!);
                        if (result.ResultStatus == (int)enResponse.isLogicError)
                            return ResponseHelper.Error<EstModel>(HelperMessage.CEST051D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST051D));
                        else if (result.ResultStatus == (int)enResponse.isError)
                            return ResponseHelper.LogicError<EstModel>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
                        else
                            return ResponseHelper.Ok<EstModel>("", "", result.Data!);
                    }
                    else if (checkAANo == -1)
                        return ResponseHelper.Error<EstModel>(HelperMessage.SMAI014D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI014D));
                }
                estModel.EstUserNo = userInfo.Data.UserNo;
                estModel.CallKbn = request.cot == "2" || request.cot == "5" ? "1" : "2";
                int vAAcount = request.cot == "1" || request.cot == "2" ? _commonFuncHelper.GetAACount(request.cor) : 0;
                estModel.EstInpKbn = "1";
                estModel.TradeDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
                estModel.MakerName = request.mak;
                estModel.ModelName = request.cna;
                estModel.GradeName = request.gra;
                estModel.Case = request.carCase;
                estModel.ChassisNo = request.pla;
                string vNensiki = request.mod.Trim();
                estModel.FirstRegYm = CommonFunction.IsNumeric(vNensiki) ? vNensiki : "";
                string strCheckCarYm = CommonFunction.setCheckCarYm(request.ins);
                estModel.CheckCarYm = strCheckCarYm;
                estModel.NowOdometer = CommonFunction.IsNumeric(request.mil) ? int.Parse(request.mil) : 0;
                bool isCheckMilUnit = string.IsNullOrEmpty(request.milUnit);
                estModel.MilUnit = isCheckMilUnit ? CommonConst.def_MilUnitTKM : request.milUnit;
                estModel.DispVol = string.IsNullOrEmpty(request.vol) ? "" : request.vol.Trim().Replace("cc", ")");
                estModel.DispVolUnit = string.IsNullOrEmpty(request.volUnit) ? CommonConst.def_DispVolUnitCC : request.volUnit.ToLower();
                estModel.Mission = request.shi;
                estModel.AccidentHis = 2;
                estModel.BusinessHis = request.his;
                estModel.FuelName = request.FuelName;
                estModel.DriveName = request.DriveName;
                estModel.CarDoors = CommonFunction.IsNumeric(request.CarDoors) ? int.Parse(request.CarDoors) : 0;
                estModel.BodyName = CommonFunction.StandardlizeBodyName(request.BodyName);
                estModel.Capacity = CommonFunction.IsNumeric(request.Capacity) ? int.Parse(request.Capacity) : 0;
                estModel.Equipment = request.equ;
                estModel.BodyColor = request.col;
                string wCarImgPath = request.img;
                string outImg = "";
                string outImg1 = "";
                string outImg2 = ""; string outImg3 = ""; string outImg4 = ""; string outImg5 = ""; string outImg6 = ""; string outImg7 = ""; string outImg8 = "";
                if (string.IsNullOrEmpty(wCarImgPath))
                {
                    estModel.CarImgPath = _jwtPhysicalSettings.DmyImg;
                }
                else
                {
                    var isUrlImg = await _commonFuncHelper.CheckUrlImg(wCarImgPath);
                    if (isUrlImg == true)
                    {
                        strTempImagePath = wCarImgPath.ToUpper();
                        if (!strTempImagePath.EndsWith(".JPG") && !strTempImagePath.EndsWith(".GIF") && !strTempImagePath.EndsWith(".PNG"))
                        {
                            strSavePath = request.cor + request.fex + "001.jpg";
                        }

                        _commonFuncHelper.DownloadImg(wCarImgPath, valToken.sesCarImgPath!, _jwtPhysicalSettings.DmyImg, ref outImg, strSavePath);
                        estModel.CarImgPath = outImg;
                        _commonFuncHelper.CheckImgPath(request.img1, valToken.sesCarImgPath1!, "", ref outImg1, "201.jpg", request.cor, request.fex);
                        _commonFuncHelper.CheckImgPath(request.img2, valToken.sesCarImgPath2!, "", ref outImg2, "202.jpg", request.cor, request.fex);
                        _commonFuncHelper.CheckImgPath(request.img3, valToken.sesCarImgPath3!, "", ref outImg3, "203.jpg", request.cor, request.fex);
                        _commonFuncHelper.CheckImgPath(request.img4, valToken.sesCarImgPath4!, "", ref outImg4, "204.jpg", request.cor, request.fex);
                        _commonFuncHelper.CheckImgPath(request.img5, valToken.sesCarImgPath5!, "", ref outImg5, "205.jpg", request.cor, request.fex);
                        _commonFuncHelper.CheckImgPath(request.img6, valToken.sesCarImgPath6!, "", ref outImg6, "206.jpg", request.cor, request.fex);
                        _commonFuncHelper.CheckImgPath(request.img7, valToken.sesCarImgPath7!, "", ref outImg7, "207.jpg", request.cor, request.fex);
                        _commonFuncHelper.CheckImgPath(request.img8, valToken.sesCarImgPath8!, "", ref outImg8, "208.jpg", request.cor, request.fex);
                        estModel.CarImgPath1 = outImg1;
                        estModel.CarImgPath2 = outImg2;
                        estModel.CarImgPath3 = outImg3;
                        estModel.CarImgPath4 = outImg4;
                        estModel.CarImgPath5 = outImg5;
                        estModel.CarImgPath6 = outImg6;
                        estModel.CarImgPath7 = outImg7;
                        estModel.CarImgPath8 = outImg8;
                    }
                }

                estModel.TotalCost = 0;
                decimal intCarPrice = CommonFunction.IsNumeric(request.pri) ? Convert.ToDecimal(request.pri) : 0m;
                estModel.RakuSatu = CommonFunction.IsNumeric(request.fee) ? request.cor == "F6" ? 25000 : int.Parse(request.fee) : 0;
                estModel.Rikusou = CommonFunction.IsNumeric(request.tra) ? int.Parse(request.tra) : 0;
                estModel.OptionInputKb = true;
                estModel.TaxInsInputKb = true;
                estModel.TaxFreeKb = true;
                if (!string.IsNullOrEmpty(request.fex))
                {
                    estModel.TaxFreeRecycle = _commonFuncHelper.GetRecDeposit(request.cor, request.fex);
                }
                estModel.TaxCostKb = true;
                estModel.TradeInMilUnit = CommonConst.def_TradeInMilUnitKM;
                estModel.ShopNm = userInfo.Data.UserNm;
                estModel.ShopAdr = userInfo.Data.UserAdr;
                estModel.ShopTel = userInfo.Data.UserTel;
                estModel.Corner = request.cor;
                estModel.Aacount = vAAcount;
                estModel.Mode = Convert.ToByte(request.Mode);
                estModel.Aayear = vNensiki;
                estModel.Aahyk = CommonFunction.IsNumeric(request.poi) ? request.poi : "0";
                estModel.Aaprice = (int)intCarPrice;
                estModel.SirPrice = estModel.CarPrice;
                estModel.YtiRieki = 0;
                estModel.Aaplace = request.aan;
                estModel.Aano = request.exh;
                if (!string.IsNullOrEmpty(request.lim))
                {
                    string vAATime = request.lim.Trim();
                    estModel.Aatime = request.lim.Trim().Length == 8 ? CommonFunction.Left(vAATime, 4) + "/" + CommonFunction.Mid(vAATime, 4, 2) + "/" + CommonFunction.Right(vAATime, 2) : vAATime;

                }
                int intHaiki = 0;
                if (CommonFunction.IsNumeric(estModel.DispVol))
                {
                    intHaiki = int.Parse(estModel.DispVol);
                }
                bool flgTaxAutoCalc = _commonFuncHelper.enableTaxCalc(request.mak);
                int intFirstMonth = Convert.ToInt32(string.Format("{0:D2}", DateTime.Now.Month));
                if (flgTaxAutoCalc && intHaiki > 0 && estModel.DispVolUnit == CommonConst.def_DispVolUnitCC)
                {
                    var carTax = _commonFuncHelper.getCarTax(intFirstMonth, intHaiki);
                    if (carTax == -1)
                        return ResponseHelper.Error<EstModel>(HelperMessage.SMAI023D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI023D));
                    estModel.AutoTax = carTax;
                    estModel.AutoTaxMonth = intFirstMonth.ToString();
                }
                int userDefDamageInsMonth = 0;
                var dtUserDef = _commonFuncHelper.getUserDefData(valToken.UserNo);
                if (dtUserDef != null)
                {
                    estModel.ConTaxInputKb = dtUserDef.ConTaxInputKb;
                    estModel.ShopNm = dtUserDef.ShopNm;
                    estModel.ShopAdr = dtUserDef.ShopAdr;
                    estModel.ShopTel = dtUserDef.ShopTel;
                    estModel.EstTanName = dtUserDef.EstTanName;
                    estModel.SekininName = dtUserDef.SekininName;
                    userDefDamageInsMonth = Convert.ToInt32(dtUserDef.DamageInsMonth);
                    bool isIntHaiki = intHaiki > 0 && intHaiki <= 660;
                    if ((string.IsNullOrEmpty(valToken.sesMode) ? int.Parse(valToken.sesMode!) : 0) == 0)
                    {
                        estModel.YtiRieki = isIntHaiki ? dtUserDef.YtiRiekiK : dtUserDef.YtiRiekiH;
                    }
                    estModel.SyakenNew = isIntHaiki ? dtUserDef.SyakenNewK : dtUserDef.SyakenNewH;
                    estModel.SyakenZok = 0;
                    if ((strCheckCarYm.Length == 6 && CommonFunction.DateDiff(IntervalEnum.Months, DateTime.Today, DateTime.Parse(CommonFunction.Left(strCheckCarYm, 4) + "/" + CommonFunction.Right(strCheckCarYm, 2) + "/01")) > 0)
                        || (strCheckCarYm.Length == 4 && CommonFunction.DateDiff(IntervalEnum.Years, DateTime.Today, DateTime.Parse(strCheckCarYm + "/01")) > 0))
                    {
                        estModel.SyakenNew = 0;
                        estModel.SyakenZok = isIntHaiki ? dtUserDef.SyakenZokK : dtUserDef.SyakenZokH;
                    }
                    estModel.TaxFreeCheck = isIntHaiki ? dtUserDef.TaxFreeCheckK : dtUserDef.TaxFreeCheckH;
                    estModel.TaxFreeGarage = isIntHaiki ? dtUserDef.TaxFreeGarageK : dtUserDef.TaxFreeGarageH;
                    estModel.TaxCheck = isIntHaiki ? dtUserDef.TaxCheckK : dtUserDef.TaxCheckH;
                    estModel.TaxGarage = isIntHaiki ? dtUserDef.TaxGarageK : dtUserDef.TaxGarageH;
                    estModel.TaxRecycle = isIntHaiki ? dtUserDef.TaxRecycleK : dtUserDef.TaxRecycleH;
                    estModel.TaxDelivery = isIntHaiki ? dtUserDef.TaxDeliveryK : dtUserDef.TaxDeliveryH;
                    estModel.TaxSet1Title = dtUserDef.TaxSet1Title;
                    estModel.TaxSet1 = isIntHaiki ? dtUserDef.TaxSet1K : dtUserDef.TaxSet1H;
                    estModel.TaxSet2Title = dtUserDef.TaxSet2Title;
                    estModel.TaxSet2 = isIntHaiki ? dtUserDef.TaxSet2K : dtUserDef.TaxSet2H;
                    estModel.TaxSet3Title = dtUserDef.TaxSet3Title;
                    estModel.TaxSet3 = isIntHaiki ? dtUserDef.TaxSet3K : dtUserDef.TaxSet3H;
                    estModel.TaxFreeSet1Title = dtUserDef.TaxFreeSet1Title;
                    estModel.TaxFreeSet1 = isIntHaiki ? int.Parse(dtUserDef.TaxFreeSet1K) : dtUserDef.TaxFreeSet1H;
                    estModel.TaxFreeSet2Title = dtUserDef.TaxFreeSet2Title;
                    estModel.TaxFreeSet2 = isIntHaiki ? dtUserDef.TaxFreeSet2K : dtUserDef.TaxFreeSet2H;
                }
                else
                {
                    estModel.ConTaxInputKb = true;
                }
                int intSelfIns = 0;
                int intRemIns = 0;
                string inYYYY = "";
                string inMM = "";

                if (flgTaxAutoCalc && intHaiki > 0)
                {
                    if (strCheckCarYm.Length == 6)
                    {
                        inYYYY = CommonFunction.Left(estModel.CheckCarYm, 4);
                        inMM = CommonFunction.Right(estModel.CheckCarYm, 2);
                    }
                    if (!_commonFuncHelper.getSelfInsurance(intHaiki, inYYYY, inMM, userDefDamageInsMonth, ref intSelfIns, ref intRemIns))
                        return ResponseHelper.Error<EstModel>(HelperMessage.SMAI028D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI028D));

                    else if (intSelfIns > 0)
                    {
                        estModel.DamageIns = intSelfIns;
                        estModel.DamageInsMonth = intRemIns.ToString();
                    }
                }
                var vTax = _commonFuncHelper.getTax(estModel.Udate, valToken.sesTaxRatio, valToken.UserNo);
                decimal wkVal;
                if (estModel.ConTaxInputKb == true)
                {
                    if (request.nonTax != "1")
                    {
                        intCarPrice += Math.Floor(intCarPrice * vTax);
                        intCarPrice += estModel.YtiRieki;
                    }
                    estModel.RakuSatu += (int)Math.Floor(estModel.RakuSatu * vTax);
                    estModel.Rikusou += (int)Math.Floor(estModel.Rikusou * vTax);
                }
                else if (request.nonTax != "1")
                {
                    intCarPrice += estModel.YtiRieki;
                }
                else
                {
                    wkVal = intCarPrice / (1 + vTax);
                    intCarPrice = Math.Ceiling(wkVal);
                }

                estModel.CarPrice = Convert.ToInt32(intCarPrice);
                estModel.SonotaTitle = CommonConst.def_TitleSonota;
                if (!await RegEstData(estModel))
                {
                    return ResponseHelper.Error<EstModel>(HelperMessage.SMAI029D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI029D));
                }
                return ResponseHelper.Ok(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), estModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAsnetInfo");
                return ResponseHelper.Error<EstModel>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
            }
        }
        private void SetvalueToken()
        {
            var token = HelperToken.GenerateJsonToken(_jwtSettings, valToken);
            valToken.Token = token;
        }

        private async Task<bool> RegEstData(EstModel model)
        {
            try
            {
                string strEstNo = "";
                string strEstSubNo = "";
                if (!_commonEst.GetEstNoFromDb(ref strEstNo))
                {
                    return false;
                }
                if (!_commonEst.GetEstSubNoFromDb(strEstNo, ref strEstSubNo))
                {
                    return false;
                }
                TEstimate entityEst = new();
                entityEst = _mapper.Map<TEstimate>(model);
                entityEst.EstNo = strEstNo;
                entityEst.EstSubNo = strEstSubNo;
                entityEst.ModelName = StringWidthHelper.ToHalfWidth(model.ModelName);
                entityEst.DispVol = string.IsNullOrEmpty(model.DispVol) ? "" : model.DispVol.Trim().Replace("cc", "");
                entityEst.CarSum = model.CarPrice + model.Sonota + model.NouCost + model.SyakenNew - model.Discount;
                entityEst.OptionInputKb = true;
                entityEst.TaxInsInputKb = true;
                entityEst.TaxFreeKb = true;
                entityEst.TaxCostKb = true;
                entityEst.Rdate = DateTime.Now;
                entityEst.Udate = DateTime.Now;
                entityEst.Dflag = false;
                TEstimateSub entityEstSub = new();
                entityEstSub = _mapper.Map<TEstimateSub>(model);
                entityEstSub.EstNo = strEstNo;
                entityEstSub.EstSubNo = strEstSubNo;
                entityEstSub.Rdate = DateTime.Now;
                entityEstSub.Udate = DateTime.Now;
                entityEstSub.Dflag = false;
                entityEstSub.LoanModifyFlag = false;
                entityEstSub.LoanRecalcSettingFlag = true;
                entityEstSub.LoanInfo = CommonConst.def_LoanInfo_Unexecuted;
                _unitOfWork.Estimates.Add(entityEst);
                _unitOfWork.EstimateSubs.Add(entityEstSub);
                await _unitOfWork.CommitAsync();
                valToken.sesEstNo = strEstNo;
                valToken.sesEstSubNo = strEstSubNo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "regEstData", "CEST-010D");
                return false;
            }
            if (!await _commonEst.CalcSum(valToken.sesEstNo, valToken.sesEstSubNo, valToken))
            {
                return false;
            }

            return true;
        }
        private ResponseEstMainModel BindingDataEsmain(ResponseEstMainModel Model)
        {
            var estModelView = Model.EstModelView;
            estModelView.TradeDate = CommonFunction.japaneseFormat(Model.EstModel.TradeDate);
            estModelView.SalesSum = CommonFunction.setFormatCurrency(Model.EstModel.SalesSum);

            if (Model.EstModel.FirstRegYm.Trim().Length == 4)
            {
                estModelView.FirstRegYm = CommonFunction.getWareki(CommonFunction.Mid(Model.EstModel.FirstRegYm, 0, 4)) + "年";
            }
            else if (Model.EstModel.FirstRegYm.Trim().Length == 6)
            {
                estModelView.FirstRegYm = CommonFunction.getWareki(CommonFunction.Mid(Model.EstModel.FirstRegYm, 0, 4)) + "年" + Convert.ToInt32(CommonFunction.Mid(Model.EstModel.FirstRegYm, 4, 2)) + "月";
            }
            string Ysc = "";
            string Msc = "";
            CommonFunction.FormatDay(Model.EstModel.CheckCarYm ?? "0", ref Ysc, ref Msc);
            estModelView.CheckCarYm = Model.EstModel.CheckCarYm == "無し" || string.IsNullOrEmpty(Model.EstModel.CheckCarYm) ? Model.EstModel.CheckCarYm : CommonFunction.getWareki(Ysc) + "年" + Msc + "月";
            estModelView.NowRun = CommonFunction.IsNumeric(Model.EstModel.NowOdometer.ToString()) ? CommonFunction.setFormatCurrency(Model.EstModel.NowOdometer, Model.EstModel.MilUnit) : "";
            var DispVol = string.IsNullOrEmpty(Model.EstModel.DispVol) ? 0 : Convert.ToInt32(Model.EstModel.DispVol);
            estModelView.Vol = CommonFunction.setFormatCurrency(DispVol, Model.EstModel.DispVolUnit);


            if (!string.IsNullOrEmpty(Model.EstModel.EstTanName.Trim()))
                estModelView.EstTanName += "担当 : " + Model.EstModel.EstTanName + "　　";
            if (!string.IsNullOrEmpty(Model.EstModel.SekininName.Trim()))
                estModelView.SekininName += "責任者 : " + Model.EstModel.SekininName + "　　";
            estModelView.ShopTel += "TEL : " + Model.EstModel.ShopTel;
            estModelView.AAInfo = Model.EstModel.Mode == 1 ? Model.EstModel.AAInfo : "";

            string titleInOutTax = Model.EstModel.ConTaxInputKb ? CommonConst.def_TitleInTax : CommonConst.def_TitleOutTax;
            estModelView.CarPriceTitle = CommonConst.def_TitleCarPrice + titleInOutTax;
            estModelView.CarPrice = CommonFunction.setFormatCurrency(Model.EstModel.CarPrice);
            estModelView.DiscountT = Model.EstModel.Discount == 0 ? "" : CommonConst.def_TitleDisCount + titleInOutTax;
            estModelView.Discount = Model.EstModel.Discount == 0 ? "" : "▲" + Convert.ToString(CommonFunction.setFormatCurrency(Model.EstModel.Discount));
            estModelView.SonotaTitle = Model.EstModel.SonotaTitle + titleInOutTax;
            estModelView.Sonota = CommonFunction.setFormatCurrency(Model.EstModel.Sonota);
            long wSyakenNew;
            if (Model.EstModel.SyakenNew > 0 && Model.EstModel.SyakenZok == 0)
            {
                wSyakenNew = Model.EstModel.SyakenNew;
                estModelView.SyakenNewZokT = CommonConst.def_TitleSyakenNew;
            }
            else if (Model.EstModel.SyakenNew == 0 && Model.EstModel.SyakenZok > 0)
            {
                wSyakenNew = Model.EstModel.SyakenZok;
                estModelView.SyakenNewZokT = CommonConst.def_TitleSyakenZok;
            }
            else
            {
                wSyakenNew = 0;
                estModelView.SyakenNewZokT = CommonConst.def_TitleSyakenNew;
                if ((Model.EstModel.CheckCarYm!.Length == 6 &&
                    CommonFunction.DateDiff(IntervalEnum.Months, DateTime.Today, DateTime.Parse(CommonFunction.Left(Model.EstModel.CheckCarYm, 4) + "/" + CommonFunction.Right(Model.EstModel.CheckCarYm, 2) + "/01")) > 0)
                    || (Model.EstModel.CheckCarYm.Length == 4 && CommonFunction.DateDiff(IntervalEnum.Years, DateTime.Today, DateTime.Parse(Model.EstModel.CheckCarYm + "/01")) > 0))
                {
                    estModelView.SyakenNewZokT = CommonConst.def_TitleSyakenZok;
                }
            }
            estModelView.SyakenNewZokT += titleInOutTax;
            estModelView.SyakenNew = CommonFunction.setFormatCurrency(wSyakenNew);
            estModelView.OpSpeCialTitle = CommonConst.def_TitleOpSpeCial + titleInOutTax;
            estModelView.OptionPriceAll = CommonFunction.setFormatCurrency(Model.EstModel.OptionPriceAll);
            estModelView.CarSum = CommonFunction.setFormatCurrency(Model.EstModel.CarSum);
            estModelView.TaxInsAll = CommonFunction.setFormatCurrency(Model.EstModel.TaxInsAll);
            estModelView.TaxInsEquivalentAll = CommonFunction.setFormatCurrency(Model.EstModel.TaxInsEquivalentAll);
            estModelView.TaxFreeAll = CommonFunction.setFormatCurrency(Model.EstModel.TaxFreeAll);


            estModelView.DaikoTitle = CommonConst.def_TitleDaiko + titleInOutTax;
            estModelView.TaxInsEquivalentTitle = CommonConst.def_TitleTaxInsEquivalent + titleInOutTax;
            estModelView.TaxName = Model.EstModel.ConTaxInputKb ? CommonConst.def_TitleConTaxTotalInTax : CommonConst.def_TitleConTaxTotalOutTax;
            estModelView.CarSaleSumTitle = CommonConst.def_TitleCarKei;
            estModelView.SalesSumTitle = Model.EstModel.ConTaxInputKb ? CommonConst.def_TitleSalesSumInTax : CommonConst.def_TitleSalesSumOutTax;

            estModelView.TaxCostAll = CommonFunction.setFormatCurrency(Model.EstModel.TaxCostAll);
            estModelView.ConTax = CommonFunction.setFormatCurrency(Model.EstModel.ConTax);
            estModelView.CarSaleSum = CommonFunction.setFormatCurrency(Model.EstModel.CarSaleSum);
            estModelView.TradeInPrice = Model.EstModel.TradeInPrice == 0 ? "" : "▲" + Convert.ToString(CommonFunction.setFormatCurrency(Model.EstModel.TradeInPrice));
            estModelView.Balance = CommonFunction.setFormatCurrency(Model.EstModel.Balance);

            estModelView.AutoTax = CommonFunction.setFormatCurrency(Model.EstModel.AutoTax, Model.EstModel.TaxInsInputKb);
            estModelView.AutoTaxEquivalent = CommonFunction.setFormatCurrency(Model.EstModel.AutoTaxEquivalent, Model.EstModel.TaxInsInputKb);
            estModelView.AcqTax = CommonFunction.setFormatCurrency(Model.EstModel.AcqTax, Model.EstModel.TaxInsInputKb);
            estModelView.WeightTax = CommonFunction.setFormatCurrency(Model.EstModel.WeightTax, Model.EstModel.TaxInsInputKb);
            estModelView.DamageIns = CommonFunction.setFormatCurrency(Model.EstModel.DamageIns, Model.EstModel.TaxInsInputKb);
            estModelView.DamageInsEquivalent = CommonFunction.setFormatCurrency(Model.EstModel.DamageInsEquivalent, Model.EstModel.TaxInsInputKb);
            estModelView.OptionIns = CommonFunction.setFormatCurrency(Model.EstModel.OptionIns, Model.EstModel.TaxInsInputKb);

            if (Model.EstModel.TaxInsInputKb)
            {
                if (Model.EstModel.AutoTaxEquivalent > 0)
                    estModelView.AutoTaxMonth = CommonConst.def_TitleAutoTaxEquivalent;
                else
                    estModelView.AutoTaxMonth = CommonConst.def_TitleAutoTax + (Model.EstModel.AutoTax == 0 ? "" : "（" + Model.EstModel.AutoTaxMonth + "月中登録）");

                if (Model.EstModel.DamageInsEquivalent > 0)
                    estModelView.DamageInsMonth = CommonConst.def_TitleDamageInsEquivalent;
                else
                    estModelView.DamageInsMonth = CommonConst.def_TitleDamageIns + (Model.EstModel.DamageIns == 0 ? "" : "（" + Model.EstModel.DamageInsMonth + "ヶ月）");
            }

            estModelView.TaxFreeGarage = CommonFunction.setFormatCurrency(Model.EstModel.TaxFreeGarage, Model.EstModel.TaxFreeKb);
            estModelView.TaxFreeCheck = CommonFunction.setFormatCurrency(Model.EstModel.TaxFreeCheck, Model.EstModel.TaxFreeKb);
            estModelView.TaxFreeTradeIn = CommonFunction.setFormatCurrency(Model.EstModel.TaxFreeTradeIn, Model.EstModel.TaxFreeKb);
            estModelView.TaxFreeRecycle = CommonFunction.setFormatCurrency(Model.EstModel.TaxFreeRecycle, Model.EstModel.TaxFreeKb);
            estModelView.TaxFreeOther = CommonFunction.setFormatCurrency(Model.EstModel.TaxFreeOther, Model.EstModel.TaxFreeKb);
            estModelView.TaxFreeSet1Title = Model.EstModel.TaxFreeKb ? Model.EstModel.TaxFreeSet1Title : "";
            estModelView.TaxFreeSet1 = CommonFunction.setFormatCurrency(Model.EstModel.TaxFreeSet1, Model.EstModel.TaxFreeKb);
            estModelView.TaxFreeSet2Title = Model.EstModel.TaxFreeKb ? Model.EstModel.TaxFreeSet2Title : "";
            estModelView.TaxFreeSet2 = CommonFunction.setFormatCurrency(Model.EstModel.TaxFreeSet2, Model.EstModel.TaxFreeKb);

            estModelView.TaxGarage = CommonFunction.setFormatCurrency(Model.EstModel.TaxGarage, Model.EstModel.TaxCostKb);
            estModelView.TaxCheck = CommonFunction.setFormatCurrency(Model.EstModel.TaxCheck, Model.EstModel.TaxCostKb);
            estModelView.TaxTradeIn = CommonFunction.setFormatCurrency(Model.EstModel.TaxTradeIn, Model.EstModel.TaxCostKb);
            estModelView.TaxDelivery = CommonFunction.setFormatCurrency(Model.EstModel.TaxDelivery, Model.EstModel.TaxCostKb);
            estModelView.TaxRecycle = CommonFunction.setFormatCurrency(Model.EstModel.TaxRecycle, Model.EstModel.TaxCostKb);
            estModelView.TaxOther = CommonFunction.setFormatCurrency(Model.EstModel.TaxOther, Model.EstModel.TaxCostKb);
            estModelView.TaxTradeInSatei = CommonFunction.setFormatCurrency(Model.EstModel.TaxTradeInSatei, Model.EstModel.TaxCostKb);
            estModelView.TaxSet1Title = Model.EstModel.TaxCostKb ? Model.EstModel.TaxSet1Title : "";
            estModelView.TaxSet1 = CommonFunction.setFormatCurrency(Model.EstModel.TaxSet1, Model.EstModel.TaxCostKb);
            estModelView.TaxSet2Title = Model.EstModel.TaxCostKb ? Model.EstModel.TaxSet2Title : "";
            estModelView.TaxSet2 = CommonFunction.setFormatCurrency(Model.EstModel.TaxSet2, Model.EstModel.TaxCostKb);
            estModelView.TaxSet3Title = Model.EstModel.TaxCostKb ? Model.EstModel.TaxSet3Title : "";
            estModelView.TaxSet3 = CommonFunction.setFormatCurrency(Model.EstModel.TaxSet3, Model.EstModel.TaxCostKb);

            estModelView.Deposit = CommonFunction.setFormatCurrency(Model.EstModel.Deposit);
            estModelView.Principal = Model.EstModel.Principal == 0 ? estModelView.SalesSum : CommonFunction.setFormatCurrency(Model.EstModel.SalesSum - Model.EstModel.Deposit);
            estModelView.PartitionFee = CommonFunction.setFormatCurrency((long)Model.EstModel.PartitionFee);
            estModelView.PartitionAmount = CommonFunction.setFormatCurrency(Model.EstModel.PartitionAmount);
            estModelView.PayTimes = Model.EstModel.PayTimes > 0 ? Model.EstModel.PayTimes + " 回" : "";

            string fromdt = CommonFunction.getFormatDayYMD(Model.EstModel.FirstPayMonth);
            string todt = CommonFunction.getFormatDayYMD(Model.EstModel.LastPayMonth); ;
            estModelView.Kikan = (string.IsNullOrEmpty(fromdt) || string.IsNullOrEmpty(todt)) ? "" : fromdt + " - " + todt;

            estModelView.FirstPayAmount = CommonFunction.setFormatCurrency(Model.EstModel.FirstPayAmount);
            estModelView.PayAmount = CommonFunction.setFormatCurrency(Model.EstModel.PayAmount);
            estModelView.PayTimes2 = Model.EstModel.PayTimes > 0 ? "（×" + Convert.ToString(Model.EstModel.PayTimes - 1) + "回）" : "";
            if (Model.EstModel.BonusAmount > 0)
            {
                estModelView.BonusMonth = Model.EstModel.BonusFirst != "" ? Model.EstModel.BonusFirst + "月" : "";
                estModelView.BonusMonth += Model.EstModel.BonusSecond != "" ? "・" + Model.EstModel.BonusSecond + "月" : "";
            }
            estModelView.BonusAmount = CommonFunction.setFormatCurrency(Model.EstModel.BonusAmount);
            estModelView.BonusTimes = Model.EstModel.BonusAmount > 0 ? Model.EstModel.BonusTimes > 0 ? "（×" + Model.EstModel.BonusTimes + "回）" : "" : "";
            estModelView.Rate = Model.EstModel.Rate > 0 ? "分割払手数料は実質年率 " + Model.EstModel.Rate + "% で計算しています" : "";
            estModelView.WarningRecalc = Model.EstModel.LoanInfo == CommonConst.def_LoanInfo_Clear ? CommonConst.LOAN_RECALC_CLEAR :
                                             Model.EstModel.LoanInfo == CommonConst.def_LoanInfo_NormalEnd ? CommonConst.LOAN_RECALC_NORMAL_END :
                                              Model.EstModel.LoanInfo == CommonConst.def_LoanInfo_Error ? CommonConst.LOAN_RECALC_ERROR : "";

            estModelView.FirstRegistration = CommonFunction.getFormatDayYMD(Model.EstIDEModel.FirstRegistration);
            estModelView.InspectionExpirationDate = CommonFunction.getFormatDayYMD(Model.EstIDEModel.InspectionExpirationDate);
            estModelView.LeaseStartMonth = CommonFunction.getFormatDayYMD(Model.EstIDEModel.LeaseStartMonth);
            estModelView.LeasePeriod = Model.EstIDEModel.LeasePeriod == 0 ? "" : Model.EstIDEModel.LeasePeriod + "ヶ月";
            estModelView.LeaseExpirationDate = CommonFunction.getFormatDayYMD(Model.EstIDEModel.LeaseExpirationDate);
            estModelView.MonthlyLeaseFee = CommonFunction.setFormatCurrency(Model.EstIDEModel.MonthlyLeaseFee);
            estModelView.LeaseTotal = string.IsNullOrEmpty(estModelView.LeasePeriod) ? "" : estModelView.MonthlyLeaseFee + " (" + estModelView.LeasePeriod + ")";
            estModelView.ContractPlanName = Model.EstIDEModel.ContractPlanName;
            estModelView.IsExtendedGuarantee = Model.EstIDEModel.IsExtendedGuarantee == 99 ? "" : Model.EstIDEModel.IsExtendedGuarantee == 0 ? "あり" : "なし";
            var insuranceCompany = string.IsNullOrEmpty(Model.EstIDEModel.InsuranceCompanyName) ? "なし" : Model.EstIDEModel.InsuranceCompanyName;
            estModelView.InsuranceCompanyName = Model.EstIDEModel.IsData ? insuranceCompany : "";
            estModelView.InsuranceFee = CommonFunction.setFormatCurrency(Model.EstIDEModel.InsuranceFee);
            estModelView.DownPayment = CommonFunction.setFormatCurrency(Model.EstIDEModel.DownPayment);
            estModelView.IdeTradeInPrice = CommonFunction.setFormatCurrency(Model.EstIDEModel.TradeInPrice);
            estModelView.TradeInFirstRegYm = !string.IsNullOrEmpty(Model.EstModel.TradeInFirstRegYm) ? (Model.EstModel.TradeInFirstRegYm.Trim().Length == 4) ? CommonFunction.getWareki(CommonFunction.Mid(Model.EstModel.TradeInFirstRegYm, 1, 4)) + "年" :
                          (Model.EstModel.TradeInFirstRegYm.Trim().Length == 6) ? CommonFunction.getWareki(CommonFunction.Mid(Model.EstModel.TradeInFirstRegYm, 0, 4)) + "年" + Convert.ToInt32(CommonFunction.Mid(Model.EstModel.TradeInFirstRegYm, 4, 2)) + "月" : "" : "";

            string Yfm = "";
            string Mfm = "";
            CommonFunction.FormatDay(Model.EstModel.TradeInCheckCarYm ?? "0", ref Yfm, ref Mfm);
            estModelView.TradeInCheckCarYm = Model.EstModel.TradeInCheckCarYm == "無し" || string.IsNullOrEmpty(Model.EstModel.TradeInCheckCarYm) ? Model.EstModel.TradeInCheckCarYm : CommonFunction.getWareki(Yfm) + "年" + Mfm + "月";
            estModelView.SitaRun = CommonFunction.setFormatCurrency(Model.EstModel.TradeInNowOdometer, Model.EstModel.TradeInMilUnit);
            estModelView.TradeInRegNo = string.IsNullOrEmpty(Model.EstModel.TradeInRegNo) ? "" : Model.EstModel.TradeInRegNo.Replace("/", "");

            estModelView.OptionPrice1 = CommonFunction.setFormatCurrency(Model.EstModel.OptionPrice1);
            estModelView.OptionPrice2 = CommonFunction.setFormatCurrency(Model.EstModel.OptionPrice2);
            estModelView.OptionPrice3 = CommonFunction.setFormatCurrency(Model.EstModel.OptionPrice3);
            estModelView.OptionPrice4 = CommonFunction.setFormatCurrency(Model.EstModel.OptionPrice4);
            estModelView.OptionPrice5 = CommonFunction.setFormatCurrency(Model.EstModel.OptionPrice5);
            estModelView.OptionPrice6 = CommonFunction.setFormatCurrency(Model.EstModel.OptionPrice6);
            estModelView.OptionPrice7 = CommonFunction.setFormatCurrency(Model.EstModel.OptionPrice7);
            estModelView.OptionPrice8 = CommonFunction.setFormatCurrency(Model.EstModel.OptionPrice8);
            estModelView.OptionPrice9 = CommonFunction.setFormatCurrency(Model.EstModel.OptionPrice8);
            estModelView.OptionPrice10 = CommonFunction.setFormatCurrency(Model.EstModel.OptionPrice10);
            estModelView.OptionPrice11 = CommonFunction.setFormatCurrency(Model.EstModel.OptionPrice11);
            estModelView.OptionPrice12 = CommonFunction.setFormatCurrency(Model.EstModel.OptionPrice12);
            estModelView.Notes = Model.EstModel.Notes.ReplaceLineEndings("<br />");
            if (File.Exists(Model.EstModel.CarImgPath))
            {
                estModelView.CarImgPath = ConverterHelper.LoadImage(Model.EstModel.CarImgPath);
            }
            else
            {
                estModelView.CarImgPath = ConverterHelper.LoadImage(_jwtPhysicalSettings.DmyImg);
            }
            return Model;
        }

        // ******************************************
        // CSV ファイル 項目の整形
        // ******************************************
        private string formatCsvTitle(string prmStr)
        {
            if (0 <= prmStr.IndexOf("（"))
                prmStr = CommonFunction.Left(prmStr, prmStr.IndexOf("（"));

            return prmStr;
        }

        private string SetSyakenNewZokT(EstModel estModel)
        {
            if (estModel.SyakenNew > 0 && estModel.SyakenZok == 0)
                return CommonConst.def_TitleSyakenNew;
            else if (estModel.SyakenNew == 0 && estModel.SyakenZok > 0)
                return CommonConst.def_TitleSyakenZok;
            else
            {
                string initText = CommonConst.def_TitleSyakenNew;

                if ((estModel.CheckCarYm!.Length == 6 &&
                    CommonFunction.DateDiff(IntervalEnum.Months, DateTime.Today, DateTime.Parse(CommonFunction.Left(estModel.CheckCarYm, 4) + "/" + CommonFunction.Right(estModel.CheckCarYm, 2) + "/01")) > 0)
                    || (estModel.CheckCarYm.Length == 4 && CommonFunction.DateDiff(IntervalEnum.Years, DateTime.Today, DateTime.Parse(estModel.CheckCarYm + "/01")) > 0))
                    initText = CommonConst.def_TitleSyakenZok;

                return initText;
            }
        }

        // ******************************************
        // CSV ファイル 項目の整形
        // ******************************************
        private string formatCsvItem(string prmStr)
        {
            if (string.IsNullOrEmpty(prmStr))
            {
                return "";
            }

            string strWork = prmStr.Trim();

            // 桁区切りカンマや単位を削除
            if (((0 <= strWork.IndexOf("円")) || (0 <= strWork.IndexOf("回")) || (0 <= strWork.IndexOf("千Km")) | (0 <= strWork.IndexOf("Km")) || (0 <= strWork.IndexOf("cc"))))
            {
                strWork = strWork.Replace(",", "");
                strWork = strWork.Replace("円", "");
                strWork = strWork.Replace("回", "");
                strWork = strWork.Replace("千Km", "");
                strWork = strWork.Replace("Km", "");
                strWork = strWork.Replace("cc", "");
                strWork = strWork.Replace("×", "");
                strWork = strWork.Replace("（", "");
                strWork = strWork.Replace("）", "");
            }

            // マイナス金額表示用記号の変換
            strWork = strWork.Replace("▲", "");

            // 半角カンマを全角カンマに変換
            strWork = strWork.Replace(",", "，");

            // 半角ダブルクォーテーションを全角ダブルクォーテーションに変換
            strWork = strWork.Replace("\"", char.ConvertFromUtf32((int)0x201D));

            // 制御コードが含まれていた場合のエスケープ
            strWork = strWork.Replace("\r", "%0D");
            strWork = strWork.Replace("\n", "%0A");
            strWork = strWork.Replace("\t", "%09");
            strWork = strWork.Replace("\b", "%08");

            // HTML タグが含まれていた場合のエスケープ
            strWork = strWork.Replace("<BR>", "%0D%0A");
            strWork = strWork.Replace("<BR />", "%0A");
            strWork = strWork.Replace("<br>", "%0D%0A");
            strWork = strWork.Replace("<br />", "%0A");

            // 末尾がエスケープされた改行コードの場合、それを削除（１行目のみのデータ）
            // ※先頭がエスケープされた改行コードの場合は、特になにもしない（２行目のみのデータ）
            if (string.IsNullOrEmpty(strWork) == false && strWork.Length >= 6)
            {
                if ((strWork.Substring(strWork.Length - 6) == "%0D%0A"))
                    strWork = strWork.Replace("%0D%0A", "");
            }

            return strWork.Trim();
        }



        #endregion fuc private
    }
}












