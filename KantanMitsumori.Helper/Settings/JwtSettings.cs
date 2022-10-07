namespace KantanMitsumori.Helper.Settings
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string AccessExpires { get; set; }
        public string RefreshExpires { get; set; }
        public string RefreshBytes { get; set; }

    }
}
