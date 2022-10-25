using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.IService.ASEST
{
    public interface ISerEstService
    {
        ResponseBase<LogToken> GenerateToken(RequestSerEstExternal model);
    }
}
