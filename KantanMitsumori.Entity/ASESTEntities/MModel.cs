using System;
using System.Collections.Generic;

namespace KantanMitsumori.DataAccess
{
    public partial class MModel
    {
        public int MakerId { get; set; }
        public int ModelId { get; set; }
        public string? ModelName { get; set; }
        public DateTime? Rdate { get; set; }
        public DateTime? Udate { get; set; }
        public bool? Dflag { get; set; }
    }
}
