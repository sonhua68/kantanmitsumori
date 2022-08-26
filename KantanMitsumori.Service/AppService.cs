using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Reflection;

namespace KantanMitsumori.Service
{
    public class AppService : IAppService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        private LogToken valToken;
        private CommonFuncHelper _commonFuncHelper;

        private List<string> reCalEstModel;
        private List<string> reCalEstSubModel;

        public AppService(IMapper mapper, ILogger<AppService> logger, IUnitOfWork unitOfWork, CommonFuncHelper commonFuncHelper)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonFuncHelper = commonFuncHelper;
            reCalEstModel = new List<string>();
            reCalEstSubModel = new List<string>();
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

        public ResponseBase<UserModel> getUserName(string userNo)
        {
            try
            {
                ResponseBase<UserModel> iResult = new ResponseBase<UserModel>();

                var mUser = _unitOfWork.Users.GetAll().Where(u => u.UserNo == userNo).Select(i => _mapper.Map<UserModel>(i)).FirstOrDefault();

                mUser!.UserInfo = mUser.UserNo + " " + mUser.UserNm + " 様";

                return ResponseHelper.Ok<UserModel>("", "", mUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GCMF-020D" + " ◆会員認証エラー◆ 復号化後会員番号：" + userNo);
                return ResponseHelper.Error<UserModel>("Error", "Error");
            }
        }

        public async Task<ResponseBase<FormEstMainModel>> getEstMain(string isInputBack, string sel, RequestHeaderModel request, LogToken logToken)
        {
            try
            {
                valToken = logToken;

                logToken.stateLoadWindow = "EstMain";

                if (Strings.InStr(isInputBack, "1") == 0 && (String.IsNullOrEmpty(sel) ? "0" : sel) == "0")
                {
                    logToken.sesPriDisp = "0";
                }

                logToken.stateLoadWindow = "EstMain";

                // ASNET、店頭商談NETの判定
                if (request.Mode != "" && _commonFuncHelper.IsNumeric(request.Mode!))
                {
                    logToken.sesMode = request.Mode;
                }
                else
                {
                    logToken.sesMode = "";
                    logToken.sesErrMsg = CommonConst.def_ErrMsg4 + CommonConst.def_ErrMsg4 + "SMAI-001P" + CommonConst.def_ErrCodeR;
                    //return Redirect(CommonConst.def_ErrPage);
                    return ResponseHelper.Error<FormEstMainModel>("Error", logToken.sesErrMsg);
                }

                // 価格表示有無の取得（店頭商談NET
                if (request.PriDisp != "" && _commonFuncHelper.IsNumeric(request.PriDisp!))
                {
                    logToken.sesPriDisp = request.PriDisp;
                }

                if (request.leaseFlag != "" && _commonFuncHelper.IsNumeric(request.leaseFlag!))
                {
                    logToken.sesLeaseFlag = request.leaseFlag;
                }
                else
                {
                    logToken.sesLeaseFlag = "0";
                }

                // ASNET車両詳細ページからの情報を取得・DB保存
                var getAsInfo = await getAsnetInfo(request);

                if (getAsInfo.ResultStatus == 0)
                {
                    return ResponseHelper.Error<FormEstMainModel>("Error", getAsInfo.MessageContent);
                }

                return ResponseHelper.Ok<FormEstMainModel>("", "");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return ResponseHelper.Error<FormEstMainModel>("Error", "Error");
            }
        }

        /// <summary>
        /// ASNET車両ページからの情報を取得、DB保存
        /// </summary>
        private async Task<ResponseBase<bool>> getAsnetInfo(RequestHeaderModel request)
        {
            if (request.cot == "" || request.cna == "" || request.mem == "")
            {
                valToken.sesErrMsg = CommonConst.def_ErrMsg3 + CommonConst.def_ErrCodeL + "SMAI-020P" + CommonConst.def_ErrCodeR;
                //Redirect(CommonConst.def_ErrPage);
                return ResponseHelper.Error<bool>("Error", valToken.sesErrMsg);
            }

            //string strTempImagePath;
            //string strSavePath;

            // 会員番号取得
            string encUsrNo = request.mem!.Trim();
            string decUsrNo = "";



            if (!_commonFuncHelper.DecUserNo(encUsrNo, decUsrNo))
            {
                valToken.sesErrMsg = CommonConst.def_ErrMsg3 + CommonConst.def_ErrCodeL + "SMAI-021C" + CommonConst.def_ErrCodeR;
                //Redirect(CommonConst.def_ErrPage);
                return ResponseHelper.Error<bool>("Error", valToken.sesErrMsg);

            }

            var responseUser = getUserName(decUsrNo);

            if (responseUser.ResultStatus == 0 || responseUser.Data!.UserInfo == "0")
            {
                responseUser.MessageContent = CommonConst.def_ErrMsg3 + CommonConst.def_ErrCodeL + "SMAI-042D" + CommonConst.def_ErrCodeR;
                //ErrorAction(responseUser);
                return ResponseHelper.Error<bool>("Error", responseUser.MessageContent);
            }

            // ラベルセット
            var mode = new LogToken();
            mode.UserNo = responseUser.Data.UserNo;
            mode.UserNm = responseUser.Data.UserNm;
            var token = HelperToken.GenerateJsonToken(mode);
            mode.Token = token;


            valToken.sesUserNo = responseUser.Data.UserNo;
            valToken.sesUserNm = responseUser.Data.UserNm;
            valToken.sesUserAdr = responseUser.Data.UserAdr;
            valToken.sesUserTel = responseUser.Data.UserTel;
            valToken.sesdispUserInf = responseUser.Data.UserInfo;

            if (request.exh != "")          // '出品番号
            {
                string wAANo = request.exh!;
                string wAAPlace = request.aan!;
                string wConnerType = request.cot!;
                string wMode = request.Mode!;

                // 同じAA会場の出品車輌の見積書をすでに(何回か)作成していた場合は
                // 最新の見積データを取得し、新しい見積枝番でデータ作成。
                if (chkAANo(valToken.sesUserNo, wAANo, wAAPlace, int.Parse(wConnerType), int.Parse(wMode)))
                {

                }
                else
                {
                    return ResponseHelper.Error<bool>("Error", CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "GCMF-040D" + CommonConst.def_ErrCodeR);
                }
            }

            //setTokenCookie(token);

            return ResponseHelper.Ok<bool>("Error", "Error"); ;
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

            calcSum(vEstNo, vEstSubNo);

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
                var a = _unitOfWork.Estimates.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false);
                var estModel = _mapper.Map<EstmateModel>(_unitOfWork.Estimates.GetSingle(x => x.EstNo == inEstNo && x.EstSubNo == inEstSubNo && x.Dflag == false));

                var estSubModel = _mapper.Map<EstmateSubModel>(_unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == estModel.EstNo && x.EstSubNo == estModel.EstSubNo));

