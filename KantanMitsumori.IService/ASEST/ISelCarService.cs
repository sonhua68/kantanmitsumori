using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService
{
    public interface ISelCarService
    {
        ResponseBase<List<ResponseAsopMaker>> GetListASOPMakers();
        ResponseBase<List<ResponseAsopCarname>> GetListASOPCarName(int makId );
        ResponseBase<List<ResponseTbRuibetsuN>> chkGetListRuiBetSu(RequestSelGrd requestSel, int Flg);
        ResponseBase<List<ResponseTbRuibetsuNew>> GetListRuiBetSu(RequestSelGrd requestSel);
        ResponseBase<List<ResponseSerEst>> GetListSerEst(RequestSerEst requestSel);
    }
}