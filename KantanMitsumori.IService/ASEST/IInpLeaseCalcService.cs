using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService
{
    public interface IInpLeaseCalcService
    {
        ResponseBase<List<ResponseCarType>> GetCarType();
        ResponseBase<List<ResponseContractPlan>> GetContractPlan();
        ResponseBase<List<ResponseVolInsurance>> GetVolInsurance();
        ResponseBase<List<ResponseFirstAfterSecondTerm>> GetFirstAfterSecondTerm(int carType);
        ResponseBase<ResponseUnitPriceRatesLimit> GetUnitPriceRatesLimit();
        ResponseBase<ResponseInpLeaseCalc> InpLeaseCal(RequestInpLeaseCalc model, LogToken logToken);
        Task<ResponseBase<int>> UpdateLeaseProgressIde(int leaseProgress, LogToken logToken);
    }
}