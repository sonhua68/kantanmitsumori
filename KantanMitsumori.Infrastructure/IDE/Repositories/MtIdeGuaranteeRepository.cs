using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
using KantanMitsumori.Helper.CommonFuncs;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeGuaranteeRepository : GenericRepositoryIDE<MtIdeGuarantee>, IMtIdeGuaranteeRepository
    {
        public MtIdeGuaranteeRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeGuarantee entity)
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
                _logger.LogError(ex, "MtIdeGuarantee insert error", typeof(MtIdeGuaranteeRepository));
                return false;
            }
        }

        public override bool Update(MtIdeGuarantee entity)
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
                _logger.LogError(ex, "MtIdeGuarantee update error", typeof(MtIdeGuaranteeRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeGuarantee entity)
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
                _logger.LogError(ex, "MtIdeGuarantee delete error", typeof(MtIdeGuaranteeRepository));
                return false;
            }
        }

        private MtIdeGuarantee? isExists(MtIdeGuarantee entity)
        {
            return dbSet.FirstOrDefault(x => x.Years == entity.Years);
        }
    }
}
