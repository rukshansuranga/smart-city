using System;
using AutoMapper;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.DTOs.Complain.GeneralComplain;
using PSMWebAPI.DTOs.Complain.ProjectComplain;

namespace PSMWebAPI.Profiles;

public class ComplainProfile : Profile
{
    public ComplainProfile()
    {
        //project complain

        CreateMap<ProjectComplainPostRequest, ProjectComplain>();

        CreateMap<ProjectComplainUpdateRequest, ProjectComplain>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<ProjectComplain, ProjectComplain>().ReverseMap()
            .ForMember(dest => dest.ComplainType, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        //general complain
        CreateMap<GeneralComplainAddRequest, GeneralComplain>().ReverseMap();

        CreateMap<GeneralComplainUpdateRequest, GeneralComplain>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<GeneralComplain, GeneralComplain>().ReverseMap()
            .ForMember(dest => dest.ComplainType, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        //light post complain
        CreateMap<LightPostComplainRequest, LightPostComplain>().ReverseMap();

        CreateMap<LightPostComplain, LightPostComplain>().ReverseMap()
            .ForMember(dest => dest.ComplainType, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        //workpackage
        CreateMap<ComplainRequest, Complain>().ReverseMap();
        
        CreateMap<Complain, Complain>().ReverseMap()
            .ForMember(dest => dest.ComplainType, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
