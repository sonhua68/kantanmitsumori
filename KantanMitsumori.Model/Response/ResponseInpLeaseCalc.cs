using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Response
{
    public class ResponseInpLeaseCalc
    {
        public List<string>? ListUILog { get; set; }
        public bool IsError { get; set; }
        public int IsShowButton { get; set; } = 0;
        public string? PriceEnd { get; set; }
        public int PriceLeaseFeeLowerLimit { get; set; }

    }
    public class ResponseCarType
    {
        public int CarType { get; set; }
        public string? CarTypeName { get; set; }
    }
    public class ResponseContractPlan
    {
        public int ID { get; set; }
        public string? PlanName { get; set; }
    }
    public class ResponseVolInsurance
    {
        public int ID { get; set; }
        public string? CompanyName { get; set; }
    }
    public class ResponseFirstAfterSecondTerm
    {
        public int FirstTerm { get; set; }
        public int? AfterSecondTerm { get; set; }
    }
    public class ResponseUnitPriceRatesLimit
    {
        public int LowerLimit { get; set; }
        public int UpperLimit { get; set; }
        public int UnitPrice { get; set; }
  
    }
}
