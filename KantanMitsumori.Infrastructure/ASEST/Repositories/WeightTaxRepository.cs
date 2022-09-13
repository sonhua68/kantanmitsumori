using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class WeightTaxRepository : GenericRepository<MWeightTax>, IWeightTaxRepository
    {
        public WeightTaxRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(MWeightTax entity)
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
                _logger.LogError(ex, "m_WeightTax insert error", typeof(WeightTaxRepository));
                return false;
            }
        }

        public override bool Update(MWeightTax entity)
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
                _logger.LogError(ex, "m_WeightTax update error", typeof(WeightTaxRepository));
                return false;
            }
        }

        public override bool Delete(MWeightTax entity)
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
                _logger.LogError(ex, "m_WeightTax delete error", typeof(WeightTaxRepository));
                return false;
            }
        }

        private MWeightTax? isExists(MWeightTax entity)
        {
            return dbSet.FirstOrDefault(x => x.WeightTaxId  == entity.WeightTaxId);
        }
    }
}
