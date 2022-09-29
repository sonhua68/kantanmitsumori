using KantanMitsumori.DataAccess;
using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeMemberRepository : GenericRepositoryIDE<MtIdeMember>, IMtIdeMemberRepository
    {
        public MtIdeMemberRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeMember entity)
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
                _logger.LogError(ex, "MtIdeMember insert error", typeof(MtIdeMemberRepository));
                return false;
            }
        }

        public override bool Update(MtIdeMember entity)
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
                _logger.LogError(ex, "MtIdeMember update error", typeof(MtIdeMemberRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeMember entity)
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
                _logger.LogError(ex, "MtIdeMember delete error", typeof(MtIdeMemberRepository));
                return false;
            }
        }

        private MtIdeMember? isExists(MtIdeMember entity)
        {
            return dbSet.FirstOrDefault(x => x.AsmemberNum == entity.AsmemberNum);
        }
    }
}
