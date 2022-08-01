using KantanMitsumori.Model;

namespace KantanMitsumori.IService
{
    public interface IAppService
    {
        ResponseBase<List<MakerModel>> GetMaker();
        Task<ResponseBase<int>> CreateMaker(MakerModel model);
    }
}