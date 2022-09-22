using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class YenCurrencyConverter : IValueConverter<int?, string>
    {
        private string _prefix;
        private string _unit;
        public YenCurrencyConverter(string prefix = "", string unit = " 円")
        {
            _prefix = prefix;
            _unit = unit;
        }

        public string Convert(int? source, ResolutionContext context)
        {
            if (source == null || source.Value == 0)
                return "";
            return $"{_prefix}{source.Value.ToYenCurrency(unit: _unit)}";
        }
    }
}
