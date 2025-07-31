using System;
using AutoMapper;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Ticket;

namespace PSMWebAPI.Profiles;

public class TicketProfile : Profile
{
    public TicketProfile()
    {
        CreateMap<TicketPostRequest, Ticket>().ReverseMap();

        CreateMap<TicketUpdateRequest, Ticket>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<Ticket, Ticket>().ReverseMap();
    }
}
