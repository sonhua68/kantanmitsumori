namespace KantanMitsumori.Model.Response
{
    public class ResponseInpSyohiyo
    {
        public string EstNo { get; set; } = "";
        public string EstSubNo { get; set; } = "";
        public int TradeInUM { get; set; }
        public int TradeInPrice { get; set; }
        public int Balance { get; set; }
        public string TradeInCarName { get; set; } = "";
        public string TradeInChassisNo { get; set; } = "";
        public int TradeInNowOdometer { get; set; }
        public string TradeInMilUnit { get; set; } = "";
        public string TradeInBodyColor { get; set; } = "";
        public string TradeInRegNo { get; set; } = "";
        public string TradeInFirstRegYm { get; set; } = "";
        public string TradeInCheckCarYm { get; set; } = "";
        public int TaxFreeTradeIn { get; set; }
        public int TaxTradeIn { get; set; }
        public int TaxTradeInSatei { get; set; }
        public string? DispVol { get; set; }
    }
}
