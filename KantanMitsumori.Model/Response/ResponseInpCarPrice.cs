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
        public string txtPrice { get; set; } = "";
        public string txtRakuSatu { get; set; } = "";
        public string txtRikusou { get; set; } = "";
        public string txtSonotaTitle { get; set; } = "その他費用";
        public string txtSonotaSumTitle { get; set; } = "その他費用計";
        public string txtSonota { get; set; } = "";
        public string txtCarSum { get; set; } = "";
        public string DispVol { get; set; } = "";
        public string txtSyakenSeibi { get; set; } = "";
        public string SyakenZok { get; set; } = "";
        public string lblZei1 { get; set; } = "";
        public string lblZei2 { get; set; } = "";
        public string lblZei3 { get; set; } = "";
        public string radSyakenY { get; set; } = "";
        public string radSyakenN { get; set; } = "";

        public string hidEstNo { get; set; } = "";
        public string hidEstSubNo { get; set; } = "";
        public string hidUserNo { get; set; } = "";
        public string hidSyakenNewZok { get; set; } = "";
        public string hidDispVol { get; set; } = "";
        
        

    }
}
