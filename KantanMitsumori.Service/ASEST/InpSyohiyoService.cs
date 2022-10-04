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
    public class InpSyohiyoService : IInpSyohiyoService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        private CommonEstimate _commonEst;
        private CommonFuncHelper _commonFuncHelper;

        public InpSyohiyoService(IMapper mapper, ILogger<InpCustKanaService> logger, IUnitOfWork unitOfWork, CommonEstimate commonEst, CommonFuncHelper commonFuncHelper)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonEst = commonEst;
            _commonFuncHelper = commonFuncHelper;
        }

        public ResponseBase<ResponseInpSyohiyo> GetInfoSyohiyo(string estNo, string estSubNo)
        {
            try
            {
                // 見積書データ取得
                var estData = _commonEst.GetEst_EstSubData(estNo, estSubNo);

                if (estData == null)
                {
                    return ResponseHelper.Error<ResponseInpSyohiyo>(HelperMessage.SMAI014D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI014D));
                }

                var model = new ResponseInpSyohiyo();
                model = _mapper.Map<ResponseInpSyohiyo>(estData);

                return ResponseHelper.Ok<ResponseInpSyohiyo>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetInfoSyohiyo");
                return ResponseHelper.Error<ResponseInpSyohiyo>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public async Task<ResponseBase<int>> UpdateInpSyohiyo(RequestUpdateInpSyohiyo request)
        {
            try
            {
                // get [t_Estimate]
                var estModel = _unitOfWork.Estimates.GetSingle(x => x.EstNo == request.EstNo && x.EstSubNo == request.EstSubNo && x.Dflag == false);

                // set request into model Estimate
                estModel.TaxCheck = request.TaxCheck;
                estModel.TaxGarage = request.TaxGarage;
                estModel.TaxTradeIn = request.TaxTradeIn;
                estModel.TaxRecycle = request.TaxRecycle;
                estModel.TaxDelivery = request.TaxDelivery;
                estModel.TaxOther = request.TaxOther;
                estModel.TaxCostAll = request.TaxCostAll;
                estModel.TaxFreeCheck = request.TaxFreeCheck;
                estModel.TaxFreeGarage = request.TaxFreeGarage;
                estModel.TaxFreeTradeIn = request.TaxFreeTradeIn;
                estModel.TaxFreeRecycle = request.TaxFreeRecycle;
                estModel.TaxFreeOther = request.TaxFreeOther;
                estModel.TaxFreeAll = request.TaxFreeAll;
                estModel.Udate = DateTime.Now;

                // get [t_EstimateSub]
                var estSubModel = _unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == request.EstNo && x.EstSubNo == request.EstSubNo && x.Dflag == false);

                // set request into model EstimateSub
                estSubModel.TaxTradeInSatei = request.TaxTradeInSatei;
                estSubModel.TaxSet1Title = request.TaxSet1Title;
                estSubModel.TaxSet1 = request.TaxSet1;
                estSubModel.TaxSet2Title = request.TaxSet2Title;
                estSubModel.TaxSet2 = request.TaxSet2;
                estSubModel.TaxSet3Title = request.TaxSet3Title;
                estSubModel.TaxSet3 = request.TaxSet3;
                estSubModel.TaxFreeSet1Title = request.TaxFreeSet1Title;
                estSubModel.TaxFreeSet1 = request.TaxFreeSet1;
                estSubModel.TaxFreeSet2Title = request.TaxFreeSet2Title;
                estSubModel.TaxFreeSet2 = request.TaxFreeSet2;
                estSubModel.Udate = DateTime.Now;

                _unitOfWork.Estimates.Update(estModel);
                _unitOfWork.EstimateSubs.Update(estSubModel);

                await _unitOfWork.CommitAsync();

                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInpSyohiyo");
                return ResponseHelper.Error<int>(HelperMessage.SICK010D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICK010D));
            }
        }

    }
}
