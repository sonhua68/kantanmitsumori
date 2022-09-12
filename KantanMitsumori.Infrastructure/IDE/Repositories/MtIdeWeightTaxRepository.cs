using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeWeightTaxRepository : GenericRepositoryIDE<MtIdeWeightTax>, IMtIdeWeightTaxRepository
    {
        public MtIdeWeightTaxRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeWeightTax entity)
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
                _logger.LogError(ex, "MtIdeWeightTax insert error", typeof(MtIdeWeightTaxRepository));
                return false;
            }
        }

        public override bool Update(MtIdeWeightTax entity)
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
                _logger.LogError(ex, "MtIdeWeightTax update error", typeof(MtIdeWeightTaxRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeWeightTax entity)
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
                _logger.LogError(ex, "MtIdeWeightTax delete error", typeof(MtIdeWeightTaxRepository));
                return false;
            }
        }

        private MtIdeWeightTax? isExists(MtIdeWeightTax entity)
        {
            return dbSet.FirstOrDefault(x => x.CarType == entity.CarType && x.ElapsedYearsFrom == entity.ElapsedYearsFrom && x.ElapsedYearsTo == entity.ElapsedYearsTo);
        }
    }
}
