using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;

namespace KantanMitsumori.Service.Mapper
{
    public class MapperService : Profile
    {
        public MapperService()
        {
            CreateMap<TEstimate, TEstimateSub>();
            CreateMap<MMaker, MakerModel>();
            CreateMap<MMaker, MakerModel>();

            CreateMap<TEstimate, EstmateModel>().ReverseMap();

            CreateMap<TEstimateSub, EstmateSubModel>().ReverseMap();
        }
      
    }
}
