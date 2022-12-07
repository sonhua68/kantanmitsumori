using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
using KantanMitsumori.Helper.CommonFuncs;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeInspectionRepository : GenericRepositoryIDE<MtIdeInspection>, IMtIdeInspectionRepository
    {
        public MtIdeInspectionRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeInspection entity)
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
                _logger.LogError(ex, "MtIdeInspection insert error", typeof(MtIdeInspectionRepository));
                return false;
            }
        }

        public override bool Update(MtIdeInspection entity)
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
                _logger.LogError(ex, "MtIdeInspection update error", typeof(MtIdeInspectionRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeInspection entity)
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
                _logger.LogError(ex, "MtIdeInspection delete error", typeof(MtIdeInspectionRepository));
                return false;
            }
        }

        private MtIdeInspection? isExists(MtIdeInspection entity)
        {
            return dbSet.FirstOrDefault(x => x.CarType == entity.CarType);
        }
    }
}
