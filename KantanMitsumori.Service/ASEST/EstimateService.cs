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
using System.Data;

namespace KantanMitsumori.Service
{
    public class EstimateService : IEstimateService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        private readonly HelperMapper _helperMapper;




        public EstimateService(IMapper mapper, ILogger<AppService> logger, IUnitOfWork unitOfWork, HelperMapper helperMapper)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _helperMapper = helperMapper;

        }

        public async Task<ResponseBase<int>> Create(TEstimate model)
        {
            ResponseBase<int> iResult = new ResponseBase<int>();
            try
            {
                _unitOfWork.Estimates.Add(model);
                await _unitOfWork.CommitAsync();
                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateTEstimate");
                return ResponseHelper.Error<int>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public ResponseBase<List<TEstimate>> GetList(RequestInputCar requestInputCar)
        {
            try
            {
                ResponseBase<List<TEstimate>> iResult = new ResponseBase<List<TEstimate>>();
                var estimatesList = _unitOfWork.Estimates.Query(n => n.EstNo == requestInputCar.EstNo && n.EstSubNo == requestInputCar.EstSubNo).Select(i => _mapper.Map<TEstimate>(i)).ToList();
                if (estimatesList == null)
                {
                    return ResponseHelper.Error<List<TEstimate>>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                return ResponseHelper.Ok<List<TEstimate>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), estimatesList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInputCar");
                return ResponseHelper.Error<List<TEstimate>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

            }
            throw new NotImplementedException();
        }

        public ResponseBase<ResponseInputCar> GetDetail(RequestInputCar requestInputCar)
        {
            try
            {
                var estimates = _unitOfWork.Estimates.Query(n => n.EstNo == requestInputCar.EstNo && n.EstSubNo == requestInputCar.EstSubNo).ToList();
                var estimatesSub = _unitOfWork.EstimateSubs.Query(n => n.EstNo == requestInputCar.EstNo && n.EstSubNo == requestInputCar.EstSubNo).ToList();
                if (estimates == null || estimatesSub == null)
                {
                    return ResponseHelper.Error<ResponseInputCar>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                var dt = _helperMapper.JoinDataTables(_helperMapper.ToDataTable(estimates), _helperMapper.ToDataTable(estimatesSub),
               (row1, row2) =>
               row1.Field<string>("EstNo") == row2.Field<string>("EstNo") &&
                row1.Field<string>("EstSubNo") == row2.Field<string>("EstSubNo"));
                var data = _helperMapper.ConvertToList<ResponseInputCar>(dt).FirstOrDefault();
                return ResponseHelper.Ok<ResponseInputCar>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInputCar");
                return ResponseHelper.Error<ResponseInputCar>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

            }
            throw new NotImplementedException();
        }

        public async Task<ResponseBase<int>> UpdateInputCar(RequestUpdateInputCar model)
        {
            try
            {
                TEstimate dtEstimates = _unitOfWork.Estimates.GetSingle(n => n.EstNo == model.EstNo && n.EstSubNo == model.EstSubNo && n.Dflag == false);
                TEstimateSub dtEstimateSubs = _unitOfWork.EstimateSubs.GetSingle(n => n.EstNo == model.EstNo && n.EstSubNo == model.EstSubNo && n.Dflag == false);
                if (dtEstimates == null || dtEstimateSubs == null)
                {
                    return ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                string firstRegYm = ""; string checkCarYm = ""; string firstm = "";
                if (!string.IsNullOrEmpty(model.ddlFirstYear))
                {
                    var firstMonth = 0;
                    if (!string.IsNullOrEmpty(model.ddlFirstMonth))
                    {
                        firstMonth = Convert.ToInt32(model.ddlFirstMonth!);
                        firstm = firstMonth < 10 ? "0" + firstMonth : firstMonth.ToString();
                    }
                    firstRegYm = CommonFunction.Right(model.ddlFirstYear!, 5).Replace(")", firstm);
                }
                if (model.chkSyaken == 1)
                {
                    checkCarYm = model.chkSyaken == 1 ? "無し" : "";
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.ddlSyakenYear) || !string.IsNullOrEmpty(model.ddlSyakenMonth))
                    {
                        var syakenMonth = Convert.ToInt32(model.ddlSyakenMonth!);
                        string syakenm = syakenMonth < 10 ? "0" + syakenMonth : syakenMonth.ToString();
                        checkCarYm = CommonFunction.Right(model.ddlSyakenYear!, 5).Replace(")", syakenm);
                    }
                }
                dtEstimates.MakerName = model.Maker;
                dtEstimates.ModelName = model.CarNM;
                dtEstimates.GradeName = model.Grade;
                dtEstimates.Case = model.Kata;
                dtEstimates.ChassisNo = model.CarNo;
                dtEstimates.FirstRegYm = firstRegYm;
                dtEstimates.CheckCarYm = CommonFunction.setCheckCarYm(checkCarYm, Convert.ToBoolean(model.chkSyaken));
                dtEstimates.NowOdometer = model.Run.ToString() == string.Empty ? 0 : model.Run;
                dtEstimates.DispVol = model.Haiki;
                dtEstimates.Mission = model.Sft;
                dtEstimates.BodyColor = model.Color;
                dtEstimates.Equipment = model.Option;
                dtEstimates.BusinessHis = model.ddlCarReki;
                dtEstimates.AccidentHis = Convert.ToByte(model.raJrk);
                dtEstimateSubs.DispVolUnit = string.IsNullOrEmpty(model.dispVolUnit) ? model.radDispVol : model.dispVolUnit;
                dtEstimateSubs.MilUnit = string.IsNullOrEmpty(model.MilUnit) ? model.radMilUnit : model.MilUnit;
                _unitOfWork.Estimates.Update(dtEstimates);
                _unitOfWork.EstimateSubs.Update(dtEstimateSubs);
                await _unitOfWork.CommitAsync();
                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInputCar");
                return ResponseHelper.Error<int>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public async Task<ResponseBase<int>> UpdateInpHanbaiten(RequestUpdateInpHanbaiten model)
        {
            try
            {
                TEstimate dtEstimates = _unitOfWork.Estimates.GetSingle(n => n.EstNo == model.EstNo && n.EstSubNo == model.EstSubNo && n.Dflag == false);
                if (dtEstimates == null)
                {
                    return ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                dtEstimates.ShopNm = model.HanName;
                dtEstimates.ShopAdr = model.HanAdd;
                dtEstimates.ShopTel = model.Tel;
                dtEstimates.EstTanName = model.TantoName;
                dtEstimates.SekininName = model.Sekinin;
                _unitOfWork.Estimates.Update(dtEstimates);
                await _unitOfWork.CommitAsync();
                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInpHanbaiten");
                return ResponseHelper.Error<int>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }
    }
}