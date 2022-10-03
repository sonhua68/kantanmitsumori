using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Response
{
    public class ResponseInpLeaseCalc
    {

    }
    public class ResponseCarType
    {
        public int CarType { get; set; }
        public string? CarTypeName { get; set; }
    }
    public class ResponseContractPlan
    {
        public int CarType { get; set; }
        public string? CarTypeName { get; set; }
    }
    public class ResponseVolInsurance
    {
        public int ID { get; set; }
        public string? CompanyName { get; set; }
    }
}
