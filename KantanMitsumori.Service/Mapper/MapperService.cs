using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Entity.ASESTSQL;
using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;
using KantanMitsumori.Service.Mapper.MapperConverter;

namespace KantanMitsumori.Service.Mapper
{
    public class MapperService : Profile
    {
        public MapperService()
        {
            // Generic type mapping
            CreateMap<int?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            CreateMap<long?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            CreateMap<byte?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            CreateMap<bool?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            CreateMap<string?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            CreateMap<double?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            CreateMap<float?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            CreateMap<decimal?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            CreateMap<DateTime?, string>().ConvertUsing(s => s.ToStringOrEmpty());
            CreateMap<string, int>().ConstructUsing(s => s.FromStringOrDefault<int>());
            CreateMap<string, long>().ConstructUsing(s => s.FromStringOrDefault<long>());
            CreateMap<string, bool>().ConstructUsing(s => s.FromStringOrDefault<bool>());
            CreateMap<string, byte>().ConstructUsing(s => s.FromStringOrDefault<byte>());
            CreateMap<string, double>().ConstructUsing(s => s.FromStringOrDefault<double>());
            CreateMap<string, float>().ConstructUsing(s => s.FromStringOrDefault<float>());
            CreateMap<string, decimal>().ConstructUsing(s => s.FromStringOrDefault<decimal>());

            // Source name as sesPropertyName
            RecognizePrefixes("ses");
            RecognizePrefixes("hid");
            RecognizePrefixes("txt");

            // Specified class mapping            
            CreateMap<MMaker, MakerModel>();
            CreateMap<MMaker, MakerModel>();

            CreateMap<TEstimate, EstModel>().ReverseMap();
            CreateMap<TEstimateSub, EstModel>().ReverseMap();
            CreateMap<MUserDef, UserDefModel>().ReverseMap();
            CreateMap<MUser, UserModel>().ReverseMap();
            CreateMap<TEstimateIde, EstimateIdeModel>().ReverseMap();
            CreateMap<ResponseInpSitaCar, EstModel>().ReverseMap();
            CreateMap<ResponseInpSyohiyo, EstModel>().ReverseMap();
            CreateMap<MtIdeMember, MemberIDEModel>().ReverseMap();

            CreateMap<AsopMaker, ResponseAsopMaker>();
            CreateMap<AsopCarname, ResponseAsopCarname>();
            CreateMap<TbRuibetsuN, ResponseTbRuibetsuN>();
            CreateMap<TbRuibetsuEntity, ResponseTbRuibetsuNew>();
            CreateMap<ResponseEstimate, TEstimate>();
            CreateMap<SerEstEntity, ResponseSerEst>();
            CreateMap<MUserDef, ResponseUserDef>();
            CreateMap<RequestUpdateInpInitVal, MUserDef>();

            CreateMap<MtIdeCartype, ResponseCarType>();
            CreateMap<MtIdeContractPlan, ResponseContractPlan>();
            CreateMap<MtIdeVoluntaryInsurance, ResponseVolInsurance>();
            CreateMap<MtIdeInspection, ResponseFirstAfterSecondTerm>();

            // Mapping for InpCarPrice
            CreateMap<LogSession, RequestInpCarPrice>();
            CreateMap<RequestInpCarPrice, ResponseInpCarPrice>();
            CreateMap<TEstimate, ResponseInpCarPrice>()
                .ForMember(t => t.SyakenSeibi, o => { o.MapFrom(new SyakenSeibiResolver()); })
                .ForMember(t => t.IsSyakenZok, o => { o.MapFrom(new IsSyakenZokResolver()); })
                .ForMember(t => t.TaxIncluded, o => { o.MapFrom(s => s.ConTaxInputKb == true ? CommonConst.def_TitleInTax : CommonConst.def_TitleOutTax); })
                .ForMember(t => t.CarSum, o => o.MapFrom(s => s.CarSum.ToStringWithNoZero()))
                .ForMember(t => t.CarPrice, o => o.MapFrom(s => s.CarPrice.ToStringWithNoZero()));
            CreateMap<TEstimateSub, ResponseInpCarPrice>()
                .ForMember(t => t.EstNo, o => { o.Ignore(); })
                .ForMember(t => t.EstSubNo, o => { o.Ignore(); })
                .ForMember(t => t.UserNo, o => { o.Ignore(); })
                .ForMember(t => t.RakuSatu, o => o.MapFrom(s => s.RakuSatu.ToStringWithNoZero()))
                .ForMember(t => t.Rikusou, o => o.MapFrom(s => s.Rikusou.ToStringWithNoZero()))
                .ForMember(t => t.Sonota, o => o.MapFrom(s => s.Sonota.ToStringWithNoZero()));


            CreateMap<RequestUpdateInpCarPrice, RequestUpdateCarPrice>()
                .ForMember(t => t.SyakenZok, o => { o.PreCondition(s => s.IsSyakenZok); o.MapFrom(s => s.txtSyakenSeibi.FromStringOrDefault<int>()); })
                .ForMember(t => t.SyakenNew, o => { o.PreCondition(s => !s.IsSyakenZok); o.MapFrom(s => s.txtSyakenSeibi.FromStringOrDefault<int>()); })
                .ForMember(t => t.SonotaTitle, o => { o.MapFrom(s => string.IsNullOrWhiteSpace(s.txtSonotaTitle) ? "その他費用" : s.txtSonotaTitle); });

            CreateMap<RequestUpdateCarPrice, TEstimate>();
            CreateMap<RequestUpdateCarPrice, TEstimateSub>();
            // Request mapping
            CreateMap<LogSession, RequestReport>();
            // Mapping for EstMain
            CreateMapForEstMain();
        }

