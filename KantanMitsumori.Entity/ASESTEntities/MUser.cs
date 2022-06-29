using System;
using System.Collections.Generic;

namespace KantanMitsumori.DataAccess
{
    public partial class MUser
    {
        public string UserNo { get; set; } = null!;
        public string? UserNm { get; set; }
        public string? UserAdr { get; set; }
        public string? UserTel { get; set; }
        public DateTime? Rdate { get; set; }
        public DateTime? Udate { get; set; }
        public bool? Dflag { get; set; }
    }
}
