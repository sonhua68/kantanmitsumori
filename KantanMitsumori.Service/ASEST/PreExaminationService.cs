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
        private readonly ILogger _logger;

        private readonly CommonEstimate _commonEst;
        private readonly CommonIDE _commonIDE;
        private readonly IUnitOfWork _unitOfWork;

        public PreExaminationService(ILogger<InpCustKanaService> logger, CommonEstimate commonEst, CommonIDE commonIDE, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _commonEst = commonEst;
            _commonIDE = commonIDE;
            _unitOfWork = unitOfWork;
        }

        public ResponseBase<ResponsePreExamination> GetInfoPreExamination(string estNo, string estSubNo)
        {
            try
            {
                // 見積書データ取得
                var estData = _commonEst.GetEst_EstSubData(estNo, estSubNo);

                // 見積書データ取得
                var estIdeData = _commonEst.GetEstIDEData(estNo, estSubNo);

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

                var requestModel = new RequestPreExaminationModel
                {
                    EstModel = estData,
                    EstIDEModel = estIdeData,
                    MemberIDE = memberData,
                    CarTypeIDE = carType.CarTypeName,
                    CompanyName = voluntaryInsurance.CompanyName,
                    PlanName = contractPlan.PlanName,
                    GuaranteeFee = guaranteeFee.GuaranteeCharge,
                    GuaranteeFeeEx = guaranteeFeeEx.GuaranteeCharge
                };

                // set data model 
                var model = ToModel(requestModel);

                return ResponseHelper.Ok<ResponsePreExamination>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002), model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetInfoPreExamination");
                return ResponseHelper.Error<ResponsePreExamination>(HelperMessage.ISYS010I, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.ISYS010I));
            }
        }
        public async Task<ResponseBase<int>> UpdatePreExamination(LogSession LogSession)
        {
            try
            {
                var dtEstimateIdes = _unitOfWork.EstimateIdes.GetSingle(n => n.EstNo == LogSession.sesEstNo && n.EstSubNo == LogSession.sesEstSubNo);
                if (dtEstimateIdes == null)
                {
                    return ResponseHelper.Error<int>(HelperMessage.CEST050S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.CEST050S));
                }
                dtEstimateIdes.IsApplyLease = 1;
                _unitOfWork.EstimateIdes.Update(dtEstimateIdes);
                await _unitOfWork.CommitAsync();
                return ResponseHelper.Ok<int>(HelperMessage.I0002, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.I0002));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdatePreExamination");
                return ResponseHelper.Error<int>(HelperMessage.SICR001S, KantanMitsumoriUtil.GetMessage(CommonConst.language_JP, HelperMessage.SICR001S));
            }
        }
        private ResponsePreExamination ToModel(RequestPreExaminationModel model)
        {
            // set binding data
            var responseModel = new ResponsePreExamination
            {
                EstNo = model.EstModel.EstNo,
                EstSubNo = model.EstModel.EstSubNo,
                leaseEstimateNo = model.EstIDEModel.EstNo,
                ssCode = model.MemberIDE.Ssnumber,
                ssName = model.MemberIDE.Ssname,
                ssStaffName = model.EstModel.EstTanName,
                ssMailAddress = model.MemberIDE.Ssmail,
                vendorName = model.MemberIDE.StoreName,
                mkNm = model.EstModel.MakerName,
                commodityName = model.EstModel.ModelName,
                gradeName = model.EstModel.GradeName,
                bcolorNm = model.EstModel.BodyColor,
                fuelName = model.EstModel.FuelName,
                haiki = model.EstIDEModel.IsElectricCar == 1 ? "0" : model.EstModel.DispVol,
                missionCd = model.EstModel.Mission,
                wheelDriveName = model.EstModel.DriveName,
                doorCd = model.EstModel.CarDoors,
                body = model.EstModel.BodyName,
                teiinA = model.EstModel.Capacity,
                ninKata = model.EstModel.Case,
                colorCost = model.EstModel.ChassisNo,
                reopName1 = model.EstIDEModel.CarType == 4 ? 1 : 2,
                Mileage = model.EstModel.MilUnit.Contains("千km") ? (model.EstModel.NowOdometer * 1000).ToString() + "km" : model.EstModel.NowOdometer.ToString() + model.EstModel.MilUnit,
                carType = model.CarTypeIDE,
                firstMonth = model.EstIDEModel.FirstRegistration,
                reopGroupName1 = model.EstIDEModel.InspectionExpirationDate,
                reopCost1 = model.EstIDEModel.LeaseStartMonth,
                leasePeriod = model.EstIDEModel.LeasePeriod,
                reopGroupName2 = model.EstIDEModel.LeaseExpirationDate,
                maintenanceName = model.PlanName,
                maothlyFeeT = model.EstIDEModel.MyMaintenancePrice,
                guaranteeFee = model.GuaranteeFee,
                guaranteeFeeOne = model.EstIDEModel.IsExtendedGuarantee == 0 ? "あり" : "なし",
                guaranteeFeeEx = model.GuaranteeFeeEx,
                insuranceWhich = model.EstIDEModel.InsuranceCompanyId == (-1) ? "なし" : "あり",
                carInsCompany = model.CompanyName,
                carInsPrice = model.EstIDEModel.InsuranceFee,
                carPriceTax = model.EstModel.CarPrice,
                nebiki = model.EstModel.Discount,
                otherTax = model.EstModel.Sonota,
                carMaintenance = model.EstModel.SyakenNew > 0 ? model.EstModel.SyakenNew : (model.EstModel.SyakenNew == 0 && model.EstModel.SyakenZok > 0) ? model.EstModel.SyakenZok : 0,
                fOpCost = (model.EstModel.OptionPrice1 + model.EstModel.OptionPrice2 + model.EstModel.OptionPrice3 + model.EstModel.OptionPrice4 + model.EstModel.OptionPrice5 + model.EstModel.OptionPrice6 + model.EstModel.OptionPrice7 + model.EstModel.OptionPrice8 + model.EstModel.OptionPrice9 + model.EstModel.OptionPrice10 + model.EstModel.OptionPrice11 + model.EstModel.OptionPrice12),
                carSellPriceT = model.EstModel.SalesSum,
                automobileTax = model.EstIDEModel.CarTax,
                environmental = model.EstModel.AcqTax,
                weightTax = model.EstIDEModel.WeightTax,
                liabilityInsur = model.EstIDEModel.LiabilityInsurance,
                totalTaxEx = model.EstModel.TaxInsAll,
                legalCustody = model.EstModel.TaxFreeAll,
                procedureAgency = model.EstModel.TaxCostAll,
                recyclingCost = model.EstModel.TaxFreeRecycle,
                custodyStatutory = model.EstModel.TaxFreeAll - model.EstModel.TaxFreeRecycle,
                cashSellingT = model.EstModel.CarSaleSum,
                reopName2 = model.EstModel.Equipment.Replace(" ", "　"),
                fOp1ComName = model.EstModel.OptionName1,
                fOp1Cost = model.EstModel.OptionPrice1,
                fOp2ComName = model.EstModel.OptionName2,
                fOp2Cost = model.EstModel.OptionPrice2,
                fOp3ComName = model.EstModel.OptionName3,
                fOp3Cost = model.EstModel.OptionPrice3,
                fOp4ComName = model.EstModel.OptionName4,
                fOp4Cost = model.EstModel.OptionPrice4,
                fOp5ComName = model.EstModel.OptionName5,
                fOp5Cost = model.EstModel.OptionPrice5,
                fOp6ComName = model.EstModel.OptionName6,
                fOp6Cost = model.EstModel.OptionPrice6,
                fOp7ComName = model.EstModel.OptionName7,
                fOp7Cost = model.EstModel.OptionPrice7,
                fOp8ComName = model.EstModel.OptionName8,
                fOp8Cost = model.EstModel.OptionPrice8,
                fOp9ComName = model.EstModel.OptionName9,
                fOp9Cost = model.EstModel.OptionPrice9,
                fOp10ComName = model.EstModel.OptionName10,
                fOp10Cost = model.EstModel.OptionPrice10,
                fOp11ComName = model.EstModel.OptionName11,
                fOp11Cost = model.EstModel.OptionPrice11,
                fOp12ComName = model.EstModel.OptionName12,
                fOp12Cost = model.EstModel.OptionPrice12,
                dPaymentPriceTax = model.EstIDEModel.DownPayment,
                TITradeIn = model.EstIDEModel.TradeInPrice,
                DP_TIT = model.EstIDEModel.DownPayment + model.EstIDEModel.TradeInPrice,
                leasePriceInTax = model.EstIDEModel.MonthlyLeaseFee,
                costAdjustPriceT = model.EstIDEModel.FeeAdjustment,
                examOrderType = model.EstIDEModel.LeaseProgress == 2 ? "確定" : model.EstIDEModel.LeaseProgress == 1 ? "予定" : ""
            };

            return responseModel;
        }
    }
}
