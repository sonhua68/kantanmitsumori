using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.ASESTEntities
{
    public partial class DmtMaker
    {
        public string MakerCode { get; set; } = null!;
        public string MakerName { get; set; } = null!;
        public string InsertDate { get; set; } = null!;
        public string UpdateDate { get; set; } = null!;
    }
}
