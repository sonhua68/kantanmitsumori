using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService
{
    public interface IInpLeaseCalcService
    {
        Task<ResponseBase<List<ResponseCarType>>> GetCarType();
        Task<ResponseBase<List<ResponseContractPlan>>> GetContractPlan();
        Task<ResponseBase<List<ResponseVolInsurance>>> GetVolInsurance();
        Task<ResponseBase<List<ResponseFirstAfterSecondTerm>>> GetFirstAfterSecondTerm(int carType);
        Task<ResponseBase<ResponseUnitPriceRatesLimit>> GetUnitPriceRatesLimit();
    }
}