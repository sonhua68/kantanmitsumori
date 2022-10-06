using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService
{
    public interface IInpCarPriceService
    {
        ResponseBase<ResponseInpCarPrice> GetCarPriceInfo(RequestInpCarPrice request);
        ResponseBase<bool?> Update(RequestUpdateCarPrice request);
        
    }
}
