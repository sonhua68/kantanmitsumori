using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeCartypeRepository : GenericRepositoryIDE<MtIdeCartype>, IMtIdeCartypeRepository
    {
        public MtIdeCartypeRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeCartype entity)
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
                _logger.LogError(ex, "MtIdeCartype insert error", typeof(MtIdeCartypeRepository));
                return false;
            }
        }

        public override bool Update(MtIdeCartype entity)
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
                _logger.LogError(ex, "MtIdeCartype update error", typeof(MtIdeCartypeRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeCartype entity)
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
                _logger.LogError(ex, "MtIdeCartype delete error", typeof(MtIdeCartypeRepository));
                return false;
            }
        }

        private MtIdeCartype? isExists(MtIdeCartype entity)
        {
            return dbSet.FirstOrDefault(x => x.CarType == entity.CarType);
        }
    }
}
