using KantanMitsumori.Model;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.IService.ASEST
{
    public interface IPreExaminationService
    {
        ResponseBase<ResponsePreExamination> GetInfoPreExamination(string estNo, string estSubNo);
    }
}
