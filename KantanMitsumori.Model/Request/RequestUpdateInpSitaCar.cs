namespace KantanMitsumori.Model.Request
{
    public class RequestUpdateInpSitaCar
    {
        public string EstNo { get; set; } = "";
        public string EstSubNo { get; set; } = "";
        public int SitaPri { get; set; }
        public int SitaZan { get; set; }
        public string? SitaCarName { get; set; }
        public string? SitaCarNO { get; set; }
        public int SitaNowRun { get; set; }
        public string? SitaMilUnit { get; set; }
        public string? SitaColor { get; set; }
        public int TaxFreeTradeIn { get; set; }
        public int TaxTradeIn { get; set; }
        public int TaxTradeInSatei { get; set; }
        public string? ddlTorokuNo1 { get; set; }
        public string? txtToroku1 { get; set; }
        public string? ddlTorokuNo2 { get; set; }
        public string? txtToroku2 { get; set; }
        public string? ddlSitaFirstY { get; set; }
        public string? ddlSitaFirstM { get; set; }
        public string? ddlSitaSyakenY { get; set; }
        public string? ddlSitaSyakenM { get; set; }
        public int chkSyakenUM { get; set; }
        public int SSita { get; set; }
        public string? milUnit { get; set; }
    }
}
