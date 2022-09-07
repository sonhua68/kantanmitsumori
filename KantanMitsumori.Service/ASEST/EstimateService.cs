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
using Microsoft.Extensions.Options;
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

        public ResponseBase<List<TEstimate>> GetList(RequestInp requestInputCar)
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

        public ResponseBase<ResponseInp> GetDetail(RequestInp requestInputCar)
        {
            try
            {
                var estimates = _unitOfWork.Estimates.Query(n => n.EstNo == requestInputCar.EstNo && n.EstSubNo == requestInputCar.EstSubNo).ToList();
                var estimatesSub = _unitOfWork.EstimateSubs.Query(n => n.EstNo == requestInputCar.EstNo && n.EstSubNo == requestInputCar.EstSubNo).ToList();
                if (estimates == null || estimatesSub == null)
                {
                    return ResponseHelper.Error<ResponseInp>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                var dt = _helperMapper.JoinDataTables(_helperMapper.ToDataTable(estimates), _helperMapper.ToDataTable(estimatesSub),
               (row1, row2) =>
               row1.Field<string>("EstNo") == row2.Field<string>("EstNo") &&
                row1.Field<string>("EstSubNo") == row2.Field<string>("EstSubNo"));
                var data = _helperMapper.ConvertToList<ResponseInp>(dt).FirstOrDefault();
                if (data!.Rate == 0)
                {
                    var getDetail = _unitOfWork.UserDefs.GetSingle(n => n.UserNo == requestInputCar.UserNo);
                    if (getDetail != null)
                    {
                        data.Rate = getDetail.Rate;
                    }
                }
                return ResponseHelper.Ok<ResponseInp>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInputCar");
                return ResponseHelper.Error<ResponseInp>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

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
                string radDispVol = model.radDispVol! == "その他" ? "" : model.radDispVol!;
                string radMilUnit = model.radMilUnit! == "その他" ? "" : model.radMilUnit!;
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
                dtEstimateSubs.DispVolUnit = string.IsNullOrEmpty(model.dispVolUnit) ? radDispVol : model.dispVolUnit;
                dtEstimateSubs.MilUnit = string.IsNullOrEmpty(model.MilUnit) ? radMilUnit : model.MilUnit;
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

        public async Task<ResponseBase<int>> UpdateInpOption(RequestUpdateInpOption model)
        {
            try
            {
                TEstimate dtEstimates = _unitOfWork.Estimates.GetSingle(n => n.EstNo == model.EstNo && n.EstSubNo == model.EstSubNo && n.Dflag == false);
                if (dtEstimates == null)
                {
                    return ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                dtEstimates.OptionName1 = model.OptionName1;
                dtEstimates.OptionPrice1 = model.OptionPrice1;
                dtEstimates.OptionName2 = model.OptionName2;
                dtEstimates.OptionPrice2 = model.OptionPrice2;
                dtEstimates.OptionName3 = model.OptionName3;
                dtEstimates.OptionPrice3 = model.OptionPrice3;
                dtEstimates.OptionName4 = model.OptionName4;
                dtEstimates.OptionPrice4 = model.OptionPrice4;
                dtEstimates.OptionName5 = model.OptionName5;
                dtEstimates.OptionPrice5 = model.OptionPrice5;
                dtEstimates.OptionName6 = model.OptionName6;
                dtEstimates.OptionPrice6 = model.OptionPrice6;
                dtEstimates.OptionName7 = model.OptionName7;
                dtEstimates.OptionPrice7 = model.OptionPrice7;
                dtEstimates.OptionName8 = model.OptionName8;
                dtEstimates.OptionPrice8 = model.OptionPrice8;
                dtEstimates.OptionName9 = model.OptionName9;
                dtEstimates.OptionPrice9 = model.OptionPrice9;
                dtEstimates.OptionName10 = model.OptionName10;
                dtEstimates.OptionPrice10 = model.OptionPrice10;
                dtEstimates.OptionName11 = model.OptionName11;
                dtEstimates.OptionPrice11 = model.OptionPrice11;
                dtEstimates.OptionName12 = model.OptionName12;
                dtEstimates.OptionPrice12 = model.OptionPrice12;
                dtEstimates.OptionPriceAll = model.OptionPriceAll;
                _unitOfWork.Estimates.Update(dtEstimates);
                await _unitOfWork.CommitAsync();
                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInpOption");
                return ResponseHelper.Error<int>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public async Task<ResponseBase<int>> UpdateInpZeiHoken(RequestUpdateInpZeiHoken model)
        {
            try
            {
                TEstimate dtEstimates = _unitOfWork.Estimates.GetSingle(n => n.EstNo == model.EstNo && n.EstSubNo == model.EstSubNo && n.Dflag == false);
                TEstimateSub dtEstimateSubs = _unitOfWork.EstimateSubs.GetSingle(n => n.EstNo == model.EstNo && n.EstSubNo == model.EstSubNo && n.Dflag == false);
                if (dtEstimates == null || dtEstimateSubs == null)
                {
                    return ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                dtEstimates.AutoTax = model.MeiCarTax;
                dtEstimates.AcqTax = model.MeiGetTax;
                dtEstimates.WeightTax = model.MeiWeightTax;
                dtEstimates.DamageIns = model.MeiJibaiHoken;
                dtEstimates.OptionIns = model.MeiNiniHoken;
                dtEstimates.TaxInsAll = model.TaxInsAll;

                dtEstimateSubs.AutoTaxMonth = model.ddlCarTaxMonth;
                dtEstimateSubs.DamageInsMonth = model.ddlJibaiHokenMonth;
                dtEstimateSubs.AutoTaxEquivalent = model.MeiCarTaxEquivalent;
                dtEstimateSubs.DamageInsEquivalent = model.MeiJibaiHokenEquivalent;
                dtEstimateSubs.TaxInsEquivalentAll = model.TaxInsEquivalentAll;

                _unitOfWork.Estimates.Update(dtEstimates);
                _unitOfWork.EstimateSubs.Update(dtEstimateSubs);
                await _unitOfWork.CommitAsync();
                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateInpZeiHoken");
                return ResponseHelper.Error<int>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }
    }
}