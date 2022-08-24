using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Utility;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.IService;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Service
{
    public class EstimateService : IEstimateService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;



        public EstimateService(IMapper mapper, ILogger<AppService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase<int>> Create(TEstimate model)
        {
            ResponseBase<int> iResult = new ResponseBase<int>();
            try
            {
                _unitOfWork.Estimates.Add(model);
                await _unitOfWork.CommitAsync();
                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage("jp", HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateTEstimate");
                return ResponseHelper.Error<int>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage("jp", HelperMessage.SICR001S));
            }
        }

        public ResponseBase<List<TEstimate>> GetList(RequestInputCar requestInputCar)
        {
            try
            {
                ResponseBase<List<TEstimate>> iResult = new ResponseBase<List<TEstimate>>();
                var estimatesList = _unitOfWork.Estimates.Query(n=>n.EstNo==requestInputCar.EstNo && n.EstSubNo==requestInputCar.EstSubNo).Result.Select(i => _mapper.Map<TEstimate>(i)).ToList();
                if(estimatesList==null)
                {
                    return ResponseHelper.Error<List<TEstimate>>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage("jp", HelperMessage.CEST050S));
                }
                return ResponseHelper.Ok<List<TEstimate>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage("jp", HelperMessage.I0002), estimatesList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInputCar");
                return ResponseHelper.Error<List<TEstimate>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage("jp", HelperMessage.SICR001S));

            }
            throw new NotImplementedException();
        }

        public ResponseBase<TEstimate> GetDetail(RequestInputCar requestInputCar)
        {
            try
            {               
                var estimatesList = _unitOfWork.Estimates.Query(n => n.EstNo == requestInputCar.EstNo && n.EstSubNo == requestInputCar.EstSubNo).Result.FirstOrDefault();
                if (estimatesList == null)
                {
                    return ResponseHelper.Error<TEstimate>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage("jp", HelperMessage.CEST050S));
                }
                return ResponseHelper.Ok<TEstimate>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage("jp", HelperMessage.I0002), estimatesList!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInputCar");
                return ResponseHelper.Error<TEstimate>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage("jp", HelperMessage.SICR001S));

            }
            throw new NotImplementedException();
        }

        public async Task<ResponseBase<int>> UpdateInputCar(RequestUpdateInputCar model)
        {
            try
            {
                TEstimate dtEstimates = await _unitOfWork.Estimates.GetSingle(n => n.EstNo == model.EstNo && n.EstSubNo == model.EstSubNo && n.Dflag == false);
                TEstimateSub dtEstimateSubs = await _unitOfWork.EstimateSubs.GetSingle(n => n.EstNo == model.EstNo && n.EstSubNo == model.EstSubNo && n.Dflag == false);
                if (dtEstimates == null || dtEstimateSubs == null)
                {
                    return ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage("jp", HelperMessage.CEST050S));
                }
                string first_y = model.ddlFirstYear!.Replace(CommonFunction.Right(model.ddlFirstYear!, 5), ")");
                string first_m = model.ddlFirstYear!;
                string syaken_y = model.ddlSyakenMonth!.Replace(CommonFunction.Right(model.ddlSyakenMonth!, 5), ")");
                string syaken_m = model.ddlSyakenYear!;
                dtEstimates.MakerName = model.Maker;
                dtEstimates.ModelName = model.CarNM;
                dtEstimates.GradeName = model.Grade;
                dtEstimates.Case = model.Kata;
                dtEstimates.ChassisNo = model.CarNo;
                dtEstimates.FirstRegYm = first_y + first_m;
                dtEstimates.CheckCarYm = CommonFunction.setCheckCarYm(syaken_y + syaken_m, Convert.ToBoolean(model.chkSyaken));
                dtEstimates.NowOdometer = model.Run.ToString() == string.Empty ? 0 : model.Run;
                dtEstimates.DispVol = model.Haiki;
                dtEstimates.Mission = model.Sft;
                dtEstimates.BodyColor = model.Color;
                dtEstimates.Equipment = model.Option;
                dtEstimates.BusinessHis = model.ddlCarReki;
                dtEstimates.AccidentHis = Convert.ToByte(model.raJrk);
                dtEstimateSubs.DispVolUnit = model.dispVolUnit;
                dtEstimateSubs.MilUnit = model.MilUnit;
                _unitOfWork.Estimates.Update(dtEstimates);
                _unitOfWork.EstimateSubs.Update(dtEstimateSubs);
                await _unitOfWork.CommitAsync();
                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage("jp", HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInputCar");
                return ResponseHelper.Error<int>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage("jp", HelperMessage.SICR001S));
            }
        }
    }
}