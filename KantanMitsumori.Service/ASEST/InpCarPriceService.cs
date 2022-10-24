using AutoMapper;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Model;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;
using KantanMitsumori.IService;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Entity.ASESTEntities;

namespace KantanMitsumori.Service
{
    public class InpCarPriceService : IInpCarPriceService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private CommonEstimate _commonEst;

        public InpCarPriceService(IMapper mapper
            , ILogger<InpCarPriceService> logger
            , IUnitOfWork unitOfWork
            , CommonEstimate commonEst)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonEst = commonEst;
        }

        public ResponseBase<ResponseInpCarPrice> GetCarPriceInfo(RequestInpCarPrice request)
        {
            try
            {
                // Query data from database
                var estEntity = _unitOfWork.Estimates.GetSingle(i => i.EstNo == request.EstNo && i.EstSubNo == request.EstSubNo);
                var estSubEntity = _unitOfWork.EstimateSubs.GetSingle(i => i.EstNo == request.EstNo && i.EstSubNo == request.EstSubNo);
                var userEntity = _unitOfWork.UserDefs.GetSingle(i => i.UserNo == request.UserNo);

                if (estEntity == null || estSubEntity == null)
                    return ResponseHelper.Error<ResponseInpCarPrice>(HelperMessage.CEST040D, KantanMitsumoriUtil.GetMessage(HelperMessage.CEST040D));

                // Mapping data from entity to model
                var model = new ResponseInpCarPrice();
                _mapper.Map(request, model);
                _mapper.Map(estEntity, model);
                _mapper.Map(estSubEntity, model);
                GetUserData(estEntity, estSubEntity, userEntity, model);
                return ResponseHelper.Ok<ResponseInpCarPrice>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCarPriceInfo");
                return ResponseHelper.Error<ResponseInpCarPrice>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(HelperMessage.ISYS010I));
            }
        }

        private void GetUserData(TEstimate estEntity, TEstimateSub estSubEntity, MUserDef userEntity, ResponseInpCarPrice model)
        {
            try
            {
                int carVol = int.Parse(estEntity.DispVol!);
                if (carVol <= 660)
                {
                    model.UserSyakenZok = $"{userEntity.SyakenZokK ?? 0}";
                    model.UserSyakenNew = $"{userEntity.SyakenNewK ?? 0}";
                }
                else
                {
                    model.UserSyakenZok = $"{userEntity.SyakenZokH ?? 0}";
                    model.UserSyakenNew = $"{userEntity.SyakenNewH ?? 0}";
                }
            }
            catch
            {
                model.UserSyakenZok = $"{userEntity.SyakenZokH ?? 0}";
                model.UserSyakenNew = $"{userEntity.SyakenNewH ?? 0}";
            }
        }

        public async Task<ResponseBase<int>> UpdateCarPrice(RequestUpdateCarPrice request, LogToken logToken)
        {
            try
            {
                // Query data from database
                var estEntity = _unitOfWork.Estimates.GetSingle(i => i.EstNo == request.EstNo && i.EstSubNo == request.EstSubNo);
                var estSubEntity = _unitOfWork.EstimateSubs.GetSingle(i => i.EstNo == request.EstNo && i.EstSubNo == request.EstSubNo);
                if (estEntity == null || estSubEntity == null)
                    return ResponseHelper.Error<int>(HelperMessage.CEST040D, KantanMitsumoriUtil.GetMessage(HelperMessage.CEST040D));

                // Map data from request
                _mapper.Map(request, estEntity);
                _mapper.Map(request, estSubEntity);

                // Update data
                var result = _unitOfWork.Estimates.Update(estEntity);
                result &= _unitOfWork.EstimateSubs.Update(estSubEntity);
                await _unitOfWork.CommitAsync();
                if (!result)
                    return ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(HelperMessage.CEST050S));
                // 小計・合計計算
                if (!await _commonEst.CalcSum(request.EstNo, request.EstSubNo!, logToken))
                    return ResponseHelper.LogicError<int>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));

                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateCarPrice");
                return ResponseHelper.Error<int>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(HelperMessage.ISYS010I));
            }
        }
    }
}
