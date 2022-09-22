using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Entity.ASESTSQL;
using KantanMitsumori.Model;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.Service.Mapper
{
    public class MapperService : Profile
    {
        public MapperService()
        {
            //CreateMap<TEstimate, TEstimateSub>();
            CreateMap<MMaker, MakerModel>();
            CreateMap<MMaker, MakerModel>();

            CreateMap<TEstimate, EstModel>().ReverseMap();
            CreateMap<TEstimateSub, EstModel>().ReverseMap();
            CreateMap<MUserDef, UserDefModel>().ReverseMap();
            CreateMap<MUser, UserModel>().ReverseMap();
            CreateMap<TEstimateIde, EstimateIdeModel>().ReverseMap();
            CreateMap<ResponseInpSitaCar, EstModel>().ReverseMap();
            CreateMap<ResponseInpSyohiyo, EstModel>().ReverseMap();

            CreateMap<AsopMaker, ResponseAsopMaker>();
            CreateMap<AsopCarname, ResponseAsopCarname>();
            CreateMap<TbRuibetsuN, ResponseTbRuibetsuN>();
            CreateMap<TbRuibetsuEntity, ResponseTbRuibetsuNew>();
            CreateMap<ResponseEstimate, TEstimate>();
            CreateMap<SerEstEntity, ResponseSerEst>();
            CreateMap<MUserDef, ResponseUserDef>();
            CreateMap<RequestUpdateInpInitVal, MUserDef>();
        }
    }
}
