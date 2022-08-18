using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.ASESTEntities
{
    public partial class TbSysExh
    {
        public string Corner { get; set; } = null!;
        public string ExhFrom { get; set; } = null!;
        public string ExhTo { get; set; } = null!;
        public int WeekNum { get; set; }
    }
}
