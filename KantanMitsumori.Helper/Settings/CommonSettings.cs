using KantanMitsumori.Helper.Settings;
using Microsoft.Extensions.Configuration;

namespace KantanMitsumori.Helper.Settings
{
    public static class CommonSettings
    {

        public static IConfiguration? Configuration;
        public static void Configure(IConfiguration config) => Configuration = config;        
        public static TestSettings TestSettings => new TestSettings(Configuration!);
        public static DataSettings DataSettings => new DataSettings(Configuration!);
        public static PhysicalPathSettings PhysicalPathSettings => new PhysicalPathSettings(Configuration!);
        public static URLSettings URLSettings => new URLSettings(Configuration!);
        public static ConnectionStrings ConnectionStrings => new ConnectionStrings(Configuration!);
        public static JwtSettings JwtSettings => new JwtSettings(Configuration!);
    }
}
