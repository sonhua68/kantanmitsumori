using Microsoft.Extensions.Configuration;

namespace KantanMitsumori.Helper.CommonFuncs
{
    public class CommonSettings
    {

        public static IConfiguration Configuration;
        public static void Configure(IConfiguration config)
        {
            Configuration = config;
        }

        public static string def_DmyImg => Configuration["CommonSettings:def_DmyImg"];
        public static string DB_TYPE => Configuration["CommonSettings:DB_TYPE"];
        public static string DB_NAME => Configuration["CommonSettings:DB_NAME"];
        public static string DB_HOST => Configuration["CommonSettings:DB_HOST"];
        public static string DB_USER => Configuration["CommonSettings:DB_USER"];
        public static string DB_PASS => Configuration["CommonSettings:DB_PASS"];
        public static string def_LogPlace => Configuration["CommonSettings:def_LogPlace"];
        public static string def_CarImgPlace => Configuration["CommonSettings:def_CarImgPlace"];
        public static string def_ExclusionListOfAutoCalc => Configuration["CommonSettings:def_ExclusionListOfAutoCalc"];
        public static string def_BizFilePdf => Configuration["CommonSettings:def_BizFilePdf"];
        public static string def_MakerName => Configuration["CommonSettings:def_MakerName"];
        public static string IsShowLogUI => Configuration["CommonSettings:IsShowLogUI"];
        public static string AutoFlagLogoUrl => Configuration["CommonSettings:AutoFlagLogoUrl"];
        public static string PointReQuestPreExamination => Configuration["CommonSettings:PointReQuestPreExamination"];
    }
}
