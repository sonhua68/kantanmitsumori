using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class RuibetsuNRepository : GenericRepository<TbRuibetsuN>, IRuibetsuNRepository
    {
        public RuibetsuNRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(TbRuibetsuN entity)
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
                _logger.LogError(ex, "tb_RuibetsuN insert error", typeof(RuibetsuNRepository));
                return false;
            }
        }

        public override bool Update(TbRuibetsuN entity)
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
                _logger.LogError(ex, "tb_RuibetsuN update error", typeof(RuibetsuNRepository));
                return false;
            }
        }

        public override bool Delete(TbRuibetsuN entity)
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
                _logger.LogError(ex, "tb_RuibetsuN delete error", typeof(RuibetsuNRepository));
                return false;
            }
        }

        private TbRuibetsuN? isExists(TbRuibetsuN entity)
        {
            return dbSet.FirstOrDefault(x => x.Code == entity.Code);
        }
    }
}
