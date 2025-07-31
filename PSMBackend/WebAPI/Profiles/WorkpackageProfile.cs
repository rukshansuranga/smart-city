using System;
using AutoMapper;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.DTOs.Workpackage.GeneralComplain;
using PSMWebAPI.DTOs.Workpackage.ProjectComplain;

namespace PSMWebAPI.Profiles;

public class WorkpackageProfile : Profile
{
    public WorkpackageProfile()
    {
        //project complain

        CreateMap<ProjectComplainPostRequest, ProjectComplain>();

        CreateMap<ProjectComplainUpdateRequest, ProjectComplain>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<ProjectComplain, ProjectComplain>().ReverseMap()
            .ForMember(dest => dest.WorkpackageType, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        //general complain
        CreateMap<GeneralComplainAddRequest, GeneralComplain>().ReverseMap();

        CreateMap<GeneralComplainUpdateRequest, GeneralComplain>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<GeneralComplain, GeneralComplain>().ReverseMap()
            .ForMember(dest => dest.WorkpackageType, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        //light post complain
        CreateMap<LightPostComplainRequest, LightPostComplain>().ReverseMap();

        CreateMap<LightPostComplain, LightPostComplain>().ReverseMap()
            .ForMember(dest => dest.WorkpackageType, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        //workpackage
        CreateMap<WorkpackageRequest, Workpackage>().ReverseMap();
        CreateMap<Workpackage, Workpackage>().ReverseMap()
            .ForMember(dest => dest.WorkpackageType, opt => opt.Ignore());

    }
}
