using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class DMTMakerRepository : GenericRepository<DmtMaker>, IDMTMakerRepository
    {
        public DMTMakerRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(DmtMaker entity)
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
                _logger.LogError(ex, "DMT_Maker insert error", typeof(DMTMakerRepository));
                return false;
            }
        }

        public override bool Update(DmtMaker entity)
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
                _logger.LogError(ex, "DMT_Maker update error", typeof(DMTMakerRepository));
                return false;
            }
        }

        public override bool Delete(DmtMaker entity)
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
                _logger.LogError(ex, "DMT_Maker delete error", typeof(DMTMakerRepository));
                return false;
            }
        }

        private DmtMaker? isExists(DmtMaker entity)
        {
            return dbSet.FirstOrDefault(x => x.MakerCode.Equals(entity.MakerCode));
        }
    }
}
