using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Helper.Settings
{
    public class DataSettings
    {
        IConfiguration Configuration;
        public DataSettings(IConfiguration configuration) => Configuration = configuration;
        public string def_MakerName => Configuration["DataSettings:def_MakerName"];
    }
}
