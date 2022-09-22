using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Helper.CommonFuncs
{
    public class CommonSettings
    {
        private readonly IConfiguration Configuration;

        public CommonSettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string def_DmyImg => Configuration["CommonSettings:def_DmyImg"];
    }
}
