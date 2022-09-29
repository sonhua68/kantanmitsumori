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

namespace KantanMitsumori.Service.ASEST
{
    public class PreExaminationService : IPreExaminationService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        private CommonEstimate _commonEst;
        private CommonFuncHelper _commonFuncHelper;

        public PreExaminationService(IMapper mapper, ILogger<InpCustKanaService> logger, IUnitOfWork unitOfWork, CommonEstimate commonEst, CommonFuncHelper commonFuncHelper)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonEst = commonEst;
            _commonFuncHelper = commonFuncHelper;
        }

        public ResponseBase<ResponsePreExamination> GetInfoPreExamination(string estNo, string estSubNo)
        {
            try
            {
                // 見積書データ取得
                var estData = _commonEst.getEst_EstSubData(estNo, estSubNo);

                // 見積書データ取得
                var estIdeData = _commonEst.getEstIDEData(estNo, estSubNo);

                if (estData == null || estIdeData == null)
                {
                    return ResponseHelper.Error<ResponsePreExamination>(HelperMessage.SMAI014D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI014D));
                }



                var model = new ResponsePreExamination();
                model = _mapper.Map<ResponsePreExamination>(estData);

                return ResponseHelper.Ok<ResponsePreExamination>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetInfoPreExamination");
                return ResponseHelper.Error<ResponsePreExamination>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }
    }
}
