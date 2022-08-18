using KantanMitsumori.Infrastructure.IRepositories;
using KantanMitsumori.Infrastructure.Repositories;

namespace KantanMitsumori.Infrastructure.Base
{
    public interface IUnitOfWork
    {
        IASOPCarNameRepository ASOPCarNames { get; }
        IASOPMakerRepository ASOPMakers { get; }
        IDMTMakerRepository DMTMakers { get; }
        IAquisitionTaxRepository AquisitionTaxs { get; }
        ICarRepository Cars { get; }
        ICarTaxRepository CarTaxs { get; }
        IMakerRepository Makers { get; }
        IModelRepository Models { get; }
        ISelfInsuranceRepository SelfInsurances { get; }
        IUserRepository Users { get; }
        IUserDefRepository UserDefs { get; }
        IWeightTaxRepository WeightTaxs { get; }
        IOfficeRepository Offices { get; }
        IEstimateRepository Estimates { get; }
        IEstimateItcRepository EstimateItcs { get; }
        IEstimateSubRepository EstimateSubs { get; }
        ITaxRatioDefRepository TaxRatioDefs { get; }
        IUseLogRepository UseLogs { get; }
        IUseLogItcRepository UseLogItcs { get; }
        IPsinfoRepository Psinfos { get; }
        IRuibetsuNRepository RuibetsuNs { get; }
        ISysRepository Syss { get; }
        ISysExhRepository SysExhs { get; }
        IAsMemberRepository AsMembers { get; }
        Task<bool> CommitAsync();
    }
}
