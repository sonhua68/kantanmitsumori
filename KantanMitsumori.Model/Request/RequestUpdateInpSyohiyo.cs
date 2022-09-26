namespace KantanMitsumori.Model.Request
{
    public class RequestUpdateInpSyohiyo
    {
        public string EstNo { get; set; }
        public string EstSubNo { get; set; }
        public int TaxCheck { get; set; }
        public int TaxGarage { get; set; }
        public int TaxTradeIn { get; set; }
        public int TaxTradeInSatei { get; set; }
        public int TaxRecycle { get; set; }
        public int TaxDelivery { get; set; }
        public int TaxSet1 { get; set; }
        public int TaxSet2 { get; set; }
        public int TaxSet3 { get; set; }
        public int TaxOther { get; set; }
        public int TaxCostAll { get; set; }
        public string TaxSet1Title { get; set; } = "";
        public string TaxSet2Title { get; set; } = "";
        public string TaxSet3Title { get; set; } = "";
        public int TaxFreeCheck { get; set; }
        public int TaxFreeGarage { get; set; }
        public int TaxFreeTradeIn { get; set; }
        public int TaxFreeRecycle { get; set; }
        public int TaxFreeOther { get; set; }
        public int TaxFreeAll { get; set; }
        public string TaxFreeSet1Title { get; set; } = "";
        public string TaxFreeSet2Title { get; set; } = "";
        public int TaxFreeSet1 { get; set; }
        public int TaxFreeSet2 { get; set; }
    }
}
