using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace KantanMitsumori.Service.ASEST
{
    public class EstMainService : IEstMainService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        private readonly int LocaleID = new System.Globalization.CultureInfo("ja-JP", true).LCID;

        private LogToken valToken;
        private CommonFuncHelper _commonFuncHelper;
        private CommonEstimate _commonEst;

        public EstMainService(IMapper mapper, ILogger<EstMainService> logger, IUnitOfWork unitOfWork, CommonFuncHelper commonFuncHelper, CommonEstimate commonEst)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonFuncHelper = commonFuncHelper;
            _commonEst = commonEst;
        }

        public UserModel getUserName(string userNo)
        {
            try
            {
                var mUser = _mapper.Map<UserModel>(_unitOfWork.Users.GetSingle(x => x.UserNo == userNo));
                mUser!.UserInfo = mUser.UserNo + " " + mUser.UserNm + " 様";
                return mUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GCMF-020D" + " ◆会員認証エラー◆ 復号化後会員番号：" + userNo);
                return null;
            }
        }


        public async Task<ResponseBase<ResponseEstMainModel>> getEstMain(RequestActionModel requestAction, RequestHeaderModel request)
        {
            try
            {
                valToken = new LogToken();
                bool isSesPriDisp = requestAction.IsInpBack != 1 && requestAction.Sel == 0;
                valToken.sesPriDisp = isSesPriDisp ? "0" : "";
                valToken.stateLoadWindow = "EstMain";
                if (request.Mode != "" && Information.IsNumeric(request.Mode!))
                {
                    valToken.sesMode = request.Mode;
                }
                else
                {
                    valToken.sesMode = "";
                    return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAI001P, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI001P));
                }
                if (!string.IsNullOrEmpty(request.PriDisp) && Information.IsNumeric(request.PriDisp!))
                {
                    valToken.sesPriDisp = request.PriDisp;
                }
                var response = new ResponseEstMainModel();
                var getAsInfo = await getAsnetInfo(request);
                if (getAsInfo.ResultStatus == (int)enResponse.isError)
                    return ResponseHelper.Error<ResponseEstMainModel>("Error", getAsInfo.MessageContent);

                getAsInfo.Data!.EstNo = valToken.sesEstNo;
                getAsInfo.Data.EstSubNo = valToken.sesEstSubNo;
                response.EstModel = getAsInfo.Data!;
                SetvalueToken();
                response.AccessToken = valToken.Token;
                if (string.IsNullOrEmpty(valToken.sesEstNo) || string.IsNullOrEmpty(valToken.sesEstSubNo))
                    return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAL040S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAL040S));

                //var estData = _commonEst.setEstData(valToken.sesEstNo, valToken.sesEstSubNo);
                //if (estData.ResultStatus == (int)enResponse.isSuccess)
                //    response.EstModel = estData.Data!;
                response.EstCustomerModel = new EstCustomerModel();
                response.EstCustomerModel.CustNm = valToken.sesCustNm_forPrint ?? "";
                response.EstCustomerModel.CustZip = valToken.sesCustZip_forPrint ?? "";
                response.EstCustomerModel.CustAdr = valToken.sesCustAdr_forPrint ?? "";
                response.EstCustomerModel.CustTel = valToken.sesCustTel_forPrint ?? "";
                response.EstIDEModel = new EstimateIdeModel();
                if (response.EstModel.LeaseFlag == "1")
                {
                    response.EstIDEModel = _commonEst.setEstIDEData(ref valToken);
                    if (response.EstIDEModel == null)
                        return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAL041D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
                }
                return ResponseHelper.Ok(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "getEstMain");
                return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

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
        public async Task<ResponseBase<ResponseEstMainModel>> setFreeEst()
        {
            var estModel = new EstModel();
            estModel.CallKbn = "3";
            estModel.EstInpKbn = "2";
            estModel.TradeDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            estModel.MakerName = valToken.sesMaker ?? "";
            estModel.ModelName = valToken.sesCarNM ?? "";
            estModel.GradeName = valToken.sesGrade ?? "";
            estModel.Case = valToken.sesKata ?? "";
            estModel.MilUnit = CommonConst.def_MilUnitTKM;
            estModel.DispVol = valToken.sesHaiki ?? "";
            estModel.DispVolUnit = CommonConst.def_DispVolUnitCC;
            estModel.Mission = valToken.sesSft ?? "";
            estModel.AccidentHis = 2;
            estModel.CarImgPath = CommonConst.def_DmyImg;
            estModel.SonotaTitle = CommonConst.def_TitleSonota;
            estModel.OptionInputKb = true;
            estModel.TaxInsInputKb = true;
            estModel.TaxFreeKb = true;
            estModel.TaxCostKb = true;
            estModel.TradeInNowOdometer = 0;
            estModel.TradeInMilUnit = CommonConst.def_TradeInMilUnitKM;
            int intHaiki = Information.IsNumeric(estModel.DispVol) ? int.Parse(estModel.DispVol) : 0;
            bool flgTaxAutoCalc = _commonFuncHelper.enableTaxCalc(valToken.sesMaker!);
            int intFirstMonth = Convert.ToInt32(Strings.Format(DateTime.Now, "MM"));
            if (flgTaxAutoCalc & intHaiki > 0 && estModel.DispVolUnit == CommonConst.def_DispVolUnitCC)
            {
                var carTax = _commonFuncHelper.getCarTax(intFirstMonth, intHaiki);
                if (carTax == -1)
                    return ResponseHelper.Error<ResponseEstMainModel>("Error", CommonConst.def_ErrMsg1_Maker + CommonConst.def_ErrCodeL + "SMAI-023D" + CommonConst.def_ErrCodeR);

                estModel.AutoTax = carTax;
                estModel.AutoTaxMonth = intFirstMonth.ToString();
            }
            int userDefDamageInsMonth = 0;
            var getUserDef = _commonFuncHelper.getUserDefData(valToken.UserNo);
            if (getUserDef != null)
            {
                estModel.EstUserNo = getUserDef.UserNo;
                estModel.ConTaxInputKb = getUserDef.ConTaxInputKb;
                estModel.ShopNm = getUserDef.ShopNm;
                estModel.ShopAdr = getUserDef.ShopAdr;
                estModel.ShopTel = getUserDef.ShopTel;
                estModel.EstTanName = getUserDef.EstTanName;
                estModel.SekininName = getUserDef.SekininName;
                userDefDamageInsMonth = Convert.ToInt32(getUserDef.DamageInsMonth);
                bool isIntHaiki = intHaiki > 0 & intHaiki <= 660;
                estModel.SyakenZok = 0;
                estModel.SyakenNew = isIntHaiki ? getUserDef.SyakenNewK : getUserDef.SyakenNewH;
                estModel.TaxFreeCheck = isIntHaiki ? getUserDef.TaxFreeCheckK : getUserDef.TaxFreeCheckH;
                estModel.TaxFreeGarage = isIntHaiki ? getUserDef.TaxFreeGarageK : getUserDef.TaxFreeGarageH;
                estModel.TaxCheck = isIntHaiki ? getUserDef.TaxCheckK : getUserDef.TaxCheckH;
                estModel.TaxGarage = isIntHaiki ? getUserDef.TaxGarageK : getUserDef.TaxGarageH;
                estModel.TaxRecycle = isIntHaiki ? getUserDef.TaxRecycleK : getUserDef.TaxRecycleH;
                estModel.TaxDelivery = isIntHaiki ? getUserDef.TaxDeliveryK : getUserDef.TaxDeliveryH;
                estModel.TaxSet1Title = getUserDef.TaxSet1Title;
                estModel.TaxSet1 = isIntHaiki ? getUserDef.TaxSet1K : getUserDef.TaxSet1H;
                estModel.TaxSet2Title = getUserDef.TaxSet2Title;
                estModel.TaxSet2 = isIntHaiki ? getUserDef.TaxSet2K : getUserDef.TaxSet2H;
                estModel.TaxSet3Title = getUserDef.TaxSet3Title;
                estModel.TaxSet3 = isIntHaiki ? getUserDef.TaxSet3K : getUserDef.TaxSet3H;
                estModel.TaxFreeSet1Title = getUserDef.TaxFreeSet1Title;
                estModel.TaxFreeSet1 = isIntHaiki ? int.Parse(getUserDef.TaxFreeSet1K) : getUserDef.TaxFreeSet1H;
                estModel.TaxFreeSet2Title = getUserDef.TaxFreeSet2Title;
                estModel.TaxFreeSet2 = isIntHaiki ? getUserDef.TaxFreeSet2K : getUserDef.TaxFreeSet2H;
            }
            else
            {
                estModel.ConTaxInputKb = true;
            }
            int intSelfIns = 0; int intRemIns = 0;
            if (flgTaxAutoCalc & intHaiki > 0)
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
            estModel.YtiRieki = intSelfIns;
            estModel.CarPrice = intSelfIns;
            estModel.Mode = (byte)(string.IsNullOrEmpty(valToken.sesMode) ? 0 : Convert.ToByte(valToken.sesMode));
            if (!await regEstData(estModel))
            {
                return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAI028D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI028D));

            }
            var response = new ResponseEstMainModel();
            response.EstModel = estModel;
            if (string.IsNullOrEmpty(valToken.sesEstNo) || string.IsNullOrEmpty(valToken.sesEstSubNo))
            {
                return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAI028D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI028D));
            }
            var estData = _commonEst.setEstData(valToken.sesEstNo, valToken.sesEstSubNo);
            if (estData.ResultStatus == (int)enResponse.isSuccess)
                response.EstModel = estData.Data!;
            response.EstCustomerModel = new EstCustomerModel();
            response.EstCustomerModel.CustNm = valToken.sesCustNm_forPrint ?? "";
            response.EstCustomerModel.CustZip = valToken.sesCustZip_forPrint ?? "";
            response.EstCustomerModel.CustAdr = valToken.sesCustAdr_forPrint ?? "";
            response.EstCustomerModel.CustTel = valToken.sesCustTel_forPrint ?? "";
            response.EstIDEModel = new EstimateIdeModel();
            if (response.EstModel.LeaseFlag == "1")
            {
                response.EstIDEModel = _commonEst.setEstIDEData(ref valToken);
                if (response.EstIDEModel == null)
                    return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAI028D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI028D));

            }
            SetvalueToken();
            return ResponseHelper.Ok(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), response);

        }
        #region fuc private     
        private int chkAANo(string? userNo, string AANo, string AAPlace, int CornerType, int mode)
        {
            try
            {
                var getMaxEstSub = (from sub in _unitOfWork.DbContext.TEstimateSubs
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

                if (getMaxEstSub != null)
                {
                    valToken.sesEstNo = getMaxEstSub!.maxEstNo;
                    valToken.sesEstSubNo = getMaxEstSub.maxEstSubNo;
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
        private async Task<bool> addEstNextSubNo(string estNo, string estSubNo, bool flgRecreate = false)
        {
            try
            {
                if (!await _commonEst.calcSum(estNo, estSubNo, valToken))
                {
                    return false;
                }
                var estData = _commonEst.getEstData(estNo, estSubNo);
                if (estData == null)
                {
                    return false;
                }
                if (flgRecreate)
                {
                    estNo = "";
                    if (!_commonEst.getEstNoFromDb(ref estNo))
                    {
                        return false;
                    }
                }
                string vNextSubNo = "";
                if (!_commonEst.getEstSubNoFromDb(estNo, ref vNextSubNo))
                {
                    return false;
                }
                valToken.sesEstSubNo = vNextSubNo;
                TEstimate entityEst = new TEstimate();
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
                TEstimateSub entityEstSub = new TEstimateSub();
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "addEstNextSubNo " + "CEST-052D");
                return false;
            }

            return true;
        }

        private async Task<ResponseBase<EstModel>> getAsnetInfo(RequestHeaderModel request)
        {
            var estModel = new EstModel();

            bool isCheck = string.IsNullOrEmpty(request.cot) || string.IsNullOrEmpty(request.cna) || string.IsNullOrEmpty(request.mem);
            if (isCheck)
                return ResponseHelper.Error<EstModel>("Error", CommonConst.def_ErrMsg3 + CommonConst.def_ErrCodeL + "SMAI-020P" + CommonConst.def_ErrCodeR);


            string strTempImagePath;
            string strSavePath = "";

            estModel.LeaseFlag = string.IsNullOrEmpty(request.leaseFlag) ? "0" : request.leaseFlag;

            // get user info
            var userInfo = getUserInfo(request.mem);

            // ラベルセット
            valToken.UserNo = userInfo.Data!.UserNo;
            valToken.UserNm = userInfo.Data!.UserNm;
            if (!string.IsNullOrEmpty(request.exh))
            {
                string wAANo = request.exh!;
                string wAAPlace = request.aan!;
                string wConnerType = request.cot!;
                string wMode = request.Mode!;
                var checkAANo = chkAANo(userInfo.Data.UserNo, wAANo, wAAPlace, int.Parse(wConnerType), int.Parse(wMode));
                if (checkAANo == 1)
                {
                    if (!await addEstNextSubNo(valToken.sesEstNo, valToken.sesEstSubNo))
                        return ResponseHelper.Error<EstModel>("Error", CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "GCMF-051D" + CommonConst.def_ErrCodeR);
                }
                else if (checkAANo == -1)
                    return ResponseHelper.Error<EstModel>("Error", CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "GCMF-040D" + CommonConst.def_ErrCodeR);

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
            estModel.FirstRegYm = string.IsNullOrEmpty(vNensiki) || !Information.IsNumeric(vNensiki) ? "" : vNensiki;
            string strCheckCarYm = CommonFunction.setCheckCarYm(request.ins);
            estModel.CheckCarYm = strCheckCarYm;
            estModel.NowOdometer = string.IsNullOrEmpty(request.mil) || !Information.IsNumeric(request.mil) ? 0 : int.Parse(request.mil);
            bool isCheckMilUnit = string.IsNullOrEmpty(request.milUnit);
            estModel.MilUnit = isCheckMilUnit ? CommonConst.def_MilUnitTKM : request.milUnit;
            estModel.DispVol = string.IsNullOrEmpty(request.vol) ? "" : request.vol.Trim().Replace("cc", ")");
            bool isCheckDispVolUnit = string.IsNullOrEmpty(request.volUnit);
            if (request.volUnit == "null")
            {
                estModel.DispVolUnit = CommonConst.def_DispVolUnitCC;
            }
            else
            {
                estModel.DispVolUnit = isCheckDispVolUnit ? CommonConst.def_DispVolUnitCC : request.volUnit;
            }
            estModel.Mission = request.shi;
            estModel.AccidentHis = 2;
            estModel.BusinessHis = request.his;
            estModel.FuelName = request.FuelName;
            estModel.DriveName = request.DriveName;
            estModel.CarDoors = Information.IsNumeric(request.CarDoors) ? int.Parse(request.CarDoors) : 0;
            estModel.BodyName = request.BodyName;
            estModel.Capacity = Information.IsNumeric(request.Capacity) ? int.Parse(request.Capacity) : 0;
            estModel.Equipment = request.equ;
            estModel.BodyColor = request.col;
            string wCarImgPath = request.img;
            string outImg = "";
            string outImg1 = "";
            string outImg2 = ""; string outImg3 = ""; string outImg4 = ""; string outImg5 = ""; string outImg6 = ""; string outImg7 = ""; string outImg8 = "";
            if (string.IsNullOrEmpty(wCarImgPath))
            {
                estModel.CarImgPath = CommonConst.def_DmyImg;
                estModel.CarImgPath1 = "";
                estModel.CarImgPath2 = "";
                estModel.CarImgPath3 = "";
                estModel.CarImgPath4 = "";
                estModel.CarImgPath5 = "";
                estModel.CarImgPath6 = "";
                estModel.CarImgPath7 = "";
                estModel.CarImgPath8 = "";
            }
            else
            {
                strTempImagePath = wCarImgPath.ToUpper();
                if (!strTempImagePath.EndsWith(".JPG") & !strTempImagePath.EndsWith(".GIF") & !strTempImagePath.EndsWith(".PNG"))
                {
                    strSavePath = request.cor + request.fex + "001.jpg";
                }
                _commonFuncHelper.DownloadImg(wCarImgPath, valToken.sesCarImgPath, CommonConst.def_DmyImg, ref outImg, strSavePath);
                estModel.CarImgPath = outImg;
                _commonFuncHelper.CheckImgPath(request.img1, valToken.sesCarImgPath1, "", ref outImg1, "201.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img2, valToken.sesCarImgPath2, "", ref outImg2, "202.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img3, valToken.sesCarImgPath3, "", ref outImg3, "203.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img4, valToken.sesCarImgPath4, "", ref outImg4, "204.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img5, valToken.sesCarImgPath5, "", ref outImg5, "205.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img6, valToken.sesCarImgPath6, "", ref outImg6, "206.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img7, valToken.sesCarImgPath7, "", ref outImg7, "207.jpg", request.cor, request.fex);
                _commonFuncHelper.CheckImgPath(request.img8, valToken.sesCarImgPath8, "", ref outImg8, "208.jpg", request.cor, request.fex);
                estModel.CarImgPath1 = outImg1;
                estModel.CarImgPath2 = outImg2;
                estModel.CarImgPath3 = outImg3;
                estModel.CarImgPath4 = outImg4;
                estModel.CarImgPath5 = outImg5;
                estModel.CarImgPath6 = outImg6;
                estModel.CarImgPath7 = outImg7;
                estModel.CarImgPath8 = outImg8;
            }

            estModel.TotalCost = 0;
            decimal intCarPrice = Information.IsNumeric(request.pri) ? Convert.ToDecimal(request.pri) : 0m;
            estModel.RakuSatu = Information.IsNumeric(request.fee) ? request.cor == "F6" ? 25000 : int.Parse(request.fee) : 0;
            estModel.Rikusou = Information.IsNumeric(request.tra) ? int.Parse(request.tra) : 0;
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
            estModel.Notes = "";
            estModel.Aayear = vNensiki;
            estModel.Aahyk = string.IsNullOrEmpty(request.poi) || !Information.IsNumeric(request.poi) ? "0" : request.poi;
            estModel.Aaprice = (int)intCarPrice;
            estModel.SirPrice = estModel.CarPrice;
            estModel.YtiRieki = 0;
            estModel.Aaplace = request.aan == "" ? "" : request.aan;
            estModel.Aano = request.exh == "" ? "" : request.exh;
            if (!string.IsNullOrEmpty(request.lim))
            {
                string vAATime = request.lim.Trim();
                estModel.Aatime = request.lim.Trim().Length == 8 ? Strings.Left(vAATime, 4) + "/" + Strings.Mid(vAATime, 5, 2) + "/" + Strings.Right(vAATime, 2) : vAATime;

            }
            int intHaiki = 0;
            if (Information.IsNumeric(estModel.DispVol))
            {
                intHaiki = int.Parse(estModel.DispVol);
            }
            bool flgTaxAutoCalc = _commonFuncHelper.enableTaxCalc(request.mak);
            int intFirstMonth = Convert.ToInt32(Strings.Format(DateTime.Now, "MM"));
            if (flgTaxAutoCalc & intHaiki > 0 && estModel.DispVolUnit == CommonConst.def_DispVolUnitCC)
            {
                var carTax = _commonFuncHelper.getCarTax(intFirstMonth, intHaiki);
                if (carTax == -1)
                    return ResponseHelper.Error<EstModel>("Error", CommonConst.def_ErrMsg1_Maker + CommonConst.def_ErrCodeL + "SMAI-023D" + CommonConst.def_ErrCodeR);

                estModel.AutoTax = carTax;
                estModel.AutoTaxMonth = intFirstMonth.ToString();
            }
            int userDefDamageInsMonth = 0;
            var getUserDef = _commonFuncHelper.getUserDefData(valToken.UserNo);
            if (getUserDef != null)
            {
                estModel.ConTaxInputKb = getUserDef.ConTaxInputKb;
                estModel.ShopNm = getUserDef.ShopNm;
                estModel.ShopAdr = getUserDef.ShopAdr;
                estModel.ShopTel = getUserDef.ShopTel;
                estModel.EstTanName = getUserDef.EstTanName;
                estModel.SekininName = getUserDef.SekininName;
                userDefDamageInsMonth = Convert.ToInt32(getUserDef.DamageInsMonth);
                bool isIntHaiki = intHaiki > 0 & intHaiki <= 660;
                if ((string.IsNullOrEmpty(valToken.sesMode) ? int.Parse(valToken.sesMode) : 0) == 0)
                {
                    estModel.YtiRieki = isIntHaiki ? getUserDef.YtiRiekiK : getUserDef.YtiRiekiH;
                }
                estModel.SyakenNew = isIntHaiki ? getUserDef.SyakenNewK : getUserDef.SyakenNewH;
                estModel.SyakenZok = 0;
                if (strCheckCarYm.Length == 6
                    && DateAndTime.DateDiff(DateInterval.Month, DateTime.Today, DateTime.Parse(Strings.Left(strCheckCarYm, 4) + "/" + Strings.Right(strCheckCarYm, 2) + "/01")) > 0L
                    || strCheckCarYm.Length == 4 && DateAndTime.DateDiff(DateInterval.Year, DateTime.Today, DateTime.Parse(strCheckCarYm + "/01")) > 0L)
                {
                    estModel.SyakenNew = 0;
                    estModel.SyakenZok = isIntHaiki ? getUserDef.SyakenZokK : getUserDef.SyakenZokH;
                }
                estModel.TaxFreeCheck = isIntHaiki ? getUserDef.TaxFreeCheckK : getUserDef.TaxFreeCheckH;
                estModel.TaxFreeGarage = isIntHaiki ? getUserDef.TaxFreeGarageK : getUserDef.TaxFreeGarageH;
                estModel.TaxCheck = isIntHaiki ? getUserDef.TaxCheckK : getUserDef.TaxCheckH;
                estModel.TaxGarage = isIntHaiki ? getUserDef.TaxGarageK : getUserDef.TaxGarageH;
                estModel.TaxRecycle = isIntHaiki ? getUserDef.TaxRecycleK : getUserDef.TaxRecycleH;
                estModel.TaxDelivery = isIntHaiki ? getUserDef.TaxDeliveryK : getUserDef.TaxDeliveryH;
                estModel.TaxSet1Title = getUserDef.TaxSet1Title;
                estModel.TaxSet1 = isIntHaiki ? getUserDef.TaxSet1K : getUserDef.TaxSet1H;
                estModel.TaxSet2Title = getUserDef.TaxSet2Title;
                estModel.TaxSet2 = isIntHaiki ? getUserDef.TaxSet2K : getUserDef.TaxSet2H;
                estModel.TaxSet3Title = getUserDef.TaxSet3Title;
                estModel.TaxSet3 = isIntHaiki ? getUserDef.TaxSet3K : getUserDef.TaxSet3H;
                estModel.TaxFreeSet1Title = getUserDef.TaxFreeSet1Title;
                estModel.TaxFreeSet1 = isIntHaiki ? int.Parse(getUserDef.TaxFreeSet1K) : getUserDef.TaxFreeSet1H;
                estModel.TaxFreeSet2Title = getUserDef.TaxFreeSet2Title;
                estModel.TaxFreeSet2 = isIntHaiki ? getUserDef.TaxFreeSet2K : getUserDef.TaxFreeSet2H;
            }
            else
            {
                estModel.ConTaxInputKb = true;
            }
            int intSelfIns = 0;
            int intRemIns = 0;
            string inYYYY = "";
            string inMM = "";

            if (flgTaxAutoCalc & intHaiki > 0)
            {
                if (strCheckCarYm.Length == 6)
                {
                    inYYYY = Strings.Left(estModel.CheckCarYm, 4);
                    inMM = Strings.Right(estModel.CheckCarYm, 2);
                }
                if (!_commonFuncHelper.getSelfInsurance(intHaiki, inYYYY, inMM, userDefDamageInsMonth, ref intSelfIns, ref intRemIns))
                    return ResponseHelper.Error<EstModel>("Error", CommonConst.def_ErrMsg1_Maker + CommonConst.def_ErrCodeL + "SMAI-028D" + CommonConst.def_ErrCodeR);
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
            if (!await regEstData(estModel))
            {
                return ResponseHelper.Error<EstModel>("Error", CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "SMAI-029D" + CommonConst.def_ErrCodeR);
            }

            return ResponseHelper.Ok("OK", "OK", estModel);
        }
        private void SetvalueToken()
        {
            var token = HelperToken.GenerateJsonToken(valToken);
            valToken.Token = token;
        }

        private async Task<bool> regEstData(EstModel model)
        {
            try
            {
                string strEstNo = "";
                string strEstSubNo = "";
                if (!_commonEst.getEstNoFromDb(ref strEstNo))
                {
                    return false;
                }
                if (!_commonEst.getEstSubNoFromDb(strEstNo, ref strEstSubNo))
                {
                    return false;
                }
                TEstimate entityEst = new TEstimate();
                entityEst = _mapper.Map<TEstimate>(model);
                entityEst.EstNo = strEstNo;
                entityEst.EstSubNo = strEstSubNo;
                entityEst.ModelName = Strings.StrConv(model.ModelName, VbStrConv.Narrow, LocaleID);
                entityEst.DispVol = model.DispVol.Trim().Replace("cc", "");
                entityEst.CarSum = model.CarPrice + model.Sonota + model.NouCost + model.SyakenNew - model.Discount;
                entityEst.OptionInputKb = true;
                entityEst.TaxInsInputKb = true;
                entityEst.TaxFreeKb = true;
                entityEst.TaxCostKb = true;
                entityEst.Rdate = DateTime.Now;
                entityEst.Udate = DateTime.Now;
                entityEst.Dflag = false;
                TEstimateSub entityEstSub = new TEstimateSub();
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
            if (!await _commonEst.calcSum(valToken.sesEstNo, valToken.sesEstSubNo, valToken))
            {
                return false;
            }

            return true;
        }

        #endregion fuc private
    }
}