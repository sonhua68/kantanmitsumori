using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;

namespace KantanMitsumori.IService
{
    public interface IEstimateIdeService
    {
        ResponseBase<List<TEstimateIde>> GetList();
        Task<ResponseBase<int>> Create(TEstimateIde model);
       
    }
}