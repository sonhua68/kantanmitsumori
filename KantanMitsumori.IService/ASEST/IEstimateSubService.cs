using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;

namespace KantanMitsumori.IService
{
    public interface IEstimateSubService
    {
        ResponseBase<List<TEstimateSub>> GetList();
        Task<ResponseBase<int>> Create(TEstimateSub model);

    }
}