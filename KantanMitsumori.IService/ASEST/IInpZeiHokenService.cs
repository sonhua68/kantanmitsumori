using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService
{
    public interface IInpZeiHokenService
    {
        ResponseBase<string> calcCarTax(RequestInpZeiHoken requestData);
        ResponseBase<int> calcJibai(RequestInpZeiHoken requestData);

    }
}