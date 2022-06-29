using KantanMitsumori.DataAccess;
using KantanMitsumori.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class CarTaxRepository : GenericRepository<MCarTax>, ICarTaxRepository
    {
        public CarTaxRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(MCarTax entity)
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
                _logger.LogError(ex, "m_CarTax insert error", typeof(CarTaxRepository));
                return false;
            }
        }

        public override bool Update(MCarTax entity)
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
                _logger.LogError(ex, "m_CarTax update error", typeof(CarTaxRepository));
                return false;
            }
        }

        public override bool Delete(MCarTax entity)
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
                _logger.LogError(ex, "m_CarTax delete error", typeof(CarTaxRepository));
                return false;
            }
        }

        private MCarTax? isExists(MCarTax entity)
        {
            return dbSet.FirstOrDefault(x => x.CarTaxId == entity.CarTaxId);
        }
    }
}
