using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.IDEEnitities
{
    public partial class MtIdeConsumptionTax
    {
        public double ConsumptionTax { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; } = null!;
    }
}
