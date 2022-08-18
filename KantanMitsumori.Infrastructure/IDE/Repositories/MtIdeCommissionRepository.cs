using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeCommissionRepository : GenericRepositoryIDE<MtIdeCommission>, IMtIdeCommissionRepository
    {
        public MtIdeCommissionRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeCommission entity)
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
                _logger.LogError(ex, "MtIdeCommission insert error", typeof(MtIdeCommissionRepository));
                return false;
            }
        }

        public override bool Update(MtIdeCommission entity)
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
                _logger.LogError(ex, "MtIdeCommission update error", typeof(MtIdeCommissionRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeCommission entity)
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
                _logger.LogError(ex, "MtIdeCommission delete error", typeof(MtIdeCommissionRepository));
                return false;
            }
        }

        private MtIdeCommission? isExists(MtIdeCommission entity)
        {
            return dbSet.FirstOrDefault(x => x.Id == entity.Id);
        }
    }
}
