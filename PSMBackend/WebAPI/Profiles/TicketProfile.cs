using System;
using AutoMapper;
using PSMModel.Models;
using PSMWebAPI.DTOs;

namespace PSMWebAPI.Profiles;

public class TicketProfile : Profile
{
    public TicketProfile()
    {
        CreateMap<TicketRequest, Ticket>().ReverseMap();

        CreateMap<Ticket, Ticket>().ReverseMap();
    }
}
