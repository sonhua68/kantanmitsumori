using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.IDEEnitities
{
    public partial class MtIdeMember
    {
        public string AsmemberNum { get; set; } = null!;
        public int Ssnumber { get; set; }
        public string Ssname { get; set; } = null!;
        public string Ssmail { get; set; } = null!;
        public string StoreName { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; } = null!;
    }
}
