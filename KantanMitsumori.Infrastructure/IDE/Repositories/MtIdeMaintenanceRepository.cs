using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeMaintenanceRepository : GenericRepositoryIDE<MtIdeMaintenance>, IMtIdeMaintenanceRepository
    {
        public MtIdeMaintenanceRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeMaintenance entity)
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
                _logger.LogError(ex, "MtIdeMaintenance insert error", typeof(MtIdeMaintenanceRepository));
                return false;
            }
        }

        public override bool Update(MtIdeMaintenance entity)
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
                _logger.LogError(ex, "MtIdeMaintenance update error", typeof(MtIdeMaintenanceRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeMaintenance entity)
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
                _logger.LogError(ex, "MtIdeMaintenance delete error", typeof(MtIdeMaintenanceRepository));
                return false;
            }
        }

        private MtIdeMaintenance? isExists(MtIdeMaintenance entity)
        {
            return dbSet.FirstOrDefault(x => x.CarType == entity.CarType);
        }
    }
}
