using KantanMitsumori.Model;

namespace KantanMitsumori.IService
{
    public interface IAppService
    {
        ResponseBase<List<MakerModel>> GetMaker();
    }
}