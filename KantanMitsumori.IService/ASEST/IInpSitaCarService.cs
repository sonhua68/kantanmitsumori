using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService.ASEST
{
    public interface IInpSitaCarService
    {
        ResponseBase<ResponseInpSitaCar> GetInfoSitaCar(string estNo, string estSubNo, string userNo);
        ResponseBase<List<string>> GetListOffice();
        Task<ResponseBase<int>> UpdateInpSitaCar(RequestUpdateInpSitaCar request);
    }
}
