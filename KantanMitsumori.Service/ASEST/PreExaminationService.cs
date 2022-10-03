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
    public class PreExaminationService : IPreExaminationService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        private CommonEstimate _commonEst;
        private CommonIDE _commonIDE;

        public PreExaminationService(IMapper mapper, ILogger<InpCustKanaService> logger, IUnitOfWork unitOfWork, CommonEstimate commonEst, CommonIDE commonIDE)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _commonEst = commonEst;
            _commonIDE = commonIDE;
        }

        public ResponseBase<ResponsePreExamination> GetInfoPreExamination(string estNo, string estSubNo)
        {
            try
            {
                // 見積書データ取得
                var estData = _commonEst.getEst_EstSubData(estNo, estSubNo);

                // 見積書データ取得
                var estIdeData = _commonEst.getEstIDEData(estNo, estSubNo);

                if (string.IsNullOrEmpty(estData.EstNo) || string.IsNullOrEmpty(estIdeData.EstNo))
                {
                    return ResponseHelper.Error<ResponsePreExamination>(HelperMessage.SMAI014D, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SMAI014D));
                }

                // get member
                var memberData = _commonIDE.getMember(estIdeData.EstUserNo);
                // get car type
                var carType = _commonIDE.getCarType(estIdeData.CarType);
                // get company name
                var voluntaryInsurance = _commonIDE.getVoluntaryInsurance(estIdeData.InsuranceCompanyId);
                // get contract plan
                var contractPlan = _commonIDE.getContractPlan(estIdeData.ContractPlanId);
                // get GuaranteeCharge years = 1
                var guaranteeFee = _commonIDE.getGuarantee(1);
                // get GuaranteeCharge years = 2
                var guaranteeFeeEx = _commonIDE.getGuarantee(2);

                var requestModel = new RequestPreExaminationModel();
                requestModel.EstModel = estData;
                requestModel.EstIDEModel = estIdeData;
                requestModel.MemberIDE = memberData;
                requestModel.CarTypeIDE = carType.CarTypeName;
                requestModel.CompanyName = voluntaryInsurance.CompanyName;
                requestModel.PlanName = contractPlan.PlanName;
                requestModel.GuaranteeFee = guaranteeFee.GuaranteeCharge;
                requestModel.GuaranteeFeeEx = guaranteeFeeEx.GuaranteeCharge;

                // set data model 
                var model = ToModel(requestModel);

                return ResponseHelper.Ok<ResponsePreExamination>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetInfoPreExamination");
                return ResponseHelper.Error<ResponsePreExamination>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }

        private ResponsePreExamination ToModel(RequestPreExaminationModel model)
        {
            // set binding data
            var responseModel = new ResponsePreExamination();
            responseModel.EstNo = model.EstModel.EstNo;
            responseModel.EstSubNo = model.EstModel.EstSubNo;
            responseModel.leaseEstimateNo = model.EstIDEModel.EstNo;
            responseModel.ssCode = model.MemberIDE.Ssnumber;
            responseModel.ssName = model.MemberIDE.Ssname;
            responseModel.ssStaffName = model.EstModel.EstTanName;
            responseModel.ssMailAddress = model.MemberIDE.Ssmail;
            responseModel.vendorName = model.MemberIDE.StoreName;
            responseModel.mkNm = model.EstModel.MakerName;
            responseModel.commodityName = model.EstModel.ModelName;
            responseModel.gradeName = model.EstModel.GradeName;
            responseModel.bcolorNm = model.EstModel.BodyColor;
            responseModel.fuelName = model.EstModel.FuelName;
            responseModel.haiki = model.EstIDEModel.IsElectricCar == 1 ? "0" : model.EstModel.DispVol;
            responseModel.missionCd = model.EstModel.Mission;
            responseModel.wheelDriveName = model.EstModel.DriveName;
            responseModel.doorCd = model.EstModel.CarDoors;
            responseModel.body = model.EstModel.BodyName;
            responseModel.teiinA = model.EstModel.Capacity;
            responseModel.ninKata = model.EstModel.Case;
            responseModel.colorCost = model.EstModel.ChassisNo;
            responseModel.reopName1 = model.EstIDEModel.CarType == 4 ? 1 : 2;
            responseModel.Mileage = model.EstModel.MilUnit.Contains("千km") ? (model.EstModel.NowOdometer * 1000).ToString() + "km" : model.EstModel.NowOdometer.ToString() + model.EstModel.MilUnit;
            responseModel.carType = model.CarTypeIDE;
            responseModel.firstMonth = model.EstIDEModel.FirstRegistration;
            responseModel.reopGroupName1 = model.EstIDEModel.InspectionExpirationDate;
            responseModel.reopCost1 = model.EstIDEModel.LeaseStartMonth;
            responseModel.leasePeriod = model.EstIDEModel.LeasePeriod;
            responseModel.reopGroupName2 = model.EstIDEModel.LeaseExpirationDate;
            responseModel.maintenanceName = model.PlanName;
            responseModel.maothlyFeeT = model.EstIDEModel.MyMaintenancePrice;
            responseModel.guaranteeFee = model.GuaranteeFee;
            responseModel.guaranteeFeeOne = model.EstIDEModel.IsExtendedGuarantee == 0 ? "あり" : "なし";
            responseModel.guaranteeFeeEx = model.GuaranteeFeeEx;
            responseModel.insuranceWhich = model.EstIDEModel.InsuranceCompanyId == (-1) ? "なし" : "あり";
            responseModel.carInsCompany = model.CompanyName;
            responseModel.carInsPrice = model.EstIDEModel.InsuranceFee;
            responseModel.carPriceTax = model.EstModel.CarPrice;
            responseModel.nebiki = model.EstModel.Discount;
            responseModel.otherTax = model.EstModel.Sonota;
            responseModel.carMaintenance = model.EstModel.SyakenNew > 0 ? model.EstModel.SyakenNew : (model.EstModel.SyakenNew == 0 && model.EstModel.SyakenZok > 0) ? model.EstModel.SyakenZok : 0;
            responseModel.fOpCost = (model.EstModel.OptionPrice1 + model.EstModel.OptionPrice2 + model.EstModel.OptionPrice3 + model.EstModel.OptionPrice4 + model.EstModel.OptionPrice5 + model.EstModel.OptionPrice6 + model.EstModel.OptionPrice7 + model.EstModel.OptionPrice8 + model.EstModel.OptionPrice9 + model.EstModel.OptionPrice10 + model.EstModel.OptionPrice11 + model.EstModel.OptionPrice12);
            responseModel.carSellPriceT = model.EstModel.SalesSum;
            responseModel.automobileTax = model.EstIDEModel.CarTax;
            responseModel.environmental = model.EstModel.AcqTax;
            responseModel.weightTax = model.EstIDEModel.WeightTax;
            responseModel.liabilityInsur = model.EstIDEModel.LiabilityInsurance;
            responseModel.totalTaxEx = model.EstModel.TaxInsAll;
            responseModel.legalCustody = model.EstModel.TaxFreeAll;
            responseModel.procedureAgency = model.EstModel.TaxCostAll;
            responseModel.recyclingCost = model.EstModel.TaxFreeRecycle;
            responseModel.custodyStatutory = model.EstModel.TaxFreeAll - model.EstModel.TaxFreeRecycle;
            responseModel.cashSellingT = model.EstModel.CarSaleSum;
            responseModel.reopName2 = model.EstModel.Equipment.Replace(" ", "　");
            responseModel.fOp1ComName = model.EstModel.OptionName1;
            responseModel.fOp1Cost = model.EstModel.OptionPrice1;
            responseModel.fOp2ComName = model.EstModel.OptionName2;
            responseModel.fOp2Cost = model.EstModel.OptionPrice2;
            responseModel.fOp3ComName = model.EstModel.OptionName3;
            responseModel.fOp3Cost = model.EstModel.OptionPrice3;
            responseModel.fOp4ComName = model.EstModel.OptionName4;
            responseModel.fOp4Cost = model.EstModel.OptionPrice4;
            responseModel.fOp5ComName = model.EstModel.OptionName5;
            responseModel.fOp5Cost = model.EstModel.OptionPrice5;
            responseModel.fOp6ComName = model.EstModel.OptionName6;
            responseModel.fOp6Cost = model.EstModel.OptionPrice6;
            responseModel.fOp7ComName = model.EstModel.OptionName7;
            responseModel.fOp7Cost = model.EstModel.OptionPrice7;
            responseModel.fOp8ComName = model.EstModel.OptionName8;
            responseModel.fOp8Cost = model.EstModel.OptionPrice8;
            responseModel.fOp9ComName = model.EstModel.OptionName9;
            responseModel.fOp9Cost = model.EstModel.OptionPrice9;
            responseModel.fOp10ComName = model.EstModel.OptionName10;
            responseModel.fOp10Cost = model.EstModel.OptionPrice10;
            responseModel.fOp11ComName = model.EstModel.OptionName11;
            responseModel.fOp11Cost = model.EstModel.OptionPrice11;
            responseModel.fOp12ComName = model.EstModel.OptionName12;
            responseModel.fOp12Cost = model.EstModel.OptionPrice12;
            responseModel.dPaymentPriceTax = model.EstIDEModel.DownPayment;
            responseModel.TITradeIn = model.EstIDEModel.TradeInPrice;
            responseModel.DP_TIT = model.EstIDEModel.DownPayment + model.EstIDEModel.TradeInPrice;
            responseModel.leasePriceInTax = model.EstIDEModel.MonthlyLeaseFee;
            responseModel.costAdjustPriceT = model.EstIDEModel.FeeAdjustment;
            responseModel.examOrderType = model.EstIDEModel.LeaseProgress == 2 ? "確定" : model.EstIDEModel.LeaseProgress == 1 ? "予定" : "";

            return responseModel;
        }
    }
}
