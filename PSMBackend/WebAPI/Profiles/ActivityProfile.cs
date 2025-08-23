using System;
using AutoMapper;
using PSMModel.Models;
using PSMWebAPI.DTOs.TicketActivity;

namespace PSMWebAPI.Profiles;

public class ActivityProfile : Profile
{
    public ActivityProfile()
    {
        CreateMap<TicketActivityPostRequest, TicketActivity>();
        CreateMap<TicketActivityUpdateRequest, TicketActivity>();

        CreateMap<TicketActivity, TicketActivity>().ReverseMap()
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
