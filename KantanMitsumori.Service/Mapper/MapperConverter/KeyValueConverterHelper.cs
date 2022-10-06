using KantanMitsumori.Helper.Constant;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class KeyValueConverterHelper
    {
        public static Dictionary<int, string> AccidentHisDict => new Dictionary<int, string>
        {
            {0, "無し"},
            {1, "有り"},
            {2, ""},
        };
        public static Dictionary<bool, string> CarPriceTitleDict => new Dictionary<bool, string>
        {
            {true, $"{CommonConst.def_TitleCarPrice}{CommonConst.def_TitleInTax}" },
            {false, $"{CommonConst.def_TitleCarPrice}{CommonConst.def_TitleOutTax}" }
        };
        public static Dictionary<bool, string> NebikiTitleDict => new Dictionary<bool, string>
        {
            {true, $"{CommonConst.def_TitleDisCount}{CommonConst.def_TitleInTax}" },
            {false, $"{CommonConst.def_TitleDisCount}{CommonConst.def_TitleOutTax}" }
        };
        public static Dictionary<bool, string> OpSpecialTitleDict => new Dictionary<bool, string>
        {
            {true, $"[1]{CommonConst.def_TitleOpSpeCial}{CommonConst.def_TitleInTax}" },
            {false, $"[1]{CommonConst.def_TitleOpSpeCial}{CommonConst.def_TitleOutTax}" }
        };
        public static Dictionary<bool, string> TaxInsEquivalentTitleDict => new Dictionary<bool, string>
        {
            {true, $"[2]{CommonConst.def_TitleTaxInsEquivalent}{CommonConst.def_TitleInTax}" },
            {false, $"[2]{CommonConst.def_TitleTaxInsEquivalent}{CommonConst.def_TitleOutTax}" }
        };
        public static Dictionary<bool, string> DaikoTitleDict => new Dictionary<bool, string>
        {
            {true, $"[4]{CommonConst.def_TitleDaiko}{CommonConst.def_TitleInTax}" },
            {false, $"[4]{CommonConst.def_TitleDaiko}{CommonConst.def_TitleOutTax}" }
        };
        public static Dictionary<bool, string> TaxTitleDict => new Dictionary<bool, string>
        {
            {true, $"{CommonConst.def_TitleConTaxTotalInTax}" },
            {false, $"{CommonConst.def_TitleConTaxTotalOutTax}" }
        };
        //public static Dictionary<bool, string> CarSaleKeiTitleDict => new Dictionary<bool, string>
        //{
        //    {true, $"{CommonConst.def_TitleCarKei}" },
        //    {false, $"{CommonConst.def_TitleCarKei}" }
        //};
        public static Dictionary<bool, string> SaleSumTitleDict => new Dictionary<bool, string>
        {
            {true, $"{CommonConst.def_TitleSalesSumInTax}" },
            {false, $"{CommonConst.def_TitleSalesSumOutTax}" }
        };

        public static Dictionary<bool, string> ContaxInputKbDict => new Dictionary<bool, string>
        {
            {true, $"{CommonConst.def_TitleInTax}" },
            {false, $"{CommonConst.def_TitleOutTax}" }
        };

    }
}
