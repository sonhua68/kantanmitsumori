using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.IDEEnitities
{
    public partial class MtIdeWeightTax
    {
        public int CarType { get; set; }
        public int ElapsedYearsFrom { get; set; }
        public int ElapsedYearsTo { get; set; }
        public int WeightTax { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; } = null!;
    }
}
