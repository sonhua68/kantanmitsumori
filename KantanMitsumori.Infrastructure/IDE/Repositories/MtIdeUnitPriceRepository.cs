using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KantanMitsumori.DataAccess;
using KantanMitsumori.Helper.CommonFuncs;
namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdePromotionRepository : GenericRepositoryIDE<MtIdePromotion>, IMtIdePromotionRepository
    {
        public MtIdePromotionRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdePromotion entity)
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
                _logger.LogError(ex, "MtIdePromotion insert error", typeof(MtIdePromotionRepository));
                return false;
            }
        }

        public override bool Update(MtIdePromotion entity)
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
                _logger.LogError(ex, "MtIdePromotion update error", typeof(MtIdePromotionRepository));
                return false;
            }
        }

        public override bool Delete(MtIdePromotion entity)
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
                _logger.LogError(ex, "MtIdePromotion delete error", typeof(MtIdePromotionRepository));
                return false;
            }
        }

        private MtIdePromotion? isExists(MtIdePromotion entity)
        {
            return dbSet.FirstOrDefault(x => x.Promotion == entity.Promotion);
        }
    }
}
