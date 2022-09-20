﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Request
{
    public class RequestInp
    {
        public string? EstNo { get; set; }
        public string? EstSubNo { get; set; }
        public string? UserNo { get; set; }

        public decimal TaxRatio { get; set; }
    }
}
