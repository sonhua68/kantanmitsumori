using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.IDEEnitities
{
    public partial class MtIdeInterest
    {
        public int LeasePeriodFrom { get; set; }
        public int LeasePeriodTo { get; set; }
        public double Interest { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; } = null!;
    }
}
