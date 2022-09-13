using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeLeaseFeeLowerLimitRepository : GenericRepositoryIDE<MtIdeLeaseFeeLowerLimit>, IMtIdeLeaseFeeLowerLimitRepository
    {
        public MtIdeLeaseFeeLowerLimitRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeLeaseFeeLowerLimit entity)
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
                _logger.LogError(ex, "MtIdeLeaseFeeLowerLimit insert error", typeof(MtIdeLeaseFeeLowerLimitRepository));
                return false;
            }
        }

        public override bool Update(MtIdeLeaseFeeLowerLimit entity)
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
                _logger.LogError(ex, "MtIdeLeaseFeeLowerLimit update error", typeof(MtIdeLeaseFeeLowerLimitRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeLeaseFeeLowerLimit entity)
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
                _logger.LogError(ex, "MtIdeLeaseFeeLowerLimit delete error", typeof(MtIdeLeaseFeeLowerLimitRepository));
                return false;
            }
        }

        private MtIdeLeaseFeeLowerLimit? isExists(MtIdeLeaseFeeLowerLimit entity)
        {
            return dbSet.FirstOrDefault(x => x.LeaseFeeLowerLimit == entity.LeaseFeeLowerLimit);
        }
    }
}
