using KantanMitsumori.DataAccess;
using KantanMitsumori.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.Infrastructure.IRepositories;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class SysExhRepository : GenericRepository<TbSysExh>, ISysExhRepository
    {
        public SysExhRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(TbSysExh entity)
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
                _logger.LogError(ex, "tb_SysExh insert error", typeof(SysExhRepository));
                return false;
            }
        }

        public override bool Update(TbSysExh entity)
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
                _logger.LogError(ex, "tb_SysExh update error", typeof(SysExhRepository));
                return false;
            }
        }

        public override bool Delete(TbSysExh entity)
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
                _logger.LogError(ex, "tb_SysExh delete error", typeof(SysExhRepository));
                return false;
            }
        }

        private TbSysExh? isExists(TbSysExh entity)
        {
            return dbSet.FirstOrDefault(x => x.Corner.Equals(entity.Corner));
        }
    }
}
