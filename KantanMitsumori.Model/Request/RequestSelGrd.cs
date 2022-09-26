using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Request
{
    public class RequestSelGrd
    {
        public int sesMakID { get; set; }
        public string? sesMaker { get; set; }
        public string? sesCarNM { get; set; }
        public string? CaseSet { get; set; }
        public string? KbnSet { get; set; }
        public int TypeButton { get; set; }
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public int  colSort { get; set; } = 0;

    }
}
