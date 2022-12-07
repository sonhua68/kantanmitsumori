using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Infrastructure.IRepositories;
using KantanMitsumori.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

using KantanMitsumori.DataAccess;
using KantanMitsumori.Helper.CommonFuncs;

namespace KantanMitsumori.Infrastructure.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ILogger _logger;
        private readonly ASESTContext _context;
        private bool _disposed = false;
        public ASESTContext DbContext => _context;   


        public IASOPCarNameRepository ASOPCarNames { get; private set; }
        public IASOPMakerRepository ASOPMakers { get; private set; }
        public IDMTMakerRepository DMTMakers { get; private set; }
        public IAquisitionTaxRepository AquisitionTaxs { get; private set; }
        public ICarRepository Cars { get; private set; }
        public ICarTaxRepository CarTaxs { get; private set; }
        public IMakerRepository Makers { get; private set; }
        public IModelRepository Models { get; private set; }
        public ISelfInsuranceRepository SelfInsurances { get; private set; }
        public IUserRepository Users { get; private set; }
        public IUserDefRepository UserDefs { get; private set; }
        public IWeightTaxRepository WeightTaxs { get; private set; }
        public IOfficeRepository Offices { get; private set; }
        public IEstimateRepository Estimates { get; private set; }
        public IEstimateIdeRepository EstimateIdes { get; private set; }
        public IEstimateItcRepository EstimateItcs { get; private set; }
        public IEstimateSubRepository EstimateSubs { get; private set; }
        public ITaxRatioDefRepository TaxRatioDefs { get; private set; }
        public IUseLogRepository UseLogs { get; private set; }
        public IUseLogItcRepository UseLogItcs { get; private set; }
        public IPsinfoRepository Psinfos { get; private set; }
        public IRuibetsuNRepository RuibetsuNs { get; private set; }
        public ISysRepository Syss { get; private set; }
        public ISysExhRepository SysExhs { get; private set; }
        public IAsMemberRepository AsMembers { get; private set; }

        public UnitOfWork(ASESTContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
            ASOPCarNames = new ASOPCarNameRepository(context, logger);
            ASOPMakers = new ASOPMakerRepository(context, logger);
            DMTMakers = new DMTMakerRepository(context, logger);
            AquisitionTaxs = new AquisitionTaxRepository(context, logger);
            Cars = new CarRepository(context, logger);
            CarTaxs = new CarTaxRepository(context, logger);
            Makers = new MakerRepository(context, logger);
            Models = new ModelRepository(context, logger);
            SelfInsurances = new SelfInsuranceRepository(context, logger);
            Users = new UserRepository(context, logger);
            UserDefs = new UserDefRepository(context, logger);
            WeightTaxs = new WeightTaxRepository(context, logger);
            Offices = new OfficeRepository(context, logger);
            Estimates = new EstimateRepository(context, logger);
            EstimateIdes = new EstimateIdeRepository(context, logger);
            EstimateItcs = new EstimateItcRepository(context, logger);
            EstimateSubs = new EstimateSubRepository(context, logger);
            TaxRatioDefs = new TaxRatioDefRepository(context, logger);
            UseLogs = new UseLogRepository(context, logger);
            UseLogItcs = new UseLogItcRepository(context, logger);
            Psinfos = new PsinfoRepository(context, logger);
            RuibetsuNs = new RuibetsuNRepository(context, logger);
            Syss = new SysRepository(context, logger);
            SysExhs = new SysExhRepository(context, logger);
            AsMembers = new AsMemberRepository(context, logger);

        }

        public async Task<bool> CommitAsync()
        {

            _logger.LogInformation("[UnitOfWork] begin process [SaveChanges]...");   
            var result = true;
            var errorCode = 0;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    errorCode = await _context.SaveChangesAsync();
                    _logger.LogInformation("[UnitOfWork] begin commit transaction...");
                    transaction.Commit();
                    _logger.LogInformation("[UnitOfWork] commit transaction success.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[UnitOfWork] occur a exception when process [SaveChanges]. exit with code {0}", errorCode);
                    result = false;

                    _logger.LogInformation("[UnitOfWork] begin rollback transaction...");
                    transaction.Rollback();
                    _logger.LogInformation("[UnitOfWork] rollback transaction success.");
                }
            }
            _logger.LogInformation("[UnitOfWork] end process [SaveChanges] with result=[{0}]", result);
            return result;
        }

        public bool Commit()
        {
            _logger.LogInformation("[UnitOfWork] begin process [SaveChanges]...");
            var result = true;
            var errorCode = 0;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    errorCode = _context.SaveChanges();
                    _logger.LogInformation("[UnitOfWork] begin commit transaction...");
                    transaction.Commit();
                    _logger.LogInformation("[UnitOfWork] commit transaction success.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[UnitOfWork] occur a exception when process [SaveChanges]. exit with code {0}", errorCode);
                    result = false;

                    _logger.LogInformation("[UnitOfWork] begin rollback transaction...");
                    transaction.Rollback();
                    _logger.LogInformation("[UnitOfWork] rollback transaction success.");
                }
            }
            _logger.LogInformation("[UnitOfWork] end process [SaveChanges] with result=[{0}]", result);
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
    }



}
