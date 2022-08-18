using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.ASESTEntities
{
    public partial class MCar
    {
        public int CarId { get; set; }
        public int MakerId { get; set; }
        public int ModelId { get; set; }
        public int? CaseId { get; set; }
        public int GradeId { get; set; }
        public string RegularCase { get; set; } = null!;
        public string PublicCase { get; set; } = null!;
        public string GradeName { get; set; } = null!;
        public string SetNumber { get; set; } = null!;
        public string ClassNumber { get; set; } = null!;
        public string? DriveTypeCode { get; set; }
        public string? DispVol { get; set; }
        public string? Mission { get; set; }
        public string? Weight { get; set; }
        public string? NewPrice { get; set; }
        public string? SalesStart { get; set; }
        public string? SalesFinish { get; set; }
        public DateTime Rdate { get; set; }
        public DateTime Udate { get; set; }
        public byte Dflag { get; set; }
    }
}
