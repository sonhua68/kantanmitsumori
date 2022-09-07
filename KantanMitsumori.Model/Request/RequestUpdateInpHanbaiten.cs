using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Request
{
    public class RequestUpdateInpHanbaiten
    {
        public string? EstNo { get; set; }
        public string? EstSubNo { get; set; }
        public string? HanAdd { get; set; }
        public string? HanName { get; set; }
        public string? Sekinin { get; set; }
        public string? TantoName { get; set; }
        public string? Tel { get; set; }
    }
}
