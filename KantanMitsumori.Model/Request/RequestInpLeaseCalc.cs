using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Request
{
    public class RequestInpLeaseCalc
    {
        public int CarType { get;set;}
        public int ElectricCar { get; set; }
        public int FirstTerm { get; set; }
        public int AfterSecondTerm { get; set; }
        public string? FirstReg { get; set; }
        public string? ExpiresDate { get; set; }
        public string? LeaseSttMonth { get; set; }
        public int ContractTimes { get; set; }
        public int LeasePeriod { get; set; }
        public string? LeaseExpirationDate { get; set; }
        public int ContractPlan { get; set; }
        public int InsurExpanded { get; set; }
        public int OptionInsurance { get; set; }
        public int InsuranceCompany { get; set; }
        public int DiffMonth { get; set; }
        public int CalcDiffMonth { get; set; }
        public string? LeaseSttDay { get; set; }
        public int InsuranceFee { get; set; }
        public int PrePay { get; set; }
        public int TradeIn { get; set; }
        public int AdjustFee { get; set; }
    }
}
