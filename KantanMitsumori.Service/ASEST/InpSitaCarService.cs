using AutoMapper;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

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
                if (model.TradeInUM != 1 || !(model.TradeInUM == 0 &&
                    (model.TaxFreeTradeIn > 0 || model.TaxTradeIn > 0 || model.TaxTradeInSatei > 0 || model.TradeInCarName != "")))
                {
                    // 隠しフィールドには、設定レコードがあればその値を反映
                    var getUserDef = _commonFuncHelper.getUserDefData(userNo);
                    if (getUserDef != null)
                    {
                        int intHaiki = Information.IsNumeric(model.DispVol) ? Convert.ToInt32(model.DispVol) : 0;

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

        //public async Task<ResponseBase<int>> UpdateInpCustKana(RequestUpdateInpCustKana model)
        //{
        //    try
        //    {
        //        // get [t_Estimate]
        //        var estModel = _unitOfWork.Estimates.GetSingle(x => x.EstNo == model.EstNo && x.EstSubNo == model.EstSubNo && x.Dflag == false);
        //        estModel.CustKname = model.CustKana;
        //        estModel.Udate = DateTime.Now;

        //        // get [t_EstimateSub]
        //        var estSubModel = _unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == model.EstNo && x.EstSubNo == model.EstSubNo && x.Dflag == false);
        //        estSubModel.CustMemo = model.CustMemo;
        //        estSubModel.Udate = DateTime.Now;

        //        _unitOfWork.Estimates.Update(estModel);
        //        _unitOfWork.EstimateSubs.Update(estSubModel);

        //        await _unitOfWork.CommitAsync();

        //        return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "UpdateInpCustKana");
        //        return ResponseHelper.Error<int>(HelperMessage.SICK010D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICK010D));
        //    }
        //}

    }
}
