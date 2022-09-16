﻿using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
using KantanMitsumori.Entity.ASESTSQL;
using KantanMitsumori.Model.Request;
using KantanMitsumori.Model.Response;

namespace KantanMitsumori.Service.Mapper
{
    public class MapperService : Profile
    {
        public MapperService()
        {
            CreateMap<TEstimate, TEstimateSub>();
            CreateMap<MMaker, MakerModel>();
            CreateMap<MMaker, MakerModel>();
            CreateMap<AsopMaker, ResponseAsopMaker>();
            CreateMap<AsopCarname, ResponseAsopCarname>();
            CreateMap<TbRuibetsuN, ResponseTbRuibetsuN>();
            CreateMap<TbRuibetsuEntity, ResponseTbRuibetsuNew>();         
            CreateMap<ResponseEstimate, TEstimate>();
            CreateMap<SerEstEntity, ResponseSerEst>();
            CreateMap<MUserDef, ResponseUserDef>();
        }

    }
}
