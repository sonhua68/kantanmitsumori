using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeCarTaxRepository : GenericRepositoryIDE<MtIdeCarTax>, IMtIdeCarTaxRepository
    {
        public MtIdeCarTaxRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeCarTax entity)
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
                _logger.LogError(ex, "MtIdeCarTax insert error", typeof(MtIdeCarTaxRepository));
                return false;
            }
        }

        public override bool Update(MtIdeCarTax entity)
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
                _logger.LogError(ex, "MtIdeCarTax update error", typeof(MtIdeCarTaxRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeCarTax entity)
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
                _logger.LogError(ex, "MtIdeCarTax delete error", typeof(MtIdeCarTaxRepository));
                return false;
            }
        }

        private MtIdeCarTax? isExists(MtIdeCarTax entity)
        {
            return dbSet.FirstOrDefault(x => x.CarType == entity.CarType);
        }
    }
}
