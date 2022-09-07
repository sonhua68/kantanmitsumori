using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class EstimateSubRepository : GenericRepository<TEstimateSub>, IEstimateSubRepository
    {
        public EstimateSubRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(TEstimateSub entity)
        {
            try
            {
                var recordExists = isExists(entity);
                if (recordExists != null)
                {
                    return false;
                }
                else
                {
                    dbSet.Add(entity);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "t_EstimateSub insert error", typeof(EstimateSubRepository));
                return false;
            }
        }

        public override bool Update(TEstimateSub entity)
        {
            try
            {
                var recordExists = isExists(entity);
                if (recordExists == null) return false;
                // Remove old value
                _context.Entry(recordExists).State = EntityState.Detached;
                // Update new value
                _context.Entry(entity).State = EntityState.Modified;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "t_EstimateSub update error", typeof(EstimateSubRepository));
                return false;
            }
        }

        public override bool Delete(TEstimateSub entity)
        {
            try
            {
                var recordExists = isExists(entity);
                if (recordExists == null) return true;
                _context.Entry(entity).State = EntityState.Deleted;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "t_EstimateSub delete error", typeof(EstimateSubRepository));
                return false;
            }
        }

        private TEstimateSub? isExists(TEstimateSub entity)
        {
            return dbSet.FirstOrDefault(x => x.EstNo.Equals(entity.EstNo) && x.EstSubNo.Equals(entity.EstSubNo));
        }
    }
}
