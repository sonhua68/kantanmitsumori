using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<MUser>, IUserRepository
    {
        public UserRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(MUser entity)
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
                _logger.LogError(ex, "m_User insert error", typeof(UserRepository));
                return false;
            }
        }

        public override bool Update(MUser entity)
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
                _logger.LogError(ex, "m_User update error", typeof(UserRepository));
                return false;
            }
        }

        public override bool Delete(MUser entity)
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
                _logger.LogError(ex, "m_User delete error", typeof(UserRepository));
                return false;
            }
        }

        private MUser? isExists(MUser entity)
        {
            return dbSet.FirstOrDefault(x => x.UserNo.Equals(entity.UserNo));
        }
    }
}
