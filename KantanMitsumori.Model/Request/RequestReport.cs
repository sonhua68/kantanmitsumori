using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Request
{
    public class RequestReport
    {
        public string EstNo { get; set; } = "";
        public string EstSubNo { get; set; } = "";
        public ReportType ReportType { get; set; } = ReportType.Estimate;
        public string CustNm_forPrint { get; set; } = "";
        public string CustZip_forPrint { get; set; } = "";
        public string CustAdr_forPrint { get; set; } = "";
        public string CustTel_forPrint { get; set; } = "";        

    }

    public enum ReportType : int
    {
        Estimate = 1,
        Order = 2,
        LeaseEstimate = 3,
        LeaseOrder = 4
    }

}
