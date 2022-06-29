using KantanMitsumori.DataAccess;
using KantanMitsumori.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class PsinfoRepository : GenericRepository<TbPsinfo>, IPsinfoRepository
    {
        public PsinfoRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(TbPsinfo entity)
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
                _logger.LogError(ex, "tb_Psinfo insert error", typeof(PsinfoRepository));
                return false;
            }
        }

        public override bool Update(TbPsinfo entity)
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
                _logger.LogError(ex, "tb_Psinfo update error", typeof(PsinfoRepository));
                return false;
            }
        }

        public override bool Delete(TbPsinfo entity)
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
                _logger.LogError(ex, "tb_Psinfo delete error", typeof(PsinfoRepository));
                return false;
            }
        }

        private TbPsinfo? isExists(TbPsinfo entity)
        {
            return dbSet.FirstOrDefault(x => x.Corner == entity.Corner && x.ExhNum.Equals(entity.ExhNum));
        }
    }
}
