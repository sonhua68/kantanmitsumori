using KantanMitsumori.Model;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService.ASEST
{
    public interface IInpCustKanaService
    {
        ResponseBase<ResponseInpCustKana> getInfoCust(string estNo, string estSubNo);
    }
}
