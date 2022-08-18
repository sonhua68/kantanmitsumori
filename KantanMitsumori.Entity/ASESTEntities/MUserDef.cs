using System;
using System.Collections.Generic;

namespace KantanMitsumori.Entity.ASESTEntities
{
    public partial class MUserDef
    {
        public string UserNo { get; set; } = null!;
        public bool? ConTaxInputKb { get; set; }
        public double? Rate { get; set; }
        public string? ShopNm { get; set; }
        public string? ShopAdr { get; set; }
        public string? ShopTel { get; set; }
        public string? EstTanName { get; set; }
        public int? YtiRiekiH { get; set; }
        public int? YtiRiekiK { get; set; }
        public int? SyakenNewH { get; set; }
        public int? SyakenNewK { get; set; }
        public int? SyakenZokH { get; set; }
        public int? SyakenZokK { get; set; }
        public int? TaxFreeCheckH { get; set; }
        public int? TaxFreeCheckK { get; set; }
        public int? TaxFreeGarageH { get; set; }
        public int? TaxFreeGarageK { get; set; }
        public int? TaxFreeTradeInH { get; set; }
        public int? TaxFreeTradeInK { get; set; }
        public int? TaxCheckH { get; set; }
        public int? TaxCheckK { get; set; }
        public int? TaxGarageH { get; set; }
        public int? TaxGarageK { get; set; }
        public int? TaxTradeInH { get; set; }
        public int? TaxTradeInK { get; set; }
        public int? TaxTradeInChkH { get; set; }
        public int? TaxTradeInChkK { get; set; }
        public int? TaxRecycleH { get; set; }
        public int? TaxRecycleK { get; set; }
        public int? TaxDeliveryH { get; set; }
        public int? TaxDeliveryK { get; set; }
        public string? TaxSet1Title { get; set; }
        public int? TaxSet1H { get; set; }
        public int? TaxSet1K { get; set; }
        public string? TaxSet2Title { get; set; }
        public int? TaxSet2H { get; set; }
        public int? TaxSet2K { get; set; }
        public string? TaxSet3Title { get; set; }
        public int? TaxSet3H { get; set; }
        public int? TaxSet3K { get; set; }
        public string? TaxFreeSet1Title { get; set; }
        public int? TaxFreeSet1H { get; set; }
        public string? TaxFreeSet1K { get; set; }
        public string? TaxFreeSet2Title { get; set; }
        public int? TaxFreeSet2H { get; set; }
        public int? TaxFreeSet2K { get; set; }
        public string? DamageInsMonth { get; set; }
        public byte? AsArticle { get; set; }
        public string? MemberUrl { get; set; }
        public DateTime? Rdate { get; set; }
        public DateTime? Udate { get; set; }
        public bool? Dflag { get; set; }
        public string? SekininName { get; set; }
    }
}
