using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeContractPlanRepository : GenericRepositoryIDE<MtIdeContractPlan>, IMtIdeContractPlanRepository
    {
        public MtIdeContractPlanRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeContractPlan entity)
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
                _logger.LogError(ex, "MtIdeContractPlan insert error", typeof(MtIdeContractPlanRepository));
                return false;
            }
        }

        public override bool Update(MtIdeContractPlan entity)
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
                _logger.LogError(ex, "MtIdeContractPlan update error", typeof(MtIdeContractPlanRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeContractPlan entity)
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
                _logger.LogError(ex, "MtIdeContractPlan delete error", typeof(MtIdeContractPlanRepository));
                return false;
            }
        }

        private MtIdeContractPlan? isExists(MtIdeContractPlan entity)
        {
            return dbSet.FirstOrDefault(x => x.Id == entity.Id);
        }
    }
}
