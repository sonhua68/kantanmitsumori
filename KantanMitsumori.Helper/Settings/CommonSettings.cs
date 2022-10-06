using KantanMitsumori.Helper.Settings;
using Microsoft.Extensions.Configuration;

namespace KantanMitsumori.Helper.Settings
{
    public class CommonSettings
    {

        public readonly IConfiguration Configuration;
        public CommonSettings(IConfiguration configuration) => Configuration = configuration;        
        public TestSettings TestSettings => new TestSettings(Configuration);
        public DataSettings DataSettings => new DataSettings(Configuration);
        public PhysicalPathSettings PhysicalPathSettings => new PhysicalPathSettings(Configuration);
        public URLSettings URLSettings => new URLSettings(Configuration);
        public ConnectionStrings ConnectionStrings => new ConnectionStrings(Configuration);
        public JwtSettings JwtSettings => new JwtSettings(Configuration);
    }
}
