using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.ASESTEntities
{
    public partial class MCarTax
    {
        public int CarTaxId { get; set; }
        public string? ExaustDisp { get; set; }
        public int? ExaustFrom { get; set; }
        public int? ExaustTo { get; set; }
        public int? YearAmount { get; set; }
        public DateTime? Rdate { get; set; }
        public DateTime? Udate { get; set; }
        public bool? Dflag { get; set; }
    }
}
