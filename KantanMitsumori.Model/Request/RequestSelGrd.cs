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

    }

    public class RequestSelGrdFreeEst
    {       
        public string? MakerName { get; set; }
        public string? ModelName { get; set; }
        public string? GradeName { get; set; }
        public string? CarCase { get; set; }
        public string? DispVol { get; set; }
       

    }
}
