using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService
{
    public interface IInpLeaseCalcService
    {
        ResponseBase<ResponseInpLease> GetDataInpLease(LogSession logSession);
        ResponseBase<List<ResponseCarType>> GetCarType();
        ResponseBase<List<ResponseContractPlan>> GetContractPlan();
        ResponseBase<List<ResponseVolInsurance>> GetVolInsurance();
        ResponseBase<List<ResponseFirstAfterSecondTerm>> GetFirstAfterSecondTerm(int carType);
        ResponseBase<ResponseUnitPriceRatesLimit> GetUnitPriceRatesLimit();
        ResponseBase<ResponseInpLeaseCalc> InpLeaseCal(RequestInpLeaseCalc model, LogSession logSession);
        Task<ResponseBase<int>> UpdateLeaseProgressIde(int leaseProgress, LogSession logSession);
    }
}