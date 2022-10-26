﻿namespace KantanMitsumori.Model.Response
{
    public class EstimateIdeModel
    {
        public string EstNo { get; set; } = string.Empty;
        public string EstSubNo { get; set; } = string.Empty;
        public string EstUserNo { get; set; } = string.Empty;
        public int CarType { get; set; }
        public byte IsElectricCar { get; set; }
        public string FirstRegistration { get; set; } = string.Empty;
        public string InspectionExpirationDate { get; set; } = string.Empty;
        public string LeaseStartMonth { get; set; } = string.Empty;
        public int LeasePeriod { get; set; }
        public string LeaseExpirationDate { get; set; } = string.Empty;
        public int ContractPlanId { get; set; }
        public string ContractPlanName { get; set; } = string.Empty;
        public byte IsExtendedGuarantee { get; set; } = 99;
        public int InsuranceCompanyId { get; set; }
        public string InsuranceCompanyName { get; set; } = string.Empty;
        public int InsuranceFee { get; set; }
        public int DownPayment { get; set; }
        public int TradeInPrice { get; set; }
        public int FeeAdjustment { get; set; }
        public int MonthlyLeaseFee { get; set; }
        public int IdemitsuKosanFee { get; set; }
        public int SalesStoreFee { get; set; }
        public int Smasfee { get; set; }
        public int IdemitsuCreditFee { get; set; }
        public double Promotion { get; set; }
        public int PromotionFee { get; set; }
        public double ConsumptionTax { get; set; }
        public int NameChange { get; set; }
        public int FeeAdjustmentMax { get; set; }
        public int FeeAdjustmentMin { get; set; }
        public double Interest { get; set; }
        public int GuaranteeCharge { get; set; }
        public int MyMaintenancePrice { get; set; }
        public int CarTax { get; set; }
        public int LiabilityInsurance { get; set; }
        public int WeightTax { get; set; }
        public int LeaseProgress { get; set; }
        public byte IsApplyLease { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public bool IsData => !string.IsNullOrEmpty(EstNo);
    }
}
