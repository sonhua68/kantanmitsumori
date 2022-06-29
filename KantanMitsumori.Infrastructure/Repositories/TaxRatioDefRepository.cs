using KantanMitsumori.DataAccess;
using KantanMitsumori.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class TaxRatioDefRepository : GenericRepository<TTaxRatioDef>, ITaxRatioDefRepository
    {
        public TaxRatioDefRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(TTaxRatioDef entity)
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
                _logger.LogError(ex, "t_TaxRatioDef insert error", typeof(TaxRatioDefRepository));
                return false;
            }
        }

        public override bool Update(TTaxRatioDef entity)
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
                _logger.LogError(ex, "t_TaxRatioDef update error", typeof(TaxRatioDefRepository));
                return false;
            }
        }

        public override bool Delete(TTaxRatioDef entity)
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
                _logger.LogError(ex, "t_TaxRatioDef delete error", typeof(TaxRatioDefRepository));
                return false;
            }
        }

        private TTaxRatioDef? isExists(TTaxRatioDef entity)
        {
            return dbSet.FirstOrDefault(x => x.UserNo.Equals(entity.UserNo));
        }
    }
}
