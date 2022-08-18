using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.ASESTEntities
{
    public partial class WAsMember
    {
        public string UserNo { get; set; } = null!;
        public string? UserNm { get; set; }
        public string? UserAdr { get; set; }
        public string? UserTel { get; set; }
    }
}