        private void CreateMapForEstMain()
        {
            CreateMap<TEstimate, EstModel>()
                .ForMember(t => t.NowOdometer, o => o.MapFrom(s => s.NowOdometer ?? 0))
                .ForMember(t => t.ConTaxInputKb, o => o.MapFrom(s => s.ConTaxInputKb ?? true))
                .ForMember(t => t.TotalCost, o => o.MapFrom(s => s.TotalCost ?? 0))
                .ForMember(t => t.CarPrice, o => o.MapFrom(s => s.CarPrice ?? 0))
                .ForMember(t => t.Discount, o => o.MapFrom(s => s.Discount ?? 0))
                .ForMember(t => t.NouCost, o => o.MapFrom(s => s.NouCost ?? 0))
                .ForMember(t => t.SyakenNew, o => o.MapFrom(s => s.SyakenNew ?? 0))
                .ForMember(t => t.SyakenZok, o => o.MapFrom(s => s.SyakenZok ?? 0))
                .ForMember(t => t.CarSum, o => o.MapFrom(s => s.CarSum ?? 0))
                .ForMember(t => t.OptionInputKb, o => o.MapFrom(s => s.OptionInputKb ?? true))
                .ForMember(t => t.OptionPrice1, o => o.MapFrom(s => s.OptionPrice1 ?? 0))
                .ForMember(t => t.OptionPrice2, o => o.MapFrom(s => s.OptionPrice2 ?? 0))
                .ForMember(t => t.OptionPrice3, o => o.MapFrom(s => s.OptionPrice3 ?? 0))
                .ForMember(t => t.OptionPrice4, o => o.MapFrom(s => s.OptionPrice4 ?? 0))
                .ForMember(t => t.OptionPrice5, o => o.MapFrom(s => s.OptionPrice5 ?? 0))
                .ForMember(t => t.OptionPrice6, o => o.MapFrom(s => s.OptionPrice6 ?? 0))
                .ForMember(t => t.OptionPriceAll, o => o.MapFrom(s => s.OptionPriceAll ?? 0))
                .ForMember(t => t.TaxInsInputKb, o => o.MapFrom(s => s.TaxInsInputKb ?? true))
                .ForMember(t => t.AutoTax, o => o.MapFrom(s => s.AutoTax ?? 0))
                .ForMember(t => t.AcqTax, o => o.MapFrom(s => s.AcqTax ?? 0))
                .ForMember(t => t.WeightTax, o => o.MapFrom(s => s.WeightTax ?? 0))
                .ForMember(t => t.DamageIns, o => o.MapFrom(s => s.DamageIns ?? 0))
                .ForMember(t => t.OptionIns, o => o.MapFrom(s => s.OptionIns ?? 0))
                .ForMember(t => t.TaxInsAll, o => o.MapFrom(s => s.TaxInsAll ?? 0))
                .ForMember(t => t.TaxFreeKb, o => o.MapFrom(s => s.TaxFreeKb ?? true))
                .ForMember(t => t.TaxFreeGarage, o => o.MapFrom(s => s.TaxFreeGarage ?? 0))
                .ForMember(t => t.TaxFreeCheck, o => o.MapFrom(s => s.TaxFreeCheck ?? 0))
                .ForMember(t => t.TaxFreeTradeIn, o => o.MapFrom(s => s.TaxFreeTradeIn ?? 0))
                .ForMember(t => t.TaxFreeRecycle, o => o.MapFrom(s => s.TaxFreeRecycle ?? 0))
                .ForMember(t => t.TaxFreeOther, o => o.MapFrom(s => s.TaxFreeOther ?? 0))
                .ForMember(t => t.TaxFreeAll, o => o.MapFrom(s => s.TaxFreeAll ?? 0))
                .ForMember(t => t.TaxCostKb, o => o.MapFrom(s => s.TaxCostKb ?? true))
                .ForMember(t => t.TaxGarage, o => o.MapFrom(s => s.TaxGarage ?? 0))
                .ForMember(t => t.TaxCheck, o => o.MapFrom(s => s.TaxCheck ?? 0))
                .ForMember(t => t.TaxTradeIn, o => o.MapFrom(s => s.TaxTradeIn ?? 0))
                .ForMember(t => t.TaxDelivery, o => o.MapFrom(s => s.TaxDelivery ?? 0))
                .ForMember(t => t.TaxRecycle, o => o.MapFrom(s => s.TaxRecycle ?? 0))
                .ForMember(t => t.TaxOther, o => o.MapFrom(s => s.TaxOther ?? 0))
                .ForMember(t => t.TaxCostAll, o => o.MapFrom(s => s.TaxCostAll ?? 0))
                .ForMember(t => t.ConTax, o => o.MapFrom(s => s.ConTax ?? 0))
                .ForMember(t => t.CarSaleSum, o => o.MapFrom(s => s.CarSaleSum ?? 0))
                .ForMember(t => t.TradeInNowOdometer, o => o.MapFrom(s => s.TradeInNowOdometer ?? 0))
                .ForMember(t => t.TradeInPrice, o => o.MapFrom(s => s.TradeInPrice ?? 0))
                .ForMember(t => t.Balance, o => o.MapFrom(s => s.Balance ?? 0))
                .ForMember(t => t.SalesSum, o => o.MapFrom(s => s.SalesSum ?? 0))
                .ForMember(t => t.Rate, o => o.MapFrom(s => s.Rate ?? 0))
                .ForMember(t => t.Deposit, o => o.MapFrom(s => s.Deposit ?? 0))
                .ForMember(t => t.Principal, o => o.MapFrom(s => s.Principal ?? 0))
                .ForMember(t => t.PartitionFee, o => o.MapFrom(s => s.PartitionFee ?? 0))
                .ForMember(t => t.PartitionAmount, o => o.MapFrom(s => s.PartitionAmount ?? 0))
                .ForMember(t => t.PayTimes, o => o.MapFrom(s => s.PayTimes ?? 0))
                .ForMember(t => t.FirstPayAmount, o => o.MapFrom(s => s.FirstPayAmount ?? 0))
                .ForMember(t => t.PayAmount, o => o.MapFrom(s => s.PayAmount ?? 0))
                .ForMember(t => t.BonusAmount, o => o.MapFrom(s => s.BonusAmount ?? 0))
                .ForMember(t => t.BonusTimes, o => o.MapFrom(s => s.BonusTimes ?? 0))
                .ForMember(t => t.Dflag, o => o.MapFrom(s => s.Dflag ?? false))
                .ForMember(t => t.OptionPrice7, o => o.MapFrom(s => s.OptionPrice7 ?? 0))
                .ForMember(t => t.OptionPrice8, o => o.MapFrom(s => s.OptionPrice8 ?? 0))
                .ForMember(t => t.OptionPrice9, o => o.MapFrom(s => s.OptionPrice9 ?? 0))
                .ForMember(t => t.OptionPrice10, o => o.MapFrom(s => s.OptionPrice10 ?? 0))
                .ForMember(t => t.OptionPrice11, o => o.MapFrom(s => s.OptionPrice11 ?? 0))
                .ForMember(t => t.OptionPrice12, o => o.MapFrom(s => s.OptionPrice12 ?? 0))
                .ForMember(t => t.CarDoors, o => o.MapFrom(s => s.CarDoors ?? 0))
                .ForMember(t => t.Capacity, o => o.MapFrom(s => s.Capacity ?? 0));

            CreateMap<TEstimateSub, EstModel>()
                .ForMember(t => t.Aaprice, o => o.MapFrom(s => s.Aaprice ?? 0))
                .ForMember(t => t.SirPrice, o => o.MapFrom(s => s.SirPrice ?? 0))
                .ForMember(t => t.YtiRieki, o => o.MapFrom(s => s.YtiRieki ?? 0))
                .ForMember(t => t.RakuSatu, o => o.MapFrom(s => s.RakuSatu ?? 0))
                .ForMember(t => t.Rikusou, o => o.MapFrom(s => s.Rikusou ?? 0))
                .ForMember(t => t.TaxTradeInSatei, o => o.MapFrom(s => s.TaxTradeInSatei ?? 0))
                .ForMember(t => t.TaxSet1, o => o.MapFrom(s => s.TaxSet1 ?? 0))
                .ForMember(t => t.TaxSet2, o => o.MapFrom(s => s.TaxSet2 ?? 0))
                .ForMember(t => t.TaxSet3, o => o.MapFrom(s => s.TaxSet3 ?? 0))
                .ForMember(t => t.TaxFreeSet1, o => o.MapFrom(s => s.TaxFreeSet1 ?? 0))
                .ForMember(t => t.TaxFreeSet2, o => o.MapFrom(s => s.TaxFreeSet2 ?? 0))
                .ForMember(t => t.Sonota, o => o.MapFrom(s => s.Sonota ?? 0))
                .ForMember(t => t.TradeInUm, o => o.MapFrom(s => s.TradeInUm ?? 0))
                .ForMember(t => t.Aacount, o => o.MapFrom(s => s.Aacount ?? 0))
                .ForMember(t => t.AutoTaxEquivalent, o => o.MapFrom(s => s.AutoTaxEquivalent ?? 0))
                .ForMember(t => t.DamageInsEquivalent, o => o.MapFrom(s => s.DamageInsEquivalent ?? 0))
                .ForMember(t => t.TaxInsEquivalentAll, o => o.MapFrom(s => s.TaxInsEquivalentAll ?? 0))
                .ForMember(t => t.LoanModifyFlag, o => o.MapFrom(s => s.LoanModifyFlag ?? false))
                .ForMember(t => t.LoanRecalcSettingFlag, o => o.MapFrom(s => s.LoanRecalcSettingFlag ?? true));
        }
    }
}
