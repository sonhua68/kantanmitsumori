using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService.ASEST
{
    public interface IInpNotesService
    {
        ResponseBase<ResponseInpNotes> getInfoNotes(string estNo, string estSubNo);

        Task<ResponseBase<int>> UpdateInpNotes(RequestUpdateInpNotes model);
    }
}
