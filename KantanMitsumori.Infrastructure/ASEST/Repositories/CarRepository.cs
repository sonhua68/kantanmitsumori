using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class CarRepository : GenericRepository<MCar>, ICarRepository
    {
        public CarRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(MCar entity)
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
                _logger.LogError(ex, "m_Car insert error", typeof(CarRepository));
                return false;
            }
        }

        public override bool Update(MCar entity)
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
                _logger.LogError(ex, "m_Car update error", typeof(CarRepository));
                return false;
            }
        }

        public override bool Delete(MCar entity)
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
                _logger.LogError(ex, "m_Car delete error", typeof(CarRepository));
                return false;
            }
        }

        private MCar? isExists(MCar entity)
        {
            return dbSet.FirstOrDefault(x => x.CarId == entity.CarId);
        }
    }
}
