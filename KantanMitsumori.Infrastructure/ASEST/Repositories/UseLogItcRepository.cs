using KantanMitsumori.DataAccess;
using KantanMitsumori.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.Infrastructure.IRepositories;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class UseLogItcRepository : GenericRepository<TUseLogItc>, IUseLogItcRepository
    {
        public UseLogItcRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(TUseLogItc entity)
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
                _logger.LogError(ex, "t_UseLogItc insert error", typeof(UseLogItcRepository));
                return false;
            }
        }

        public override bool Update(TUseLogItc entity)
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
                _logger.LogError(ex, "t_UseLogItc update error", typeof(UseLogItcRepository));
                return false;
            }
        }

        public override bool Delete(TUseLogItc entity)
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
                _logger.LogError(ex, "t_UseLogItc delete error", typeof(UseLogItcRepository));
                return false;
            }
        }

        private TUseLogItc? isExists(TUseLogItc entity)
        {
            return dbSet.FirstOrDefault(x => x.LoginNo == entity.LoginNo);
        }
    }
}
