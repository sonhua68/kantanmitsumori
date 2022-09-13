using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Entity.ASESTSQL
{
    public class TbRuibetsuEntity
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
    }
}
