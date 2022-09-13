using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService
{
    public interface IAppService
    {
        ResponseBase<List<MakerModel>> GetMaker();
        Task<ResponseBase<int>> CreateMaker(MakerModel model);
        UserModel getUserName(string userNo);
        ResponseBase<UserModel> getUserInfo(string mem);
        Task<ResponseBase<EstimateModelView>> getEstMain(string sel, RequestHeaderModel request);

        Task<ResponseBase<ResponEstMainModel>> setFreeEst();
    }
}