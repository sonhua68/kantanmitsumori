namespace KantanMitsumori.Model
{
    public class LogToken
    {
        public string? UserNo { get; set; }
        public string? UserNm { get; set; }
        public string? Token { get; set; }
        public string? stateLoadWindow { get; set; }
        public string? sesPriDisp { get; set; }
        public string? sesMode { get; set; }
        public string? sesCustNm_forPrint { get; set; }
        public string? sesCustZip_forPrint { get; set; }
        public string? sesCustAdr_forPrint { get; set; }
        public string? sesCustTel_forPrint { get; set; }
        public string? sesEstNo { get; set; }
        public string? sesEstSubNo { get; set; }
        public decimal sesTaxRatio { get; set; } = -1M;
        public string sesLeaseFlag { get; set; } = "0";


    }
}