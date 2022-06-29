﻿using System;
using System.Collections.Generic;

namespace KantanMitsumori.DataAccess
{
    public partial class TEstimateItc
    {
        public string EstNo { get; set; } = null!;
        public string EstSubNo { get; set; } = null!;
        public string? EstUserNo { get; set; }
        public string? CallKbn { get; set; }
        public string? EstInpKbn { get; set; }
        public DateTime? TradeDate { get; set; }
        public string? MakerName { get; set; }
        public string? ModelName { get; set; }
        public string? GradeName { get; set; }
        public string? Case { get; set; }
        public string? ChassisNo { get; set; }
        public string? FirstRegYm { get; set; }
        public string? CheckCarYm { get; set; }
        public int? NowOdometer { get; set; }
        public string? DispVol { get; set; }
        public string? Mission { get; set; }
        public bool? AccidentHis { get; set; }
        public string? BusinessHis { get; set; }
        public string? Equipment { get; set; }
        public string? BodyColor { get; set; }
        public string? CarImgPath { get; set; }
        public bool? ConTaxInputKb { get; set; }
        public int? TotalCost { get; set; }
        public int? CarPrice { get; set; }
        public int? Discount { get; set; }
        public int? NouCost { get; set; }
        public int? SyakenNew { get; set; }
        public int? SyakenZok { get; set; }
        public int? CarSum { get; set; }
        public bool? OptionInputKb { get; set; }
        public string? OptionName1 { get; set; }
        public int? OptionPrice1 { get; set; }
        public string? OptionName2 { get; set; }
        public int? OptionPrice2 { get; set; }
        public string? OptionName3 { get; set; }
        public int? OptionPrice3 { get; set; }
        public string? OptionName4 { get; set; }
        public int? OptionPrice4 { get; set; }
        public string? OptionName5 { get; set; }
        public int? OptionPrice5 { get; set; }
        public string? OptionName6 { get; set; }
        public int? OptionPrice6 { get; set; }
        public int? OptionPriceAll { get; set; }
        public bool? TaxInsInputKb { get; set; }
        public int? AutoTax { get; set; }
        public int? AcqTax { get; set; }
        public int? WeightTax { get; set; }
        public int? DamageIns { get; set; }
        public int? OptionIns { get; set; }
        public int? TaxInsAll { get; set; }
        public bool? TaxFreeKb { get; set; }
        public int? TaxFreeGarage { get; set; }
        public int? TaxFreeCheck { get; set; }
        public int? TaxFreeTradeIn { get; set; }
        public int? TaxFreeRecycle { get; set; }
        public int? TaxFreeOther { get; set; }
        public int? TaxFreeAll { get; set; }
        public bool? TaxCostKb { get; set; }
        public int? TaxGarage { get; set; }
        public int? TaxCheck { get; set; }
        public int? TaxTradeIn { get; set; }
        public int? TaxDelivery { get; set; }
        public int? TaxRecycle { get; set; }
        public int? TaxOther { get; set; }
        public int? TaxCostAll { get; set; }
        public int? ConTax { get; set; }
        public int? CarSaleSum { get; set; }
        public string? TradeInCarName { get; set; }
        public string? TradeInFirstRegYm { get; set; }
        public string? TradeInCheckCarYm { get; set; }
        public int? TradeInNowOdometer { get; set; }
        public string? TradeInRegNo { get; set; }
        public string? TradeInChassisNo { get; set; }
        public string? TradeInBodyColor { get; set; }
        public int? TradeInPrice { get; set; }
        public int? Balance { get; set; }
        public int? SalesSum { get; set; }
        public string? CustKname { get; set; }
        public double? Rate { get; set; }
        public int? Deposit { get; set; }
        public int? Principal { get; set; }
        public double? PartitionFee { get; set; }
        public int? PartitionAmount { get; set; }
        public int? PayTimes { get; set; }
        public string? FirstPayMonth { get; set; }
        public string? LastPayMonth { get; set; }
        public int? FirstPayAmount { get; set; }
        public int? PayAmount { get; set; }
        public int? BonusAmount { get; set; }
        public string? BonusFirst { get; set; }
        public string? BonusSecond { get; set; }
        public int? BonusTimes { get; set; }
        public string? ShopNm { get; set; }
        public string? ShopAdr { get; set; }
        public string? ShopTel { get; set; }
        public string? EstTanName { get; set; }
        public DateTime? Rdate { get; set; }
        public DateTime? Udate { get; set; }
        public bool? Dflag { get; set; }
    }
}
