using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class UserDefRepository : GenericRepository<MUserDef>, IUserDefRepository
    {
        public UserDefRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(MUserDef entity)
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
                _logger.LogError(ex, "m_UserDef insert error", typeof(UserDefRepository));
                return false;
            }
        }

        public override bool Update(MUserDef entity)
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
                _logger.LogError(ex, "m_UserDef update error", typeof(UserDefRepository));
                return false;
            }
        }

        public override bool Delete(MUserDef entity)
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
                _logger.LogError(ex, "m_UserDef delete error", typeof(UserDefRepository));
                return false;
            }
        }

        private MUserDef? isExists(MUserDef entity)
        {
            return dbSet.FirstOrDefault(x => x.UserNo.Equals(entity.UserNo));
        }
    }
}
