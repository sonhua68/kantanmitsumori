using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.Service.Mapper.MapperConverter
{
    public class SyakenSeibiResolver : IValueResolver<TEstimate, ResponseInpCarPrice, string>
    {
        public string Resolve(TEstimate source, ResponseInpCarPrice destination, string destMember, ResolutionContext context)
        {
            try
            {
                if (source.SyakenZok.HasValue && source.SyakenZok > 0)
                    return source.SyakenZok.Value.ToString();
                if (source.SyakenNew.HasValue && source.SyakenNew > 0)
                    return source.SyakenNew.Value.ToString();
                return "";
            }
            catch
            {
                return "";
            }
        }
    }
}
