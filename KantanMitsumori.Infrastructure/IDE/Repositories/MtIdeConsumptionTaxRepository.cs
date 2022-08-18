using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeConsumptionTaxRepository : GenericRepositoryIDE<MtIdeConsumptionTax>, IMtIdeConsumptionTaxRepository
    {
        public MtIdeConsumptionTaxRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeConsumptionTax entity)
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
                _logger.LogError(ex, "MtIdeConsumptionTax insert error", typeof(MtIdeConsumptionTaxRepository));
                return false;
            }
        }

        public override bool Update(MtIdeConsumptionTax entity)
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
                _logger.LogError(ex, "MtIdeConsumptionTax update error", typeof(MtIdeConsumptionTaxRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeConsumptionTax entity)
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
                _logger.LogError(ex, "MtIdeConsumptionTax delete error", typeof(MtIdeConsumptionTaxRepository));
                return false;
            }
        }

        private MtIdeConsumptionTax? isExists(MtIdeConsumptionTax entity)
        {
            return dbSet.FirstOrDefault(x => x.ConsumptionTax == entity.ConsumptionTax);
        }
    }
}
