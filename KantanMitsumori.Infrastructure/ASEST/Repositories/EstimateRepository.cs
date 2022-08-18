using KantanMitsumori.DataAccess;
using KantanMitsumori.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.Infrastructure.IRepositories;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class EstimateRepository : GenericRepository<TEstimate>, IEstimateRepository
    {
        public EstimateRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(TEstimate entity)
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
                _logger.LogError(ex, "t_Estimate insert error", typeof(EstimateRepository));
                return false;
            }
        }

        public override bool Update(TEstimate entity)
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
                _logger.LogError(ex, "t_Estimate update error", typeof(EstimateRepository));
                return false;
            }
        }

        public override bool Delete(TEstimate entity)
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
                _logger.LogError(ex, "t_Estimate delete error", typeof(EstimateRepository));
                return false;
            }
        }

        private TEstimate? isExists(TEstimate entity)
        {
            return dbSet.FirstOrDefault(x => x.EstNo.Equals(entity.EstNo) && x.EstSubNo.Equals(entity.EstSubNo));
        }
    }
}
