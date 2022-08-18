using KantanMitsumori.DataAccess;
using KantanMitsumori.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.Infrastructure.IRepositories;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class ASOPCarNameRepository : GenericRepository<AsopCarname>, IASOPCarNameRepository
    {
        public ASOPCarNameRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(AsopCarname entity)
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
                _logger.LogError(ex, "ASOP_CarName insert error", typeof(ASOPCarNameRepository));
                return false;
            }
        }

        public override bool Update(AsopCarname entity)
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
                _logger.LogError(ex, "ASOP_CarName update error", typeof(ASOPCarNameRepository));
                return false;
            }
        }

        public override bool Delete(AsopCarname entity)
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
                _logger.LogError(ex, "ASOP_CarName delete error", typeof(ASOPCarNameRepository));
                return false;
            }
        }

        private AsopCarname? isExists(AsopCarname entity)
        {
            return dbSet.FirstOrDefault(x => x.CarmodelCode == entity.CarmodelCode && x.MekerCode == entity.MekerCode);
        }
    }
}
