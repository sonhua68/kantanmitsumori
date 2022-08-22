using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;

namespace KantanMitsumori.IService
{
    public interface IEstimateService
    {
        ResponseBase<List<TEstimate>> GetList();
        Task<ResponseBase<int>> Create(TEstimate model);
        Task<ResponseBase<int>> Update(InputCarModel model);  
    }
}