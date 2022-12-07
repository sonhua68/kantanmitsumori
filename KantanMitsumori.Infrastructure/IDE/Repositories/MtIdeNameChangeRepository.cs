using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
using KantanMitsumori.Helper.CommonFuncs;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeNameChangeRepository : GenericRepositoryIDE<MtIdeNameChange>, IMtIdeNameChangeRepository
    {
        public MtIdeNameChangeRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeNameChange entity)
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
                _logger.LogError(ex, "MtIdeNameChange insert error", typeof(MtIdeNameChangeRepository));
                return false;
            }
        }

        public override bool Update(MtIdeNameChange entity)
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
                _logger.LogError(ex, "MtIdeNameChange update error", typeof(MtIdeNameChangeRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeNameChange entity)
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
                _logger.LogError(ex, "MtIdeNameChange delete error", typeof(MtIdeNameChangeRepository));
                return false;
            }
        }

        private MtIdeNameChange? isExists(MtIdeNameChange entity)
        {
            return dbSet.FirstOrDefault(x => x.NameChange == entity.NameChange);
        }
    }
}
