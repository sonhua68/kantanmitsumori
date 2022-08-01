using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;

namespace KantanMitsumori.IService
{
    public interface IAppService
    {
        ResponseBase<List<MakerModel>> GetMaker();
        Task<ResponseBase<int>> CreateMaker(MakerModel model);
    }
}