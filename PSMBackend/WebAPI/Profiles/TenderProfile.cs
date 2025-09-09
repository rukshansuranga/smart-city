using System;
using AutoMapper;
using PSMModel.Models;
using PSMWebAPI.DTOs.tender;

namespace PSMWebAPI.Profiles;

public class TenderProfile : Profile
{
    public TenderProfile()
    {
        CreateMap<TenderPostRequest, Tender>().ReverseMap();

        CreateMap<Tender, Tender>().ReverseMap()
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
