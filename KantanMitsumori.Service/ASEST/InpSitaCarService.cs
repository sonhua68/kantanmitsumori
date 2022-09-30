using AutoMapper;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Service.ASEST
{
    public class InpSitaCarService : IInpSitaCarService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        private CommonEstimate _commonEst;
        private CommonFuncHelper _commonFuncHelper;

        public InpSitaCarService(IMapper mapper, ILogger<InpCustKanaService> logger, IUnitOfWork unitOfWork, CommonEstimate commonEst, CommonFuncHelper commonFuncHelper)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonEst = commonEst;
            _commonFuncHelper = commonFuncHelper;
        }

        public ResponseBase<ResponseInpSitaCar> GetInfoSitaCar(string estNo, string estSubNo, string userNo)
        {
            try
            {
                // 見積書データ取得
                var estData = _commonEst.getEst_EstSubData(estNo, estSubNo);

                if (estData == null)
                {
                    return ResponseHelper.Error<ResponseInpSitaCar>("Error", CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "SMAI-041D" + CommonConst.def_ErrCodeR);
                }

                var model = new ResponseInpSitaCar();

                model = _mapper.Map<ResponseInpSitaCar>(estData);

                // 預り法定費用、手続代行費用のセット
                var hasTradeIn = (model.TradeInUM == 1 || (model.TradeInUM == 0 &&
                    (model.TaxFreeTradeIn > 0 || model.TaxTradeIn > 0 || model.TaxTradeInSatei > 0 || !string.IsNullOrEmpty(model.TradeInCarName))));

                if (!hasTradeIn)
                {
                    // 隠しフィールドには、設定レコードがあればその値を反映
                    var getUserDef = _commonFuncHelper.getUserDefData(userNo);
                    if (getUserDef != null)
                    {
                        int intHaiki = CommonFunction.IsNumeric(model.DispVol ?? "") ? Convert.ToInt32(model.DispVol) : 0;

                        if (intHaiki > 660)
                        {
                            model.TaxFreeTradeIn = getUserDef.TaxFreeTradeInH;
                            model.TaxTradeIn = getUserDef.TaxTradeInH;
                            model.TaxTradeInSatei = getUserDef.TaxTradeInChkH;
                        }
                        else
                        {
                            model.TaxFreeTradeIn = getUserDef.TaxFreeTradeInK;
                            model.TaxTradeIn = getUserDef.TaxTradeInK;
                            model.TaxTradeInSatei = getUserDef.TaxTradeInChkK;
                        }
                    }
                }

                return ResponseHelper.Ok<ResponseInpSitaCar>("OK", "OK", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return ResponseHelper.Error<ResponseInpSitaCar>("Error", "Error");
            }
        }

        public ResponseBase<List<string>> GetListOffice()
        {
            try
            {
                var getOffice = _unitOfWork.Offices.GetList(c => c.Rflag == 0).OrderBy(x => x.TofficeCode).Select(s => s.PlaceNumber ?? "").ToList();

                return ResponseHelper.Ok<List<string>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), getOffice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "setRikuji - GCMF-050D");
                return ResponseHelper.Error<List<string>>("Error", CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "GCMF-050D" + CommonConst.def_ErrCodeR);
            }
        }

        public async Task<ResponseBase<int>> UpdateInpSitaCar(RequestUpdateInpSitaCar request)
        {
            try
            {
                // get [t_Estimate]
                var estModel = _unitOfWork.Estimates.GetSingle(x => x.EstNo == request.EstNo && x.EstSubNo == request.EstSubNo && x.Dflag == false);

                // set request into model Estimate
                string firstRegYM = ""; string firstRegM = ""; string checkCarYm = "";
                if (!string.IsNullOrEmpty(request.ddlSitaFirstY))
                {
                    var firstMonth = 0;
                    if (!string.IsNullOrEmpty(request.ddlSitaFirstM))
                    {
                        firstMonth = Convert.ToInt32(request.ddlSitaFirstM!);
                        firstRegM = firstMonth.ToString().PadLeft(2, '0');
                    }
                    firstRegYM = CommonFunction.Right(request.ddlSitaFirstY, 5).Replace(")", firstRegM);
                }
                if (!string.IsNullOrEmpty(request.ddlSitaSyakenY) || !string.IsNullOrEmpty(request.ddlSitaSyakenM))
                {
                    var syakenMonth = Convert.ToInt32(request.ddlSitaSyakenM);
                    string syakenMonthFormat = syakenMonth.ToString().PadLeft(2, '0'); ;
                    checkCarYm = CommonFunction.Right(request.ddlSitaSyakenY!, 5).Replace(")", syakenMonthFormat);
                }
                int SitaUM = 0;
                if (request.SSita == 1)
                {
                    SitaUM = 1;
                }
                else
                {
                    SitaUM = 0;
                    request.TaxFreeTradeIn = 0;
                    request.TaxTradeIn = 0;
                    request.TaxTradeInSatei = 0;
                }
                string ddlTorokuNo1 = string.IsNullOrEmpty(request.ddlTorokuNo1) ? "" : request.ddlTorokuNo1.Trim();
                string txtTorokuNo1 = string.IsNullOrEmpty(request.txtToroku1) ? "" : request.txtToroku1.Trim();
                string ddlTorokuNo2 = string.IsNullOrEmpty(request.ddlTorokuNo2) ? "" : request.ddlTorokuNo2.Trim();
                string txtTorokuNo2 = string.IsNullOrEmpty(request.txtToroku2) ? "" : request.txtToroku2.Trim();


                estModel.TradeInCarName = string.IsNullOrEmpty(request.SitaCarName) ? "" : request.SitaCarName.Trim();
                estModel.TradeInFirstRegYm = firstRegYM;
                estModel.TradeInNowOdometer = request.SitaNowRun;
                estModel.TradeInRegNo = ddlTorokuNo1 + "/" + txtTorokuNo1 + "/" + ddlTorokuNo2 + "/" + txtTorokuNo2;
                estModel.TradeInChassisNo = string.IsNullOrEmpty(request.SitaCarNO) ? "" : request.SitaCarNO.Trim();
                estModel.TradeInCheckCarYm = CommonFunction.setCheckCarYm(checkCarYm, Convert.ToBoolean(request.chkSyakenUM));
                estModel.TaxFreeTradeIn = request.TaxFreeTradeIn;
                estModel.TaxTradeIn = request.TaxTradeIn;
                estModel.TradeInBodyColor = request.SitaColor;
                estModel.TradeInPrice = request.SitaPri;
                estModel.Balance = request.SitaZan;
                estModel.Udate = DateTime.Now;

                // get [t_EstimateSub]
                var estSubModel = _unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == request.EstNo && x.EstSubNo == request.EstSubNo && x.Dflag == false);

                // set request into model EstimateSub
                estSubModel.TradeInUm = SitaUM;
                estSubModel.TaxTradeInSatei = request.TaxTradeInSatei;
                string tradeInMilUnit = request.milUnit! == "その他" ? request.SitaMilUnit! : request.milUnit!;
                estSubModel.TradeInMilUnit = tradeInMilUnit;
                estSubModel.Udate = DateTime.Now;

                _unitOfWork.Estimates.Update(estModel);
                _unitOfWork.EstimateSubs.Update(estSubModel);

                await _unitOfWork.CommitAsync();

                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInpCustKana");
                return ResponseHelper.Error<int>(HelperMessage.SICK010D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICK010D));
            }
        }

    }
}
