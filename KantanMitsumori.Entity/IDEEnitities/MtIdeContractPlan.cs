using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.IDEEnitities
{
    public partial class MtIdeContractPlan
    {
        public int Id { get; set; }
        public string PlanName { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; } = null!;
    }
}
