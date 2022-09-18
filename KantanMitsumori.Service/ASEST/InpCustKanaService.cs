using AutoMapper;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Model;
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
                var estData = _commonEst.getEstData(estNo, estSubNo);

                if (estData == null)
                {
                    return ResponseHelper.Error<ResponseInpCustKana>("Error", CommonConst.def_ErrMsg1 + CommonConst.def_ErrCodeL + "SMAI-041D" + CommonConst.def_ErrCodeR);
                }

                var model = new ResponseInpCustKana();
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

    }
}
