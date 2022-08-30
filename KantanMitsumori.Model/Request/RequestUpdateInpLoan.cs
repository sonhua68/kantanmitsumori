namespace KantanMitsumori.Model.Request
{
    public class RequestUpdateInpLoan
    {
        public string? EstNo { get; set; }
        public string? EstSubNo { get; set; }
        public double MoneyRateCl { get; set; }
        public int Deposit { get; set; }
        public int Principal { get; set; }
        public double Fee { get; set; }
        public int PayTotal { get; set; }
        public string? FirstPayMonth { get; set; }
        public string? LastPayMonth { get; set; }
        public int FirstPay { get; set; }
        public int PayMonth { get; set; }
        public int Bonus { get; set; }
        public string? BonusFirst { get; set; }
        public string? BonusSecond { get; set; }
        public int BonusTimes { get; set; }
        public int PayTimes { get; set; }
        public int LoanModifyFlag { get; set; }
        public int chkProhibitAutoCalc { get; set; }
    }
}
