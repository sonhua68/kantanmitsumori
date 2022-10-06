using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Helper.Settings
{
    public class TestSettings
    {
        IConfiguration Configuration;
        public TestSettings(IConfiguration configuration) => Configuration = configuration;
        public string IsShowLogUI => Configuration["TestSettings:IsShowLogUI"];
    }
}
