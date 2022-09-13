using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MakerRepository : GenericRepository<MMaker>, IMakerRepository
    {
        public MakerRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(MMaker entity)
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
                _logger.LogError(ex, "m_Maker insert error", typeof(MakerRepository));
                return false;
            }
        }

        public override bool Update(MMaker entity)
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
                _logger.LogError(ex, "m_Maker update error", typeof(MakerRepository));
                return false;
            }
        }

        public override bool Delete(MMaker entity)
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
                _logger.LogError(ex, "m_Maker delete error", typeof(MakerRepository));
                return false;
            }
        }

        private MMaker? isExists(MMaker entity)
        {
            return dbSet.FirstOrDefault(x => x.MakerId == entity.MakerId);
        }
    }
}
