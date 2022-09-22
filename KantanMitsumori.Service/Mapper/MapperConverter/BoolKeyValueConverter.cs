using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    internal class BoolKeyValueConverter : IValueConverter<bool, string>
    {
        Dictionary<bool, string> _valueDictionary;
        public BoolKeyValueConverter(Dictionary<bool, string> valueDict)
        {
            if (valueDict == null)
                throw new ArgumentNullException();
            _valueDictionary = valueDict;
        }

        public string Convert(bool source, ResolutionContext context)
        {
            return _valueDictionary[source];
            
        }
    }
}
