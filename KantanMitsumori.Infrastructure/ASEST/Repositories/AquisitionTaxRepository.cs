using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class AquisitionTaxRepository : GenericRepository<MAquisitionTax>, IAquisitionTaxRepository
    {
        public AquisitionTaxRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(MAquisitionTax entity)
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
                _logger.LogError(ex, "m_AquisitionTax insert error", typeof(AquisitionTaxRepository));
                return false;
            }
        }

        public override bool Update(MAquisitionTax entity)
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
                _logger.LogError(ex, "m_AquisitionTax update error", typeof(AquisitionTaxRepository));
                return false;
            }
        }

        public override bool Delete(MAquisitionTax entity)
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
                _logger.LogError(ex, "m_AquisitionTax delete error", typeof(AquisitionTaxRepository));
                return false;
            }
        }

        private MAquisitionTax? isExists(MAquisitionTax entity)
        {
            return dbSet.FirstOrDefault(x => x.AquisitionTaxId == entity.AquisitionTaxId);
        }
    }
}
