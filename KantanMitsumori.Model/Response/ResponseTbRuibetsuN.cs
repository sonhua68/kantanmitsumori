using KantanMitsumori.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Response
{
    public class ResponseTbRuibetsuN
    {
        public int Code { get; set; }
        public string SetNumber { get; set; } = null!;
        public string ClassNumber { get; set; } = null!;
        public string Made { get; set; } = null!;
        public int MakerId { get; set; }
        public string MakerName { get; set; } = null!;
        public int ModelId { get; set; }
        public string ModelName { get; set; } = null!;
        public string GradeName { get; set; } = null!;
        public string RegularCase { get; set; } = null!;
        public string DispVol { get; set; } = null!;
        public int ShiftId { get; set; }
        public string Mission { get; set; } = null!;
        public string DriveTypeCode { get; set; } = null!;
        public int FuelCode { get; set; }
        public string FuelType { get; set; } = null!;
        public string FlgOptPs { get; set; } = null!;
        public string FlgOptPw { get; set; } = null!;
        public string FlgOptTv { get; set; } = null!;
        public string FlgOptNav { get; set; } = null!;
        public string FlgOptSht { get; set; } = null!;
        public string FlgOptSrf { get; set; } = null!;
        public string FlgOptAw { get; set; } = null!;
        public string FlgOptAbg { get; set; } = null!;
        public string FlgOptAbs { get; set; } = null!;
        public int TotalPages { get; set; }
    }

    public class ResponseTbRuibetsuNew
    {
        public int MakerId { get; set; }
        public string? ModelName { get; set; }
        public string? MakerName { get; set; }        
        public int GradeNameOrd { get; set; }
        public string? GradeName { get; set; }
        public int RegularCaseOrd { get; set; }
        public string? RegularCase { get; set; }
        public int DispVolOrd { get; set; }
        public string? Shift { get; set; }
        public string? DispVol { get; set; }
        public int DriveTypeCodeOrd { get; set; }
        public string? DriveTypeCode { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
    }
    public class ResponseSerEst
    {
        public string? EstNo { get; set; }
        public string? TradeDate { get; set; }
        public string? CustKName { get; set; }
        public string? CarName { get; set; }
        public int PageIndex { get;  set; }
        public int TotalPages { get;  set; }
    }


}
