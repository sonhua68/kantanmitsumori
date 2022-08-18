using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.ASESTEntities
{
    public partial class AsopCarname
    {
        public int MekerCode { get; set; }
        public int CarmodelCode { get; set; }
        public string CarmodelName { get; set; } = null!;
    }
}
