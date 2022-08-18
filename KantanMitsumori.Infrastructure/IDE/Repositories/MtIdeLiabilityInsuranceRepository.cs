using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeLiabilityInsuranceRepository : GenericRepositoryIDE<MtIdeLiabilityInsurance>, IMtIdeLiabilityInsuranceRepository
    {
        public MtIdeLiabilityInsuranceRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeLiabilityInsurance entity)
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
                _logger.LogError(ex, "MtIdeLiabilityInsurance insert error", typeof(MtIdeLiabilityInsuranceRepository));
                return false;
            }
        }

        public override bool Update(MtIdeLiabilityInsurance entity)
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
                _logger.LogError(ex, "MtIdeLiabilityInsurance update error", typeof(MtIdeLiabilityInsuranceRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeLiabilityInsurance entity)
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
                _logger.LogError(ex, "MtIdeLiabilityInsurance delete error", typeof(MtIdeLiabilityInsuranceRepository));
                return false;
            }
        }

        private MtIdeLiabilityInsurance? isExists(MtIdeLiabilityInsurance entity)
        {
            return dbSet.FirstOrDefault(x => x.CarType == entity.CarType);
        }
    }
}
