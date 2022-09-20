using KantanMitsumori.Model;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService.ASEST
{
    public interface IInpSitaCarService
    {
        ResponseBase<ResponseInpSitaCar> getInfoSitaCar(string estNo, string estSubNo);

        //Task<ResponseBase<int>> UpdateInpCustKana(RequestUpdateInpCustKana model);
    }
}
