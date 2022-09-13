using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class SelfInsuranceRepository : GenericRepository<MSelfInsurance>, ISelfInsuranceRepository
    {
        public SelfInsuranceRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(MSelfInsurance entity)
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
                _logger.LogError(ex, "m_SelfInsuranceId insert error", typeof(SelfInsuranceRepository));
                return false;
            }
        }

        public override bool Update(MSelfInsurance entity)
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
                _logger.LogError(ex, "m_SelfInsuranceId update error", typeof(SelfInsuranceRepository));
                return false;
            }
        }

        public override bool Delete(MSelfInsurance entity)
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
                _logger.LogError(ex, "m_SelfInsuranceId delete error", typeof(SelfInsuranceRepository));
                return false;
            }
        }

        private MSelfInsurance? isExists(MSelfInsurance entity)
        {
            return dbSet.FirstOrDefault(x => x.SelfInsuranceId == entity.SelfInsuranceId);
        }
    }
}
