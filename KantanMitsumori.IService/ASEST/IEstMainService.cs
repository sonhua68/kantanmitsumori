using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService.ASEST
{
    public interface IEstMainService
    {
        UserModel getUserName(string userNo);
        ResponseBase<UserModel> getUserInfo(string mem);
        Task<ResponseBase<ResponseEstMainModel>> getEstMain(RequestActionModel requestAction, RequestHeaderModel request);
        ResponseBase<ResponseEstMainModel> ReloadGetEstMain(LogToken logtoken);
        Task<ResponseBase<ResponseEstMainModel>> setFreeEst(RequestSelGrdFreeEst model, LogToken logtoken);
        Task<ResponseBase<string>> AddEstimate(RequestSerEst model, LogToken logToken);
        Task<ResponseBase<string>> CalcSum(RequestSerEst model, LogToken logToken);
        ResponseBase<int> CheckGoPageLease(string firstRegYm, string makerName, int nowOdometer);
        ResponseBase<string> ExportDataCSV(LogToken logToken);
        Task<ResponseBase<int>> UpdateJiko(RequestUpdateJiko model);
    }
}