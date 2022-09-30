using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Request
{
    public class RequestUpdateInpCarPrice
    {
        public string hidEstNo { get; set; }
        public string hidEstSubNo { get; set; } = "";
        public string txtCarPrice { get; set; } = "";
        public string txtRakuSatu { get; set; } = "";
        public string txtRikusou { get; set; } = "";
        public string txtSyakenSeibi { get; set; } = "";
        public string hidSyakenNewZok { get; set; } = "";
        public string txtSonotaTitle { get; set; } = "";
        public string txtSonota { get; set; } = "";
        public string txtCarSum { get; set; } = "";
        public string Syaken { get; set; } = "";
        public bool IsSyakenZok => Syaken == "radSyakenY";
        

    }
}
