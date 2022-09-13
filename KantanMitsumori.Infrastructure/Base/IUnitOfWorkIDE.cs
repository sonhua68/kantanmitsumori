
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Entity.IDEEnitities;
using KantanMitsumori.Infrastructure.IRepositories;
using KantanMitsumori.Infrastructure.Repositories;
using KantanMitsumori.DataAccess;
namespace KantanMitsumori.Infrastructure.Base
{
    public interface IUnitOfWorkIDE
    {
        IMtIdeCarTaxRepository CarTaxs { get; }
        IMtIdeCartypeRepository CarTypes { get; }
        IMtIdeCommissionRepository Commissions { get; }
        IMtIdeConsumptionTaxRepository ConsumptionTaxs { get; }
        IMtIdeContractPlanRepository ContractPlans { get; }
        IMtIdeFeeAdjustmentRepository FeeAdjustments { get; }
        IMtIdeGuaranteeRepository Guarantees { get; }
        IMtIdeInspectionRepository Inspections { get; }
        IMtIdeInterestRepository Interests { get; }
        IMtIdeLeaseFeeLowerLimitRepository LeaseFeeLowerLimits { get; }
        IMtIdeLeaseTargetRepository LeaseTargets { get; }
        IMtIdeLiabilityInsuranceRepository LiabilityInsurances { get; }
        IMtIdeMaintenanceRepository Maintenances { get; }
        IMtIdeNameChangeRepository NameChanges { get; }
        IMtIdePromotionRepository Promotions { get; }
        IMtIdeUnitPriceRepository UnitPrices { get; }
        IMtIdeVoluntaryInsuranceRepository VoluntaryInsurances { get; }
        IMtIdeWeightTaxRepository WeightTaxs { get; }
        IDEContext DbContext { get; }
        Task<bool> CommitAsync();
    }
}
