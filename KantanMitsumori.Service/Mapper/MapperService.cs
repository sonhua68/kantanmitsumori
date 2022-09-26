using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Entity.ASESTSQL;
using KantanMitsumori.Model;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Model.Response.Report;
using KantanMitsumori.Service.Mapper.MapperConverter;
using KantanMitsumori.Entity.IDEEnitities;

namespace KantanMitsumori.Service.Mapper
{
    public class MapperService : Profile
    {
        public MapperService()
        {
            // Generic type mapping
            CreateMap<int?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            CreateMap<string?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            CreateMap<bool?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            CreateMap<DateTime?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            CreateMap<byte?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            // Source name as sesPropertyName
            RecognizePrefixes("ses");

            // Specified class mapping            
            CreateMap<MMaker, MakerModel>();
            CreateMap<MMaker, MakerModel>();

            CreateMap<TEstimate, EstModel>().ReverseMap();
            CreateMap<TEstimateSub, EstModel>().ReverseMap();
            CreateMap<MUserDef, UserDefModel>().ReverseMap();
            CreateMap<MUser, UserModel>().ReverseMap();
            CreateMap<TEstimateIde, EstimateIdeModel>().ReverseMap();

            CreateMap<AsopMaker, ResponseAsopMaker>();
            CreateMap<AsopCarname, ResponseAsopCarname>();
            CreateMap<TbRuibetsuN, ResponseTbRuibetsuN>();
            CreateMap<TbRuibetsuEntity, ResponseTbRuibetsuNew>();
            CreateMap<ResponseEstimate, TEstimate>();
            CreateMap<SerEstEntity, ResponseSerEst>();
            CreateMap<MUserDef, ResponseUserDef>();
            CreateMap<RequestUpdateInpInitVal, MUserDef>();

            CreateMapForReport();
           
        }

