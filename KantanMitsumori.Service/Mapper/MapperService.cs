using AutoMapper;
using KantanMitsumori.DataAccess;
using KantanMitsumori.Model.Request;

namespace KantanMitsumori.Service.Mapper
{
    public class MapperService : Profile
    {
        public MapperService()
        {
            CreateMap<MMaker, MakerModel>();
        }
    }
}
