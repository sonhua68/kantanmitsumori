using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class OfficeRepository : GenericRepository<MToffice>, IOfficeRepository
    {
        public OfficeRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(MToffice entity)
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
                _logger.LogError(ex, "m_TOffice insert error", typeof(OfficeRepository));
                return false;
            }
        }

        public override bool Update(MToffice entity)
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
                _logger.LogError(ex, "m_TOffice update error", typeof(OfficeRepository));
                return false;
            }
        }

        public override bool Delete(MToffice entity)
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
                _logger.LogError(ex, "m_TOffice delete error", typeof(OfficeRepository));
                return false;
            }
        }

        private MToffice? isExists(MToffice entity)
        {
            return dbSet.FirstOrDefault(x => x.TofficeId == entity.TofficeId && x.TofficeCode.Equals(entity.TofficeCode));
        }
    }
}
