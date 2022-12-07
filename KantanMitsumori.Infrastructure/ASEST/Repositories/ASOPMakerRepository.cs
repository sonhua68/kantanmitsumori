using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
using KantanMitsumori.Helper.CommonFuncs;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class ASOPMakerRepository : GenericRepository<AsopMaker>, IASOPMakerRepository
    {
        public ASOPMakerRepository(ASESTContext context, ILogger logger) : base(context, logger) { }


        public override bool Add(AsopMaker entity)
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
                _logger.LogError(ex, "ASOP_Maker insert error", typeof(ASOPMakerRepository));
                return false;
            }
        }

        public override bool Update(AsopMaker entity)
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
                _logger.LogError(ex, "ASOP_Maker update error", typeof(ASOPMakerRepository));
                return false;
            }
        }

        public override bool Delete(AsopMaker entity)
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
                _logger.LogError(ex, "ASOP_Maker delete error", typeof(ASOPMakerRepository));
                return false;
            }
        }

        private AsopMaker? isExists(AsopMaker entity)
        {
            return dbSet.FirstOrDefault(x => x.MakerCode == entity.MakerCode);
        }
    }
}
