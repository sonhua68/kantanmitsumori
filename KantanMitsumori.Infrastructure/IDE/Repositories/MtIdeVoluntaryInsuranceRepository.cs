using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeVoluntaryInsuranceRepository : GenericRepositoryIDE<MtIdeVoluntaryInsurance>, IMtIdeVoluntaryInsuranceRepository
    {
        public MtIdeVoluntaryInsuranceRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeVoluntaryInsurance entity)
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
                _logger.LogError(ex, "MtIdeVoluntaryInsurance insert error", typeof(MtIdeVoluntaryInsuranceRepository));
                return false;
            }
        }

        public override bool Update(MtIdeVoluntaryInsurance entity)
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
                _logger.LogError(ex, "MtIdeVoluntaryInsurance update error", typeof(MtIdeVoluntaryInsuranceRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeVoluntaryInsurance entity)
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
                _logger.LogError(ex, "MtIdeVoluntaryInsurance delete error", typeof(MtIdeVoluntaryInsuranceRepository));
                return false;
            }
        }

        private MtIdeVoluntaryInsurance? isExists(MtIdeVoluntaryInsurance entity)
        {
            return dbSet.FirstOrDefault(x => x.Id == entity.Id);
        }
    }
}
