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
    public class InpCustKanaService : IInpCustKanaService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        private CommonEstimate _commonEst;

        public InpCustKanaService(IMapper mapper, ILogger<InpCustKanaService> logger, IUnitOfWork unitOfWork, CommonEstimate commonEst)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonEst = commonEst;
        }

        public ResponseBase<ResponseInpCustKana> getInfoCust(string estNo, string estSubNo)
        {
            try
            {
                // 見積書データ取得
                var estData = _commonEst.getEst_EstSubData(estNo, estSubNo);

                if (estData == null)
                {
                    return ResponseHelper.Error<ResponseInpCustKana>("Error", CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "SMAI-041D" + CommonConst.def_ErrCodeR);
                }

                var model = new ResponseInpCustKana();
                model.EstNo = estData.EstNo;
                model.EstSubNo = estData.EstSubNo;
                model.CustMemo = estData.CustMemo;
                model.CustKana = estData.CustKname;
                model.CustMemo = estData.CustMemo;

                return ResponseHelper.Ok<ResponseInpCustKana>("OK", "OK", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return ResponseHelper.Error<ResponseInpCustKana>("Error", "Error");
            }
        }

        public async Task<ResponseBase<int>> UpdateInpCustKana(RequestUpdateInpCustKana model)
        {
            try
            {
                // get [t_Estimate]
                var estModel = _unitOfWork.Estimates.GetSingle(x => x.EstNo == model.EstNo && x.EstSubNo == model.EstSubNo && x.Dflag == false);
                estModel.CustKname = model.CustKana;
                estModel.Udate = DateTime.Now;

                // get [t_EstimateSub]
                var estSubModel = _unitOfWork.EstimateSubs.GetSingle(x => x.EstNo == model.EstNo && x.EstSubNo == model.EstSubNo && x.Dflag == false);
                estSubModel.CustMemo = model.CustMemo;
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
