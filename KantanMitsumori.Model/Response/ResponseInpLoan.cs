using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Response
{
    public class ResponseInpLoan
    {
     
        public long SaleSumPrice { get; set; }
       
        public long CarPrice { get; set; }
      
        public long OptionPrice { get; set; }
      
        public long Discount { get; set; }
      
        public long Tax { get; set; }
     
        public long TaxCost { get; set; }
     
        public long TaxFree { get; set; }
      
        public long TradeIn { get; set; }
      
        public long Balance { get; set; }
       
        public long Deposit { get; set; }
    
        public decimal MoneyRate { get; set; }
      
        public int PayTimes { get; set; }
       
        public int FirstMonth { get; set; }
    
        public long Bonus { get; set; }
    
        public int BonusFirst { get; set; }
     
        public int BonusSecond { get; set; }
    
        public decimal ConTax { get; set; }
      
        public long CashTotal { get; set; }
    
        public long TotalCost { get; set; }
       
        public long TotalTax { get; set; }
      
        public long Total { get; set; }
     
        public long DepositTotal { get; set; }
     
        public long Principal { get; set; }
    
        public decimal AddonDisp { get; set; }
      
        public long Fee { get; set; }
       
        public long PayTotal { get; set; }
      
        public long BonusTimes { get; set; }
     
        public long BonusTotal { get; set; }
   
        public long PartitionPayTotal { get; set; }
     
        public long FirstPay { get; set; }
      
        public long PayMonth { get; set; }
      
        public long UtiPrincipal { get; set; }
     
        public long FirstPayMonth { get; set; }
      
        public long LastPayMonth { get; set; }
      
        public long FeeTotal { get; set; }
      
        public decimal hPayMonth { get; set; }
       
        public decimal hBonus { get; set; }
  
        public string? CalcInfo { get; set; }

    }
}
