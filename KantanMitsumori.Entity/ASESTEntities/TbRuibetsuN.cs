using System;
using System.Collections.Generic;

namespace KantanMitsumori.DataAccess
{
    public partial class TbRuibetsuN
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
    }
}
