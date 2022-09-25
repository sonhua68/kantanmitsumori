using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService
{
    public interface IInpInitValService
    {   
        ResponseBase<ResponseUserDef> GetUserDefData(string userNo);
        Task<ResponseBase<int>> UpdateInpInitVal(RequestUpdateInpInitVal model, LogToken logToken);
    }
}