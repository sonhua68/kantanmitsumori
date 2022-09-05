using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Request
{
    public class RequestCalInpLoan
    {
        public int SaleSumPrice { get; set; } 
        public int Deposit { get; set; }
        public decimal MoneyRate { get; set; }
        public int PayTimes { get; set; }
        public int Bonus { get; set; }
        public int FirstMonth { get; set; }
        public int BonusFirst { get; set; }
        public int BonusSecond { get; set; }
        public decimal ConTax { get; set; }
        public bool rdBonus { get; set; }
    }
}
