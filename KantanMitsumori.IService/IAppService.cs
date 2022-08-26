using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService
{
    public interface IAppService
    {
        ResponseBase<List<MakerModel>> GetMaker();
        Task<ResponseBase<int>> CreateMaker(MakerModel model);
        ResponseBase<UserModel> getUserName(string userNo);
        Task<ResponseBase<FormEstMainModel>> getEstMain(string isInputBack, string sel, RequestHeaderModel requestHeaderModel, LogToken logToken);

        bool calcSum(string inEstNo, string inEstSubNo);
    }
}