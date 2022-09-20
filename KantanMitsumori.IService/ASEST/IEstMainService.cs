using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService.ASEST
{
    public interface IEstMainService
    {
        UserModel getUserName(string userNo);
        ResponseBase<UserModel> getUserInfo(string mem);
        Task<ResponseBase<ResponseEstMainModel>> getEstMain(RequestActionModel requestAction, RequestHeaderModel request);
        Task<ResponseBase<ResponseEstMainModel>> ReloadGetEstMain(LogToken logtoken);
        Task<ResponseBase<ResponseEstMainModel>> setFreeEst();

    }
}