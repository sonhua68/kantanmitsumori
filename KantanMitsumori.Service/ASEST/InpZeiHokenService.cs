using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Service
{
    public class InpZeiHokenService : IInpZeiHokenService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CommonFuncHelper _commonFuncHelper;



        public InpZeiHokenService(IMapper mapper, ILogger<InpZeiHokenService> logger, CommonFuncHelper commonFuncHelper,  IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonFuncHelper = commonFuncHelper;

        }

        public ResponseBase<string> calcCarTax(RequestInpZeiHoken requestData)
        {
            try
            {
                var data = _commonFuncHelper.getCarTax(requestData.CarTaxMonth, requestData.DispVol);               
                return ResponseHelper.Ok<string>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "calcCarTax");
                return ResponseHelper.Error<string>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));

            }
        }

        public ResponseBase<int> calcJibai(RequestInpZeiHoken requestData)
        {
            try
            {
               var data = _commonFuncHelper.getSelfInsurance(requestData.DispVol,requestData.JibaiMonth);
                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), (int)data!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "calcJibai");
                return ResponseHelper.Error<int>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));

            }
        }
    }
}