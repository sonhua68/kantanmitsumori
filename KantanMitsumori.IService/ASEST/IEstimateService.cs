using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService
{
    public interface IEstimateService
    {
        ResponseBase<List<TEstimate>> GetList(RequestInp requestInputCar);
        ResponseBase<ResponseInp> GetDetail(RequestInp requestInputCar);
        Task<ResponseBase<int>> Create(TEstimate model);
        Task<ResponseBase<int>> UpdateInputCar(RequestUpdateInputCar model);
        Task<ResponseBase<int>> UpdateInpHanbaiten(RequestUpdateInpHanbaiten model);
        Task<ResponseBase<int>> UpdateInpOption(RequestUpdateInpOption model, LogToken logToken);
        Task<ResponseBase<int>> UpdateInpZeiHoken(RequestUpdateInpZeiHoken model, LogToken logToken);
        ResponseBase<List<ResponseEstimate>> GetMakerNameAndModelName(string userNo, string makerName);
        Task<ResponseBase<int>> DeleteEstimate(string EstNo, string EstSubNo);
        Task<ResponseBase<int>> UpdateInpNebiki(RequestUpdateInpNebiki model, LogToken logToken);

    }
}