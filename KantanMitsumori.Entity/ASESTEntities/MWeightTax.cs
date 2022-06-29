using System;
using System.Collections.Generic;

namespace KantanMitsumori.DataAccess
{
    public partial class MWeightTax
    {
        public int WeightTaxId { get; set; }
        public byte? CarType { get; set; }
        public int? CarWeightFrom { get; set; }
        public int? CarWeightTo { get; set; }
        public int? WeightTax { get; set; }
        public DateTime? Rdate { get; set; }
        public DateTime? Udate { get; set; }
        public bool? Dflag { get; set; }
    }
}
