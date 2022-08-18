using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.IDEEnitities
{
    public partial class MtIdeCarTax
    {
        public int CarType { get; set; }
        public int FirstRegistrationDateFrom { get; set; }
        public int FirstRegistrationDateTo { get; set; }
        public int ElapsedYearsFrom { get; set; }
        public int ElapsedYearsTo { get; set; }
        public byte IsElectricCar { get; set; }
        public int DisplacementFrom { get; set; }
        public int DisplacementTo { get; set; }
        public int AnnualPrice { get; set; }
        public int MonthlyPrice { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; } = null!;
    }
}
