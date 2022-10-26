using AutoMapper;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class HasInsuranceConverter : IValueConverter<int?, string>
    {
        public string Convert(int? source, ResolutionContext context)
        {
            if (source.HasValue && (source.Value <= 0 || source.Value == 99))
                return "なし";
            return "あり";
        }
    }
}
