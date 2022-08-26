using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService
{
    public interface IEstimateService
    {
        ResponseBase<List<TEstimate>> GetList(RequestInputCar requestInputCar);
        ResponseBase<ResponseInputCar> GetDetail(RequestInputCar requestInputCar);
        Task<ResponseBase<int>> Create(TEstimate model);
        Task<ResponseBase<int>> UpdateInputCar(RequestUpdateInputCar model);
        Task<ResponseBase<int>> UpdateInpHanbaiten(RequestUpdateInpHanbaiten model);
    }
}