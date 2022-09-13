using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class UseLogRepository : GenericRepository<TUseLog>, IUseLogRepository
    {
        public UseLogRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(TUseLog entity)
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
                _logger.LogError(ex, "t_UseLog insert error", typeof(UseLogRepository));
                return false;
            }
        }

        public override bool Update(TUseLog entity)
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
                _logger.LogError(ex, "t_UseLog update error", typeof(UseLogRepository));
                return false;
            }
        }

        public override bool Delete(TUseLog entity)
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
                _logger.LogError(ex, "t_UseLog delete error", typeof(UseLogRepository));
                return false;
            }
        }

        private TUseLog? isExists(TUseLog entity)
        {
            return dbSet.FirstOrDefault(x => x.LoginNo == entity.LoginNo);
        }
    }
}
