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
        private CommonCalLease _calLease;
        private List<string> lstWriteLog = new List<string>();

        public InpLeaseCalcService(IMapper mapper, CommonEstimate commonEst, IUnitOfWorkIDE unitOfWorkIDE, ILogger<InpLeaseCalcService> logger, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonEst = commonEst;
            _unitOfWorkIDE = unitOfWorkIDE;
            //_calLease = new CommonCalLease(_logger, _unitOfWorkIDE, lstWriteLog);
        }

        public async Task<ResponseBase<List<ResponseCarType>>> GetCarType()
        {
            try
            {
                var data = _unitOfWorkIDE.CarTypes.GetAll().Select(i => _mapper.Map<ResponseCarType>(i)).ToList();
                return ResponseHelper.Ok<List<ResponseCarType>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCarType");
                return ResponseHelper.Error<List<ResponseCarType>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public async Task<ResponseBase<List<ResponseContractPlan>>> GetContractPlan()
        {
            try
            {
                var data = _unitOfWorkIDE.ContractPlans.GetAll().Select(i => _mapper.Map<ResponseContractPlan>(i)).ToList();
                return ResponseHelper.Ok<List<ResponseContractPlan>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetContractPlan");
                return ResponseHelper.Error<List<ResponseContractPlan>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public async Task<ResponseBase<List<ResponseFirstAfterSecondTerm>>> GetFirstAfterSecondTerm(int carType)
        {
            try
            {
                var data = _unitOfWorkIDE.Inspections.Query(n => n.CarType == carType).Select(i => _mapper.Map<ResponseFirstAfterSecondTerm>(i)).ToList();
                return ResponseHelper.Ok<List<ResponseFirstAfterSecondTerm>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFirstAfterSecondTerm");
                return ResponseHelper.Error<List<ResponseFirstAfterSecondTerm>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public async Task<ResponseBase<ResponseUnitPriceRatesLimit>> GetUnitPriceRatesLimit()
        {
            try
            {
                var data = new ResponseUnitPriceRatesLimit();
                var UnitPrice = _unitOfWorkIDE.UnitPrices.GetAll().FirstOrDefault()!.UnitPrice;
                var dt = _unitOfWorkIDE.FeeAdjustments.GetAll().FirstOrDefault()!;
                data.UnitPrice = UnitPrice;
                data.LowerLimit = dt.LowerLimit;
                data.UpperLimit = dt.UpperLimit;
                return ResponseHelper.Ok<ResponseUnitPriceRatesLimit>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCarType");
                return ResponseHelper.Error<ResponseUnitPriceRatesLimit>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public async Task<ResponseBase<List<ResponseVolInsurance>>> GetVolInsurance()
        {
            try
            {
                var data = _unitOfWorkIDE.VoluntaryInsurances.GetAll().Select(i => _mapper.Map<ResponseVolInsurance>(i)).ToList();
                return ResponseHelper.Ok<List<ResponseVolInsurance>>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetVolInsurance");
                return ResponseHelper.Error<List<ResponseVolInsurance>>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        public async Task<ResponseBase<ResponseInpLeaseCalc>> InpLeaseCal(RequestInpLeaseCalc model, LogToken logToken)
        {
            try
            {
                var data = new ResponseInpLeaseCalc();
                var estimates = _unitOfWork.Estimates.GetSingle(n => n.EstNo == logToken.sesEstNo && n.EstSubNo == logToken.sesEstSubNo);
                var estimateSubs = _unitOfWork.EstimateSubs.GetSingle(n => n.EstNo == logToken.sesEstNo && n.EstSubNo == logToken.sesEstSubNo);
                if (estimates == null || estimateSubs == null)
                {
                    return ResponseHelper.Error<ResponseInpLeaseCalc>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                // 4-1 
                var consumptionTax = _calLease.GetConsumptionTax();
                //// '4-2 get Price
                //var dPrice = _calLease.GetPrice(estimates.SalesSum, estimates.TaxInsAll, estimates.TaxFreeAll);
                //// '4-3 VehicleTaxPrice
                //var monthlyPrice = _calLease.getVehicleTaxWithinTheTerm(estimates.AutoTax, estimateSubs.DispVolUnit, estimates.DispVol);
                data.log = _calLease._lstWriteLog;
                return ResponseHelper.Ok<ResponseInpLeaseCalc>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InpLeaseCal");
                return ResponseHelper.Error<ResponseInpLeaseCalc>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));

            }
        }

    }
}