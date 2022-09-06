namespace KantanMitsumori.Model
{
    public class EstimateSubModel
    {
        public string EstNo { get; set; } = null!;
        public string EstSubNo { get; set; } = null!;
        public string EstUserNo { get; set; }
        public string Aayear { get; set; }
        public string Aahyk { get; set; }
        public int Aaprice { get; set; }
        public int SirPrice { get; set; }
        public int YtiRieki { get; set; }
        public int RakuSatu { get; set; }
        public int Rikusou { get; set; }
        public string Aaplace { get; set; }
        public string Aano { get; set; }
        public string Aatime { get; set; }
        public string AutoTaxMonth { get; set; }
        public string DamageInsMonth { get; set; }
        public int TaxTradeInSatei { get; set; }
        public string TaxSet1Title { get; set; }
        public int TaxSet1 { get; set; }
        public string TaxSet2Title { get; set; }
        public int TaxSet2 { get; set; }
        public string TaxSet3Title { get; set; }
        public int TaxSet3 { get; set; }
        public string TaxFreeSet1Title { get; set; }
        public int TaxFreeSet1 { get; set; }
        public string TaxFreeSet2Title { get; set; }
        public int TaxFreeSet2 { get; set; }
        public string CustMemo { get; set; }
        public string SonotaTitle { get; set; }
        public int Sonota { get; set; }
        public int TradeInUm { get; set; }
        public DateTime Rdate { get; set; }
        public DateTime Udate { get; set; }
        public bool Dflag { get; set; }
        public string Corner { get; set; }
        public short Aacount { get; set; }
        public byte Mode { get; set; }
        public string Notes { get; set; }
        public int AutoTaxEquivalent { get; set; }
        public int DamageInsEquivalent { get; set; }
        public int TaxInsEquivalentAll { get; set; }
        public string DispVolUnit { get; set; }
        public string MilUnit { get; set; }
        public string TradeInMilUnit { get; set; }
        public bool LoanModifyFlag { get; set; }
        public bool LoanRecalcSettingFlag { get; set; }
        public byte LoanInfo { get; set; }
    }
}
