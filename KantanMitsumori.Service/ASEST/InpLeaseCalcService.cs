using AutoMapper;
using GrapeCity.DataVisualization.TypeScript;
using GrapeCity.Enterprise.Data.VisualBasicReplacement;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Enum;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;

namespace KantanMitsumori.Service
{
    public class InpLeaseCalcService : IInpLeaseCalcService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfWorkIDE _unitOfWorkIDE;
        private CommonEstimate _commonEst;
        public InpLeaseCalcService(IMapper mapper, CommonEstimate commonEst, IUnitOfWorkIDE unitOfWorkIDE, ILogger<InpLeaseCalcService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonEst = commonEst;
            _unitOfWorkIDE = unitOfWorkIDE;
        }

        public async Task<ResponseBase<List<ResponseCarType>>> GetCarType()
        {
            try
            {
                var data = _unitOfWorkIDE.CarTypes.GetAll().Select(i => _mapper.Map<MtIdeCartype>(i)).ToList();
                return ResponseHelper.Ok<List<ResponseCarType>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCarType");
                return ResponseHelper.Error<List<ResponseCarType>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }
    }

}