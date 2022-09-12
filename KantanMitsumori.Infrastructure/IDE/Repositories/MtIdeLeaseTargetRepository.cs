using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeLeaseTargetRepository : GenericRepositoryIDE<MtIdeLeaseTarget>, IMtIdeLeaseTargetRepository
    {
        public MtIdeLeaseTargetRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeLeaseTarget entity)
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
                _logger.LogError(ex, "MtIdeLeaseTarget insert error", typeof(MtIdeLeaseTargetRepository));
                return false;
            }
        }

        public override bool Update(MtIdeLeaseTarget entity)
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
                _logger.LogError(ex, "MtIdeLeaseTarget update error", typeof(MtIdeLeaseTargetRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeLeaseTarget entity)
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
                _logger.LogError(ex, "MtIdeLeaseTarget delete error", typeof(MtIdeLeaseTargetRepository));
                return false;
            }
        }

        private MtIdeLeaseTarget? isExists(MtIdeLeaseTarget entity)
        {
            return dbSet.FirstOrDefault(x => x.Id == entity.Id);
        }
    }
}