                // 消費税率取得
                var vTax = _commonFuncHelper.getTax((DateTime)estModel.Udate!, valToken.sesTaxRatio, estModel.EstNo);

                // 会員諸費用設定取得
                string userNo = valToken.sesUserNo;

                var getUserDef = _commonFuncHelper.getUserDefData(userNo);

                if (getUserDef.ResultStatus != 0)
                {
                    if (estModel.ConTaxInputKb != getUserDef.Data!.ConTaxInputKb)
                    {
                        // 消費税区分（税込／税抜）がデータと設定値で不一致の場合、データの各項目を再設定
                        estModel.ConTaxInputKb = getUserDef.Data!.ConTaxInputKb;

                        //estModel.Ca= _commonFuncHelper.reCalcItem()

                        Type type = estModel.GetType();
                        IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties().Where(x => x.PropertyType.Name == "int"));

                        foreach (PropertyInfo prop in props)
                        {
                            string properties = prop.Name;
                            // Do something with propValue
                            reCalEstModel.Add(prop.Name);


                            int objValue = (int)prop.GetValue(estModel);

                            if (reCalEstModel.Contains(prop.Name))
                            {
                                objValue = _commonFuncHelper.reCalcItem(objValue, estModel.ConTaxInputKb, vTax);
                            }

                            prop.SetValue(estModel, objValue);
                        }




                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "chkAANo " + "GCMF-040D");
                return false;
            }

            return true;
        }

        #region fuc private


        #endregion fuc private
    }
}