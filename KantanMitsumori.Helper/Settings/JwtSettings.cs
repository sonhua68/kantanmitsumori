using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Helper.Settings
{
    public class JwtSettings
    {
        IConfiguration Configuration;
        public JwtSettings(IConfiguration configuration) => Configuration = configuration;
        public string Key => Configuration["JwtSettings:Key"];
        public string Issuer => Configuration["JwtSettings:Issuer"];
        public string AccessExpires => Configuration["JwtSettings:AccessExpires"];
        public string RefreshExpires => Configuration["JwtSettings:RefreshExpires"];
        public string RefreshBytes => Configuration["JwtSettings:RefreshBytes"];        

    }
}
