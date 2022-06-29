using System;
using System.Collections.Generic;

namespace KantanMitsumori.DataAccess
{
    public partial class MAquisitionTax
    {
        public int AquisitionTaxId { get; set; }
        public byte? CarType { get; set; }
        public string? PassedDisp { get; set; }
        public double? PassedYearFrom { get; set; }
        public double? PassedYearTo { get; set; }
        public double? RemainRate { get; set; }
        public DateTime? Rdate { get; set; }
        public DateTime? Udate { get; set; }
        public bool? Dflag { get; set; }
    }
}