        private void CreateMapForReport()
        {
            // Request mapping
            CreateMap<LogToken, RequestReport>();      

            // Response mapping
            CreateMap<TEstimate, EstimateReportModel>()
                .ForMember(t => t.EstNo, o => o.MapFrom(s => $"{s.EstNo}-{s.EstSubNo}"))
                .ForMember(t => t.BusiDate, o => o.MapFrom(s => s.TradeDate.ToString("yyyy年M月d日")))
                .ForMember(t => t.CarName, o => o.MapFrom(s => $"{s.MakerName}{s.ModelName}"))
                .ForMember(t => t.CaseName, o => o.MapFrom(s => s.Case ?? ""))
                .ForMember(t => t.Color, o => o.MapFrom(s => s.BodyColor ?? ""))
                .ForMember(t => t.FirstRegYm, o => o.ConvertUsing(new JpEraYMConverter()))
                .ForMember(t => t.CheckCarYm, o => o.ConvertUsing(new JpEraYMConverter()))
                .ForMember(t => t.NowOdometer, o => o.ConvertUsing(new MiUnitConverter()))
                .ForMember(t => t.AccidentHis, o => o.ConvertUsing(new KeyValueConverter<int>(KeyValueConverterHelper.AccidentHisDict)))
                .ForMember(t => t.Vol, o => o.ConvertUsing(new VolUnitConverter(), s => s.DispVol))
                .ForMember(t => t.CarImg, o => o.ConvertUsing(new ImageConverter(), s => s.CarImgPath))
                .ForMember(t => t.CarImg1, o => o.ConvertUsing(new ImageConverter(), s => s.CarImgPath1))
                .ForMember(t => t.CarImg2, o => o.ConvertUsing(new ImageConverter(), s => s.CarImgPath2))
                .ForMember(t => t.CarImg3, o => o.ConvertUsing(new ImageConverter(), s => s.CarImgPath3))
                .ForMember(t => t.CarImg4, o => o.ConvertUsing(new ImageConverter(), s => s.CarImgPath4))
                .ForMember(t => t.CarImg5, o => o.ConvertUsing(new ImageConverter(), s => s.CarImgPath5))
                .ForMember(t => t.CarImg6, o => o.ConvertUsing(new ImageConverter(), s => s.CarImgPath6))
                .ForMember(t => t.CarImg7, o => o.ConvertUsing(new ImageConverter(), s => s.CarImgPath7))
                .ForMember(t => t.CarImg8, o => o.ConvertUsing(new ImageConverter(), s => s.CarImgPath8))
                .ForMember(t => t.AAInfo, o => o.MapFrom(new AAInfoResolver()))
                .ForMember(t => t.TradeInCarName, o => { o.PreCondition(c => c.IsTradeIn()); })
                .ForMember(t => t.TradeInFirstRegYm, o => { o.PreCondition(c => c.IsTradeIn()); o.ConvertUsing(new JpEraYMConverter()); })
                .ForMember(t => t.TradeInCheckCarYm, o => { o.PreCondition(c => c.IsTradeIn()); o.ConvertUsing(new JpEraYMConverter()); })
                .ForMember(t => t.TradeInNowOdometer, o => { o.PreCondition(c => c.IsTradeIn()); o.ConvertUsing(new TradeInMiUnitConverter()); })
                .ForMember(t => t.TradeInChassisNo, o => o.PreCondition(c => c.IsTradeIn()))
                .ForMember(t => t.TradeInRegNo, o => { o.PreCondition(c => c.IsTradeIn()); o.MapFrom(s => s.TradeInRegNo != null ? s.TradeInRegNo.Replace("/", "") : ""); })
                .ForMember(t => t.TradeInBodyColor, o => o.PreCondition(c => c.IsTradeIn()))
                .ForMember(t => t.CarPriceTitle, o => o.ConvertUsing(new BoolKeyValueConverter(KeyValueConverterHelper.CarPriceTitleDict), s => s.ConTaxInputKb ?? false))
                .ForMember(t => t.CarPrice, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.NebikiTitle, o => o.ConvertUsing(new BoolKeyValueConverter(KeyValueConverterHelper.NebikiTitleDict), s => s.ConTaxInputKb ?? false))
                .ForMember(t => t.Discount, o => o.ConvertUsing(new YenCurrencyConverter(prefix: "▲")))
                .ForMember(t => t.NouCost, opt => opt.Ignore())
                .ForMember(t => t.CarKei, o => o.ConvertUsing(new YenCurrencyConverter(), s => s.CarSum))
                .ForMember(t => t.OpSpeCialTitle, o => o.ConvertUsing(new BoolKeyValueConverter(KeyValueConverterHelper.OpSpecialTitleDict), s => s.ConTaxInputKb ?? false))
                .ForMember(t => t.OptionPriceAll, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.TaxInsAll, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.TaxInsEquivalentTitle, o => o.ConvertUsing(new BoolKeyValueConverter(KeyValueConverterHelper.TaxInsEquivalentTitleDict), s => s.ConTaxInputKb ?? false))
                .ForMember(t => t.TaxFreeAll, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.DaikoTitle, o => o.ConvertUsing(new BoolKeyValueConverter(KeyValueConverterHelper.DaikoTitleDict), s => s.ConTaxInputKb ?? false))
                .ForMember(t => t.DaikoAll, o => o.ConvertUsing(new YenCurrencyConverter(), s => s.TaxCostAll))
                .ForMember(t => t.TaxTitle, o => o.ConvertUsing(new BoolKeyValueConverter(KeyValueConverterHelper.TaxTitleDict), s => s.ConTaxInputKb ?? false))
                .ForMember(t => t.ConTax, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.CarSaleKeiTitle, o => o.ConvertUsing(new BoolKeyValueConverter(KeyValueConverterHelper.CarSaleKeiTitleDict), s => s.ConTaxInputKb ?? false))
                .ForMember(t => t.CarSaleKei, o => o.ConvertUsing(new YenCurrencyConverter(), s => s.CarSaleSum))
                .ForMember(t => t.TradeInPrice, o => o.ConvertUsing(new YenCurrencyConverter(prefix: "▲")))
                .ForMember(t => t.BalanceTitle, o => o.MapFrom(s => "下取車残債"))
                .ForMember(t => t.Balance, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.SalesSumTitle, o => o.ConvertUsing(new BoolKeyValueConverter(KeyValueConverterHelper.SaleSumTitleDict), s => s.ConTaxInputKb ?? false))
                .ForMember(t => t.SaleAll, o => o.ConvertUsing(new YenCurrencyConverter(), s => s.SalesSum))
                .ForMember(t => t.Deposit, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.Principal, o => o.ConvertUsing(new YenCurrencyConverter(), s => (s.SalesSum ?? 0) - (s.Deposit ?? 0)))
                .ForMember(t => t.PartitionFee, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.PartitionAmount, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.Kikan, o => o.MapFrom(s => ConverterHelper.GetKikan(s.FirstPayMonth, s.LastPayMonth)))
                .ForMember(t => t.FirstPayAmount, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.PayAmount, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.PayTimes, o => o.ConvertUsing(new YenCurrencyConverter(unit: " 回")))
                .ForMember(t => t.PayTimes2, o => o.ConvertUsing(new YenCurrencyConverter(prefix: "（×", unit: "回）"), s => s.PayTimes == null || s.PayTimes.Value == 0 ? 0 : s.PayTimes.Value - 1))
                .ForMember(t => t.BonusMonth, o => { o.PreCondition(s => s.BonusAmount.HasValue && s.BonusAmount > 0); o.MapFrom(s => ConverterHelper.GetBonusMonth(s.BonusFirst, s.BonusSecond)); })
                .ForMember(t => t.BonusAmount, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.BonusTimes, o => { o.PreCondition(s => s.BonusAmount.HasValue && s.BonusAmount > 0); o.ConvertUsing(new YenCurrencyConverter(prefix: "（×", unit: "回）")); })
                .ForMember(t => t.Rate, o => o.MapFrom(s => ConverterHelper.GetRateText(s.Rate)))
                .ForMember(t => t.OptionName1, o => o.Condition(s => s.OptionInputKb ?? false))
                .ForMember(t => t.OptionName2, o => o.Condition(s => s.OptionInputKb ?? false))
                .ForMember(t => t.OptionName3, o => o.Condition(s => s.OptionInputKb ?? false))
                .ForMember(t => t.OptionName4, o => o.Condition(s => s.OptionInputKb ?? false))
                .ForMember(t => t.OptionName5, o => o.Condition(s => s.OptionInputKb ?? false))
                .ForMember(t => t.OptionName6, o => o.Condition(s => s.OptionInputKb ?? false))
                .ForMember(t => t.OptionName7, o => o.Condition(s => s.OptionInputKb ?? false))
                .ForMember(t => t.OptionName8, o => o.Condition(s => s.OptionInputKb ?? false))
                .ForMember(t => t.OptionName9, o => o.Condition(s => s.OptionInputKb ?? false))
                .ForMember(t => t.OptionName10, o => o.Condition(s => s.OptionInputKb ?? false))
                .ForMember(t => t.OptionName11, o => o.Condition(s => s.OptionInputKb ?? false))
                .ForMember(t => t.OptionName12, o => o.Condition(s => s.OptionInputKb ?? false))
                .ForMember(t => t.OptionPrice1, o => { o.Condition(s => s.OptionInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.OptionPrice2, o => { o.Condition(s => s.OptionInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.OptionPrice3, o => { o.Condition(s => s.OptionInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.OptionPrice4, o => { o.Condition(s => s.OptionInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.OptionPrice5, o => { o.Condition(s => s.OptionInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.OptionPrice6, o => { o.Condition(s => s.OptionInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.OptionPrice7, o => { o.Condition(s => s.OptionInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.OptionPrice8, o => { o.Condition(s => s.OptionInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.OptionPrice9, o => { o.Condition(s => s.OptionInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.OptionPrice10, o => { o.Condition(s => s.OptionInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.OptionPrice11, o => { o.Condition(s => s.OptionInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.OptionPrice12, o => { o.Condition(s => s.OptionInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.AutoTax, o => { o.Condition(s => s.TaxInsInputKb ?? false); o.MapFrom(new AutoTaxResolver()); })
                .ForMember(t => t.AutoTaxMonth, o => { o.Condition(s => s.TaxInsInputKb ?? false); o.MapFrom(new AutoTaxMonthResolver()); })
                .ForMember(t => t.AcqTax, o => { o.Condition(s => s.TaxInsInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.WeightTax, o => { o.Condition(s => s.TaxInsInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.DamageInsMonth, o => { o.Condition(s => s.TaxInsInputKb ?? false); o.MapFrom(new DamageInsMonthResolver()); })
                .ForMember(t => t.DamageIns, o => { o.Condition(s => s.TaxInsInputKb ?? false); o.MapFrom(new DamageInsResolver()); })
                .ForMember(t => t.OptionIns, o => { o.Condition(s => s.TaxInsInputKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxFreeGarage, o => { o.Condition(s => s.TaxFreeKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxFreeCheck1, o => { o.Condition(s => s.TaxFreeKb ?? false); o.ConvertUsing(new YenCurrencyConverter(), s => s.TaxFreeCheck); })
                .ForMember(t => t.TaxFreeTradeIn, o => { o.Condition(s => s.TaxFreeKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxFreeRecycle, o => { o.Condition(s => s.TaxFreeKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxFreeOther, o => { o.Condition(s => s.TaxFreeKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxGarage, o => { o.Condition(s => s.TaxCostKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxCheck, o => { o.Condition(s => s.TaxCostKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxTradeIn, o => { o.Condition(s => s.TaxCostKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxDelivery, o => { o.Condition(s => s.TaxCostKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxRecycle, o => { o.Condition(s => s.TaxCostKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxOther, o => { o.Condition(s => s.TaxCostKb ?? false); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.SName, o => { o.MapFrom(s => s.ShopNm); })
                .ForMember(t => t.Address, o => { o.MapFrom(s => s.ShopAdr); })
                .ForMember(t => t.Tanto, o => { o.MapFrom(new TantoResolver()); })
                .ForMember(t => t.SekininName, o => { o.MapFrom(new SekininNameResolver()); })
                .ForMember(t => t.Tel, o => { o.MapFrom(s => $"TEL : {s.ShopTel}"); })
                .ForMember(t => t.ConTaxInputKb, o => { o.ConvertUsing(new BoolKeyValueConverter(KeyValueConverterHelper.ContaxInputKbDict), s => s.ConTaxInputKb ?? false); });

            CreateMap<TEstimateSub, EstimateReportModel>()
                .ForMember(t => t.EstNo, opt => opt.Ignore())
                .ForMember(t => t.SonotaTitle, o => o.ConvertUsing(new SonotaTitleConverter()))
                .ForMember(t => t.Sonota, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.TaxInsEquivalentAll, o => o.ConvertUsing(new YenCurrencyConverter()))
                .ForMember(t => t.TaxFreeSet1Title, o => { o.PreCondition(s => s.IsTaxFreeKb()); })
                .ForMember(t => t.TaxFreeSet1, o => { o.PreCondition(s => s.IsTaxFreeKb()); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxFreeSet2Title, o => { o.PreCondition(s => s.IsTaxFreeKb()); })
                .ForMember(t => t.TaxFreeSet2, o => { o.PreCondition(s => s.IsTaxFreeKb()); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxTradeInSatei, o => { o.PreCondition(s => s.IsTaxCostKb()); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxSet1Title, o => { o.PreCondition(s => s.IsTaxCostKb()); })
                .ForMember(t => t.TaxSet2Title, o => { o.PreCondition(s => s.IsTaxCostKb()); })
                .ForMember(t => t.TaxSet3Title, o => { o.PreCondition(s => s.IsTaxCostKb()); })
                .ForMember(t => t.TaxSet1, o => { o.PreCondition(s => s.IsTaxCostKb()); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxSet2, o => { o.PreCondition(s => s.IsTaxCostKb()); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TaxSet3, o => { o.PreCondition(s => s.IsTaxCostKb()); o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.AutoTaxMonth, o => { o.Ignore(); })
                .ForMember(t => t.DamageInsMonth, o => { o.Ignore(); });

            CreateMap<TEstimateIde, EstimateReportModel>()
                .ForMember(t => t.MonthlyLeaseFeeName, o => { o.MapFrom(s => $"月額リース料（税込）{s.MonthlyLeaseFee:N0}円（{s.LeasePeriod}ヶ月）"); })
                .ForMember(t => t.InspectionExpirationDate, o => { o.ConvertUsing(new JpYMDConverter()); })
                .ForMember(t => t.LeaseStartMonth, o => { o.ConvertUsing(new JpYMDConverter()); })
                .ForMember(t => t.LeasePeriodName, o => { o.ConvertUsing(new YenCurrencyConverter(unit: "ヶ月"), s => s.LeasePeriod); })
                .ForMember(t => t.LeaseExpirationDate, o => { o.ConvertUsing(new JpYMDConverter()); })
                .ForMember(t => t.ExtendedGuarantee, o => { o.ConvertUsing(new ExtendedGuaranteeConverter(), s => s.IsExtendedGuarantee); })
                .ForMember(t => t.HasInsurance, o => { o.ConvertUsing(new HasInsuranceConverter(), s => s.InsuranceCompanyId); })                
                .ForMember(t => t.InsuranceFee, o => { o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.DownPayment, o => { o.ConvertUsing(new YenCurrencyConverter()); })
                .ForMember(t => t.TradeInPrice1, o => { o.ConvertUsing(new YenCurrencyConverter(), s => s.TradeInPrice); });
                
            CreateMap<MtIdeContractPlan, EstimateReportModel>()
                .ForMember(t => t.ContractPlanName, o => { o.MapFrom(s => s.PlanName); });
            CreateMap<MtIdeVoluntaryInsurance, EstimateReportModel>()                
                .ForMember(t => t.InsuranceCompanyName, o => { o.MapFrom(s => s.CompanyName); });
            CreateMap<RequestReport, EstimateReportModel>()
                .ForMember(t => t.EstNo, o => o.Ignore())
                .ForMember(t => t.CustNm_forPrint, o => { o.MapFrom(new CustNmResolver()); })
                .ForMember(t => t.CustZip_forPrint, o => { o.MapFrom(s => $"〒{s.CustZip_forPrint}"); })
                .ForMember(t => t.CustTel_forPrint, o => { o.MapFrom(s => $"（TEL : {s.CustTel_forPrint}）"); });
        }

    }
}
