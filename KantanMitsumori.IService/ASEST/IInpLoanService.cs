using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService
{
    public interface IInpLoanService
    {   
        ResponseBase<ResponseInpLoan> CalInpLoan(RequestCalInpLoan model);
        Task<ResponseBase<string>> UpdateInputLoan(RequestUpdateInpLoan model);
    }
}