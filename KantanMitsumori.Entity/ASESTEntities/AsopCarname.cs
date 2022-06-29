using System;
using System.Collections.Generic;

namespace KantanMitsumori.DataAccess
{
    public partial class AsopCarname
    {
        public int MekerCode { get; set; }
        public int CarmodelCode { get; set; }
        public string CarmodelName { get; set; } = null!;
    }
}
