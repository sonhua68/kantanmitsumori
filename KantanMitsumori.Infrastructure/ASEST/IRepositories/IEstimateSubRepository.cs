using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Model;

namespace KantanMitsumori.Infrastructure.IRepositories
{
    public interface IEstimateSubRepository : IGenericRepository<TEstimateSub>
    {
        bool UpdateEstSubCalSum(EstimateSubModel entity);
    }
}
