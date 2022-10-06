using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Helper.Settings
{
    public class URLSettings
    {
        IConfiguration Configuration;
        public URLSettings(IConfiguration configuration) => Configuration = configuration;
        public string AutoFlagLogoUrl => Configuration["URLSettings:AutoFlagLogoUrl"];
        public string PointReQuestPreExamination => Configuration["URLSettings:PointReQuestPreExamination"];
    }
}
