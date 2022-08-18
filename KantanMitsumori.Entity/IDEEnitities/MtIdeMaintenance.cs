using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.IDEEnitities
{
    public partial class MtIdeMaintenance
    {
        public int CarType { get; set; }
        public int LeasePeriod { get; set; }
        public byte BeforeFirstInspection { get; set; }
        public int OilChangeCount { get; set; }
        public int ElementChangeCount { get; set; }
        public int BreakChangeCount { get; set; }
        public int ScheduleCheckCount { get; set; }
        public int LegalCheckCount { get; set; }
        public int InspectionCount { get; set; }
        public int MonthlyCharge { get; set; }
        public int ManagementFee { get; set; }
        public int MyMaintenancePrice { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; } = null!;
    }
}
