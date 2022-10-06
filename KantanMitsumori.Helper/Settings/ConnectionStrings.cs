using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Helper.Settings
{
    public class ConnectionStrings
    {
        IConfiguration Configuration;
        public ConnectionStrings(IConfiguration configuration) => Configuration = configuration;
        public string AsestConnection => Configuration["ConnectionStrings:IDEConnection"];
        public string IDEConnection => Configuration["ConnectionStrings:IDEConnection"];
    }
}
