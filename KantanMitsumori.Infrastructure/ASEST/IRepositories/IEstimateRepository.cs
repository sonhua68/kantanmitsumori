using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Model;

namespace KantanMitsumori.Infrastructure.IRepositories
{
    public interface IEstimateRepository : IGenericRepository<TEstimate>
    {
        bool UpdateEstCalSum(EstimateModel entity);
    }
}
