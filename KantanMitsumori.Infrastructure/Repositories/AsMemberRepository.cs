using KantanMitsumori.DataAccess;
using KantanMitsumori.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class AsMemberRepository : GenericRepository<WAsMember>, IAsMemberRepository
    {
        public AsMemberRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(WAsMember entity)
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
                _logger.LogError(ex, "w_AsMember insert error", typeof(AsMemberRepository));
                return false;
            }
        }

        public override bool Update(WAsMember entity)
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
                _logger.LogError(ex, "w_AsMember update error", typeof(AsMemberRepository));
                return false;
            }
        }

        public override bool Delete(WAsMember entity)
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
                _logger.LogError(ex, "w_AsMember delete error", typeof(AsMemberRepository));
                return false;
            }
        }

        private WAsMember? isExists(WAsMember entity)
        {
            return dbSet.FirstOrDefault(x => x.UserNo.Equals(entity.UserNo));
        }
    }
}
