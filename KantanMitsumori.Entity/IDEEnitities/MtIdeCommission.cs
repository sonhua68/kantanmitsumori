using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.IDEEnitities
{
    public partial class MtIdeCommission
    {
        public int Id { get; set; }
        public string Idname { get; set; } = null!;
        public int Fee { get; set; }
        public byte IsMonthly { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; } = null!;
    }
}
