using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.IRepositories;
using KantanMitsumori.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;


namespace KantanMitsumori.Infrastructure.Base
{
    public class UnitOfWorkIDE : IUnitOfWorkIDE
    {
        private readonly ILogger _logger;
        private readonly IDEContext _context;
        private bool _disposed = false;

      public IMtIdeCarTaxRepository CarTaxs { get; private set; }
   
      public IMtIdeCartypeRepository CarTypes { get; private set; }
   
      public IMtIdeCommissionRepository Commissions { get; private set; }
   
      public IMtIdeConsumptionTaxRepository ConsumptionTaxs { get; private set; }
    
      public IMtIdeContractPlanRepository ContractPlans { get; private set; }
     
      public IMtIdeFeeAdjustmentRepository FeeAdjustments { get; private set; }
 
      public IMtIdeGuaranteeRepository Guarantees { get; private set; }
   
      public IMtIdeInspectionRepository Inspections { get; private set; }
    
      public IMtIdeInterestRepository Interests { get; private set; }

      public IMtIdeLeaseFeeLowerLimitRepository LeaseFeeLowerLimits { get; private set; }
  
      public IMtIdeLeaseTargetRepository LeaseTargets { get; private set; }

      public IMtIdeLiabilityInsuranceRepository LiabilityInsurances { get; private set; }

      public IMtIdeMaintenanceRepository Maintenances { get; private set; }
   
      public IMtIdeNameChangeRepository NameChanges { get; private set; }

      public IMtIdePromotionRepository Promotions { get; private set; }

      public IMtIdeUnitPriceRepository UnitPrices { get; private set; }
 
      public IMtIdeVoluntaryInsuranceRepository VoluntaryInsurances { get; private set; }

      public IMtIdeWeightTaxRepository WeightTaxs { get; private set; }
     
          public UnitOfWorkIDE(IDEContext context, ILogger<UnitOfWorkIDE> logger)
        {
            _context = context;
            _logger = logger;
            CarTaxs = new MtIdeCarTaxRepository(context, logger);
            CarTypes = new MtIdeCartypeRepository(context, logger);
            Commissions = new MtIdeCommissionRepository(context, logger);
            ConsumptionTaxs = new MtIdeConsumptionTaxRepository(context, logger);
            ContractPlans = new MtIdeContractPlanRepository(context, logger);
            FeeAdjustments = new MtIdeFeeAdjustmentRepository(context, logger);
            Guarantees = new MtIdeGuaranteeRepository(context, logger);
            Inspections = new MtIdeInspectionRepository(context, logger);
            Interests = new MtIdeInterestRepository(context, logger);
            LeaseFeeLowerLimits = new MtIdeLeaseFeeLowerLimitRepository(context, logger);
            LeaseTargets = new MtIdeLeaseTargetRepository(context, logger);
            LiabilityInsurances = new MtIdeLiabilityInsuranceRepository(context, logger);
            Maintenances = new MtIdeMaintenanceRepository(context, logger);
            NameChanges = new MtIdeNameChangeRepository(context, logger);
            Promotions = new MtIdePromotionRepository(context, logger);
            UnitPrices = new MtIdeUnitPriceRepository(context, logger);
            VoluntaryInsurances = new MtIdeVoluntaryInsuranceRepository(context, logger);
            WeightTaxs = new MtIdeWeightTaxRepository(context, logger);

        }

        public async Task<bool> CommitAsync()
        {
            _logger.LogInformation("[UnitOfWorkIDE] begin process [SaveChanges]...");
            var result = true;
            var errorCode = 0;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    errorCode = await _context.SaveChangesAsync();
                    _logger.LogInformation("[UnitOfWorkIDE] begin commit transaction...");
                    transaction.Commit();
                    _logger.LogInformation("[UnitOfWorkIDE] commit transaction success.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[UnitOfWorkIDE] occur a exception when process [SaveChanges]. exit with code {0}", errorCode);
                    result = false;

                    _logger.LogInformation("[UnitOfWorkIDE] begin rollback transaction...");
                    transaction.Rollback();
                    _logger.LogInformation("[UnitOfWorkIDE] rollback transaction success.");
                }
            }
            _logger.LogInformation("[UnitOfWorkIDE] end process [SaveChanges] with result=[{0}]", result);
            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        Task<bool> IUnitOfWorkIDE.CommitAsync()
        {
            throw new NotImplementedException();
        }
    }



}
