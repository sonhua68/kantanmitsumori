using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Helper.Settings
{
    public class PhysicalPathSettings
    {
        IConfiguration Configuration;
        public PhysicalPathSettings(IConfiguration configuration) => Configuration = configuration;
        public string def_DmyImg => Configuration["PhysicalPathSettings:def_DmyImg"];        
        public string def_ExclusionListOfAutoCalc => Configuration["PhysicalPathSettings:def_ExclusionListOfAutoCalc"];
        public string def_BizFilePdf => Configuration["PhysicalPathSettings:def_BizFilePdf"];
        public string def_CarImgPlace => Configuration["PhysicalPathSettings:def_CarImgPlace"];

    }
}
