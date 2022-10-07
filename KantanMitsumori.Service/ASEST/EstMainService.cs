﻿using AutoMapper;
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
        private LogToken valToken;
        private readonly CommonFuncHelper _commonFuncHelper;
        private readonly CommonEstimate _commonEst;
        private readonly JwtSettings _jwtSettings;
        private readonly DataSettings _dataSettings;
        public EstMainService(IMapper mapper, ILogger<EstMainService> logger, IUnitOfWork unitOfWork, IUnitOfWorkIDE unitOfWorkIDE, CommonFuncHelper commonFuncHelper, CommonEstimate commonEst, DataSettings dataSettings, JwtSettings jwtSettings)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _unitOfWorkIDE = unitOfWorkIDE;
            _commonFuncHelper = commonFuncHelper;
            _commonEst = commonEst;
            _unitOfWorkIDE = unitOfWorkIDE;
            _jwtSettings = jwtSettings;
            _dataSettings = dataSettings;
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
                if (estData.ResultStatus == (int)enResponse.isSuccess)
                    response.EstModel = estData.Data!;
                response.EstIDEModel = new EstimateIdeModel();
                if (response.EstModel.LeaseFlag == "1")
                {
                    response.EstIDEModel = _commonEst.SetEstIDEData(valToken);
                    if (response.EstIDEModel == null)
                        return ResponseHelper.Error<ResponseEstMainModel>(HelperMessage.SMAL041D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
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
                if (estData.ResultStatus == (int)enResponse.isSuccess)
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
                var estData = _commonEst.SetEstData(valToken.sesEstNo, valToken.sesEstSubNo);
                if (estData.ResultStatus == (int)enResponse.isSuccess)
                    response.EstModel = estData.Data!;
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

                if (request.volUnit.ToLower() == "null")
                    estModel.DispVolUnit = CommonConst.def_DispVolUnitCC;
                else
                    estModel.DispVolUnit = string.IsNullOrEmpty(request.volUnit) ? CommonConst.def_DispVolUnitCC : request.volUnit;

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
                    estModel.CarImgPath = CommonConst.def_DmyImg;
                }
                else
                {
                    strTempImagePath = wCarImgPath.ToUpper();
                    if (!strTempImagePath.EndsWith(".JPG") && !strTempImagePath.EndsWith(".GIF") && !strTempImagePath.EndsWith(".PNG"))
                    {
                        strSavePath = request.cor + request.fex + "001.jpg";
                    }
                    _commonFuncHelper.DownloadImg(wCarImgPath, valToken.sesCarImgPath!, CommonConst.def_DmyImg, ref outImg, strSavePath);
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
        private static ResponseEstMainModel BindingDataEsmain(ResponseEstMainModel Model)
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
            estModelView.TaxInsEquivalentTitle = CommonConst.def_TitleDaiko + titleInOutTax;
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
            estModelView.LeaseTotal = string.IsNullOrEmpty(estModelView.LeasePeriod) ? "月額リース料(税込)" : "月額リース料(税込) " + estModelView.MonthlyLeaseFee + " (" + estModelView.LeasePeriod + ")";
            estModelView.ContractPlanName = Model.EstIDEModel.ContractPlanName;
            estModelView.IsExtendedGuarantee = Model.EstIDEModel.IsExtendedGuarantee == unchecked((byte)(-1)) ? "" : Model.EstIDEModel.IsExtendedGuarantee == 0 ? "あり" : "なし";
            estModelView.InsuranceCompanyName = Model.EstIDEModel.InsuranceCompanyName;
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
            Model.EstModelView = estModelView;
            return Model;
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


        #endregion fuc private
    }
}












