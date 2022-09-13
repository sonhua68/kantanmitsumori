﻿using AutoMapper;
using KantanMitsumori.Entity.ASESTEntities;
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

            CreateMap<TEstimate, EstimateModel>().ReverseMap();
            CreateMap<TEstimateSub, EstimateSubModel>().ReverseMap();
            CreateMap<MUserDef, UserDefModel>().ReverseMap();
            CreateMap<MUser, UserModel>().ReverseMap();
            CreateMap<TEstimate, ResponEstMainModel>().ReverseMap();
            CreateMap<TEstimateSub, ResponEstMainModel>().ReverseMap();
            CreateMap<TEstimateIde, EstimateIdeModel>().ReverseMap();
        }

    }
}
