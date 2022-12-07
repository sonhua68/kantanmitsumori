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
        ResponseBase<ResponseEstMainModel> ReloadGetEstMain(LogSession LogSession);
        Task<ResponseBase<ResponseEstMainModel>> setFreeEst(RequestSelGrdFreeEst model, LogSession logSession);
        Task<ResponseBase<LogSession>> AddEstimate(RequestSerEst model, LogSession logSession);    
        Task<ResponseBase<LogSession>> CalcSum(RequestSerEst model, LogSession logSession);
        ResponseBase<int> CheckGoPageLease(string firstRegYm, string makerName, int nowOdometer);
        ResponseBase<string> ExportDataCSV(LogSession logSession);
        Task<ResponseBase<int>> UpdateJiko(RequestUpdateJiko model);
        void ConnectApiReport();
    }
}