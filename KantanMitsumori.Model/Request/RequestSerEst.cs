using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Request
{
    public class RequestSerEst
    {       
        public string? EstNo { get; set; }
        public string? EstSubNo { get; set; }
        public string? EstUserNo { get; set; }
        public string? sesMode { get; set; }

        public string? ddlFromSelectY { get; set; }
        public string? ddlFromSelectM { get; set; }
        public string? ddlFromSelectD { get; set; }
        public string? ddlToSelectY { get; set; }
        public string? ddlToSelectM { get; set; }
        public string? ddlToSelectD { get; set; }
        public string? CustKanaName { get; set; }
        public string? ddlMaker { get; set; }
        public string? ddlModel { get; set; }
        public string? ChassisNo { get; set; }    

        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;

    }
}
