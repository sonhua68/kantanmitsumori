﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Request
{
    public class RequestReport
    {
        public string EstNo { get; set; } = "";
        public string EstSubNo { get; set; } = "";        
        public string CustNm_forPrint { get; set; } = "";
        public string CustZip_forPrint { get; set; } = "";
        public string CustAdr_forPrint { get; set; } = "";
        public string CustTel_forPrint { get; set; } = "";

    }
}
