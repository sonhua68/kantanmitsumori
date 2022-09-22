using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class KeyValueConverter<S> : IValueConverter<S, string>
    {
        IDictionary<S, string> _valueDictionary;
        public KeyValueConverter(IDictionary<S, string> valueDict)
        {
            if(valueDict == null)
                throw new ArgumentNullException();
            _valueDictionary = valueDict;
        }

        public string Convert(S source, ResolutionContext context)
        {
            if (source == null)
                return "";
            if (!_valueDictionary.ContainsKey(source))
                return "";
            return _valueDictionary[source];
        }
    }
}
