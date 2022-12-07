using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService.ASEST
{
    public interface IInpSyohiyoService
    {
        ResponseBase<ResponseInpSyohiyo> GetInfoSyohiyo(string estNo, string estSubNo);
        Task<ResponseBase<int>> UpdateInpSyohiyo(RequestUpdateInpSyohiyo request, LogSession logSession);
    }
}
