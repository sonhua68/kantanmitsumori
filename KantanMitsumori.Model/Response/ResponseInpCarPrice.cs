using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Response
{
    
    public class ResponseInpCarPrice
    {
        public string EstNo { get; set; } = "";
        public string EstSubNo { get; set; } = "";
        public string UserNo { get; set; } = "";
        public string CarPrice { get; set; } = "";
        public string RakuSatu { get; set; } = "";
        public string Rikusou { get; set; } = "";
        public string SonotaTitle { get; set; } = "その他費用";
        public string SonotaSumTitle => $"{SonotaTitle}計" ;
        public string Sonota { get; set; } = "";
        public string CarSum { get; set; } = "";        
        public string SyakenSeibi { get; set; } = "";        
        public bool IsSyakenZok { get; set; } = true;
        public bool IsSyakenNew => !IsSyakenZok;
        public string UserSyakenZok { get; set; } = "";
        public string UserSyakenNew { get; set; } = "";
        public string TaxIncluded { get; set; } = "（税込）";
        public string Zei1 => $"車両販売価格{TaxIncluded}";
        public string Zei2 => $"整備費用{TaxIncluded}";
        public string Zei3 => $"{TaxIncluded}";

    }
}
