using KantanMitsumori.DataAccess;
using KantanMitsumori.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class ModelRepository : GenericRepository<MModel>, IModelRepository
    {
        public ModelRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(MModel entity)
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
                _logger.LogError(ex, "m_Model insert error", typeof(ModelRepository));
                return false;
            }
        }

        public override bool Update(MModel entity)
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
                _logger.LogError(ex, "m_Model update error", typeof(ModelRepository));
                return false;
            }
        }

        public override bool Delete(MModel entity)
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
                _logger.LogError(ex, "m_Model delete error", typeof(ModelRepository));
                return false;
            }
        }

        private MModel? isExists(MModel entity)
        {
            return dbSet.FirstOrDefault(x => x.ModelId == entity.ModelId);
        }
    }
}
