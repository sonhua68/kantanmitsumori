using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.ASESTEntities
{
    public partial class MSelfInsurance
    {
        public int SelfInsuranceId { get; set; }
        public byte? CarType { get; set; }
        public byte? RemainInspection { get; set; }
        public int? SelfInsurance { get; set; }
        public DateTime? Rdate { get; set; }
        public DateTime? Udate { get; set; }
        public bool? Dflag { get; set; }
    }
}
