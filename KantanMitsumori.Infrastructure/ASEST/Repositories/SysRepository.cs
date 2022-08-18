using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class SysRepository : GenericRepository<TbSy>, ISysRepository
    {
        public SysRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(TbSy entity)
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
                _logger.LogError(ex, "tb_Sys insert error", typeof(SysRepository));
                return false;
            }
        }

        public override bool Update(TbSy entity)
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
                _logger.LogError(ex, "tb_Sys update error", typeof(SysRepository));
                return false;
            }
        }

        public override bool Delete(TbSy entity)
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
                _logger.LogError(ex, "tb_Sys delete error", typeof(SysRepository));
                return false;
            }
        }

        private TbSy? isExists(TbSy entity)
        {
            return dbSet.FirstOrDefault(x => x.Corner.Equals(entity.Corner) && x.Aacount == entity.Aacount && x.Aadate.Equals(entity.Aadate));
        }
    }
}
