using System;
using System.Collections.Generic;

namespace KantanMitsumori.DataAccess
{
    public partial class TUseLog
    {
        public int LoginNo { get; set; }
        public string? UserNo { get; set; }
        public string? UserNm { get; set; }
        public DateTime? LoginDateTime { get; set; }
        public string? RefName { get; set; }
        public DateTime? Rdate { get; set; }
        public DateTime? Udate { get; set; }
        public bool? Dflag { get; set; }
    }
}
