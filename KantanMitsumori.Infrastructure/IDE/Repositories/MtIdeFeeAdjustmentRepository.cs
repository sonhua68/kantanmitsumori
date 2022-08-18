using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.Base;
using KantanMitsumori.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Infrastructure.Repositories
{
    public class MtIdeFeeAdjustmentRepository : GenericRepositoryIDE<MtIdeFeeAdjustment>, IMtIdeFeeAdjustmentRepository
    {
        public MtIdeFeeAdjustmentRepository(IDEContext context, ILogger logger) : base(context, logger) { }
        public override bool Add(MtIdeFeeAdjustment entity)
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
                _logger.LogError(ex, "MtIdeFeeAdjustment insert error", typeof(MtIdeFeeAdjustmentRepository));
                return false;
            }
        }

        public override bool Update(MtIdeFeeAdjustment entity)
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
                _logger.LogError(ex, "MtIdeFeeAdjustment update error", typeof(MtIdeFeeAdjustmentRepository));
                return false;
            }
        }

        public override bool Delete(MtIdeFeeAdjustment entity)
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
                _logger.LogError(ex, "MtIdeFeeAdjustment delete error", typeof(MtIdeFeeAdjustmentRepository));
                return false;
            }
        }

        private MtIdeFeeAdjustment? isExists(MtIdeFeeAdjustment entity)
        {
            return dbSet.FirstOrDefault(x => x.LowerLimit == entity.LowerLimit);
        }
    }
}
