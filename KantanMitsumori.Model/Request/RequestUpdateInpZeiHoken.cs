namespace KantanMitsumori.Model.Request
{
    public class RequestUpdateInpZeiHoken
    {
        public string? EstNo { get; set; }
        public string? EstSubNo { get; set; }
        public int? MeiCarTax { get; set; }
        public int? MeiGetTax { get; set; }

        public int? MeiWeightTax { get; set; }
        public int? MeiJibaiHoken { get; set; }
        public int? MeiNiniHoken { get; set; }
        public int? TaxInsAll { get; set; }
        public string? ddlCarTaxMonth { get; set; }
        public string? ddlJibaiHokenMonth { get; set; }
        public int? MeiCarTaxEquivalent { get; set; }
        public int? MeiJibaiHokenEquivalent { get; set; }
        public int? TaxInsEquivalentAll { get; set; }

    }        

    }
