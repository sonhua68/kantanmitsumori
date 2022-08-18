using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.IDEEnitities
{
    public partial class MtIdeInspection
    {
        public int CarType { get; set; }
        public string FirstTerm { get; set; } = null!;
        public int AfterSecondTerm { get; set; }
        public int OilLiter { get; set; }
        public int OilUnitPrice { get; set; }
        public int ElementPrice { get; set; }
        public int BreakPrice { get; set; }
        public int ScheduleCheckPrice { get; set; }
        public int LegalCheckPrice { get; set; }
        public int InspectionPrice { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; } = null!;
    }
}
