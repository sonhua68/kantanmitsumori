﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Response
{
    public class ResponseInputCar
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
        public byte? AccidentHis { get; set; }
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
      
        public int? OptionPrice7 { get; set; }
        public string? OptionName8 { get; set; }
        public int? OptionPrice8 { get; set; }
        public string? OptionName9 { get; set; }
        public int? OptionPrice9 { get; set; }
        public string? OptionName10 { get; set; }
        public int? OptionPrice10 { get; set; }
        public string? OptionName11 { get; set; }
        public int? OptionPrice11 { get; set; }
        public string? OptionName12 { get; set; }
        public int? OptionPrice12 { get; set; }
        public string? SekininName { get; set; }
        public string? LeaseFlag { get; set; }
        public string? FuelName { get; set; }
        public string? DriveName { get; set; }
        public int? CarDoors { get; set; }
        public string? BodyName { get; set; }
        public int? Capacity { get; set; }

        #region Sub
        public string? Aayear { get; set; }
        public string? Aahyk { get; set; }
        public int? Aaprice { get; set; }
        public int? SirPrice { get; set; }
        public int? YtiRieki { get; set; }
        public int? RakuSatu { get; set; }
        public int? Rikusou { get; set; }
        public string? Aaplace { get; set; }
        public string? Aano { get; set; }
        public string? Aatime { get; set; }
        public string? AutoTaxMonth { get; set; }
        public string? DamageInsMonth { get; set; }
        public int? TaxTradeInSatei { get; set; }
        public string? TaxSet1Title { get; set; }
        public int? TaxSet1 { get; set; }
        public string? TaxSet2Title { get; set; }
        public int? TaxSet2 { get; set; }
        public string? TaxSet3Title { get; set; }
        public int? TaxSet3 { get; set; }
        public string? TaxFreeSet1Title { get; set; }
        public int? TaxFreeSet1 { get; set; }
        public string? TaxFreeSet2Title { get; set; }
        public int? TaxFreeSet2 { get; set; }
        public string? CustMemo { get; set; }
        public string? SonotaTitle { get; set; }
        public int? Sonota { get; set; }
        public int? TradeInUm { get; set; }     
        public string? Corner { get; set; }
        public short? Aacount { get; set; }
        public byte? Mode { get; set; }
        public string? Notes { get; set; }
        public int? AutoTaxEquivalent { get; set; }
        public int? DamageInsEquivalent { get; set; }
        public int? TaxInsEquivalentAll { get; set; }
        public string? DispVolUnit { get; set; }
        public string? MilUnit { get; set; }
        public string? TradeInMilUnit { get; set; }
        public bool? LoanModifyFlag { get; set; }
        public bool? LoanRecalcSettingFlag { get; set; }
        public byte? LoanInfo { get; set; }
        #endregion sub 
    }
}
