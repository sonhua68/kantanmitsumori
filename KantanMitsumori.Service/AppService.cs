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
                var mUser = _unitOfWork.Users.GetAll().Where(u => u.UserNo == userNo).Select(i => _mapper.Map<UserModel>(i)).FirstOrDefault();

                mUser!.UserInfo = mUser.UserNo + " " + mUser.UserNm + " 様";
                return mUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GCMF-020D" + " ◆会員認証エラー◆ 復号化後会員番号：" + userNo);
                return null;
            }
        }


        public ResponseBase<FormEstMainModel> getEstMain(string sel, RequestHeaderModel request)
        {
            try
            {
                var formEstMainModel = new FormEstMainModel();

                valToken = new LogToken();

                valToken = _mapper.Map<LogToken>(request);

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
                    return ResponseHelper.Error<FormEstMainModel>("Error", valToken.sesErrMsg);
                }

                // 価格表示有無の取得（店頭商談NET
                if (request.PriDisp != "" && CommonFunction.IsNumeric(request.PriDisp!))
                {
                    valToken.sesPriDisp = request.PriDisp;
                }

                if (request.leaseFlag != "" && CommonFunction.IsNumeric(request.leaseFlag!))
                {
                    valToken.sesLeaseFlag = request.leaseFlag;
                }
                else
                {
                    valToken.sesLeaseFlag = "0";
                }

                // ASNET車両詳細ページからの情報を取得・DB保存
                var getAsInfo = getAsnetInfo(request);

                formEstMainModel.Token = valToken.Token;

                if (getAsInfo.ResultStatus == (int)enResponse.isError)
                {
                    return ResponseHelper.Error<FormEstMainModel>("Error", getAsInfo.MessageContent);
                }
                return ResponseHelper.Ok<FormEstMainModel>("", "", data: formEstMainModel);
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
        private ResponseBase<string> getAsnetInfo(RequestHeaderModel request)
        {
            bool isCheck = (string.IsNullOrEmpty(request.cot) || string.IsNullOrEmpty(request.cna) || string.IsNullOrEmpty(request.mem));
            if (isCheck)
            {
                valToken.sesErrMsg = CommonConst.def_ErrMsg3 + CommonConst.def_ErrCodeL + "SMAI-020P" + CommonConst.def_ErrCodeR;
                return ResponseHelper.Error<string>("Error", valToken.sesErrMsg);
            }
            //string strTempImagePath;
            //string strSavePath;

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
                        ResponseHelper.Error<bool>("Error", "Error");
                    }
                    else
                    {
                        return ResponseHelper.Error<string>("Error", CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "GCMF-040D" + CommonConst.def_ErrCodeR);
                    }
                }
            }

            SetvalueToken();

            return ResponseHelper.Ok<string>("Error", "Error", "");

        }

        public ResponseBase<UserModel> getUserInfo(string mem)
        {
            // 会員番号取得
            string decUsrNo = "";

            if (!_commonFuncHelper.DecUserNo(mem.Trim(), decUsrNo))
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