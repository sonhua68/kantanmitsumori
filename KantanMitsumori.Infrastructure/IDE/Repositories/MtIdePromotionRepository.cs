using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
using KantanMitsumori.Helper.CommonFuncs;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeUnitPriceRepository : GenericRepositoryIDE<MtIdeUnitPrice>, IMtIdeUnitPriceRepository
    {
        public MtIdeUnitPriceRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeUnitPrice entity)
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
                _logger.LogError(ex, "MtIdeUnitPrice insert error", typeof(MtIdeUnitPriceRepository));
                return false;
            }
        }

        public override bool Update(MtIdeUnitPrice entity)
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
                _logger.LogError(ex, "MtIdeUnitPrice update error", typeof(MtIdeUnitPriceRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeUnitPrice entity)
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
                _logger.LogError(ex, "MtIdeUnitPrice delete error", typeof(MtIdeUnitPriceRepository));
                return false;
            }
        }

        private MtIdeUnitPrice? isExists(MtIdeUnitPrice entity)
        {
            return dbSet.FirstOrDefault(x => x.UnitPrice == entity.UnitPrice);
        }
    }
}
