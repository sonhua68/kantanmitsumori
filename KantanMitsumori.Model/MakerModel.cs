using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model
{
    public class MakerModel
    {
        public int MakerId { get; set; }
        public string? MakerName { get; set; }
        public byte? Domestic { get; set; }
        public byte? DispNo { get; set; }
        public DateTime? Rdate { get; set; }
        public DateTime? Udate { get; set; }
        public bool? Dflag { get; set; }
    }
}
