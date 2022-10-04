using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Request
{
    public class RequestUpdateCarPrice
    {
        public string EstNo { get; set; } = "";
        public string EstSubNo { get; set; } = "";
        public int CarPrice { get; set; } = 0; 
        public int RakuSatu { get; set; } = 0;
        public int Rikusou { get; set; } = 0;
        public int Sonota { get; set; } = 0;
        public string SonotaTitle { get; set; } = "";
        public int SyakenNew { get; set; } = 0;
        public int SyakenZok { get; set; } = 0;        
        public int CarSum { get; set; } = 0;



    }
}
