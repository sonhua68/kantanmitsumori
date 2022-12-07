using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class IsSyakenZokResolver : IValueResolver<TEstimate, ResponseInpCarPrice, bool>
    {
        public bool Resolve(TEstimate source, ResponseInpCarPrice destination, bool destMember, ResolutionContext context)
        {
            try
            {
                if (source.SyakenZok.HasValue && source.SyakenZok > 0)
                    return true;
                if (source.SyakenNew.HasValue && source.SyakenNew > 0)
                    return false;
                // Check Expiration Date
                var expiredYm = ConverterHelper.ParseDate(source.CheckCarYm);
                if (expiredYm != null)
                {
                    var timeSpan = expiredYm - DateTime.Today;
                    if (timeSpan.HasValue && timeSpan.Value.TotalDays > 0)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
