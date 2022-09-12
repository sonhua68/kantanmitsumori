using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeInterestRepository : GenericRepositoryIDE<MtIdeInterest>, IMtIdeInterestRepository
    {
        public MtIdeInterestRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeInterest entity)
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
                _logger.LogError(ex, "MtIdeInterest insert error", typeof(MtIdeInterestRepository));
                return false;
            }
        }

        public override bool Update(MtIdeInterest entity)
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
                _logger.LogError(ex, "MtIdeInterest update error", typeof(MtIdeInterestRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeInterest entity)
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
                _logger.LogError(ex, "MtIdeInterest delete error", typeof(MtIdeInterestRepository));
                return false;
            }
        }

        private MtIdeInterest? isExists(MtIdeInterest entity)
        {
            return dbSet.FirstOrDefault(x => x.LeasePeriodFrom == entity.LeasePeriodFrom && x.LeasePeriodTo == entity.LeasePeriodTo);
        }
    }
}
