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
        CreateMap<TicketPostRequest, InternalTicket>().ReverseMap();
        CreateMap<TicketPostRequest, ComplainTicket>().ReverseMap();
        CreateMap<TicketPostRequest, ProjectTicket>().ReverseMap();

        CreateMap<TicketUpdateRequest, Ticket>()
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<TicketUpdateRequest, InternalTicket>()
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<TicketUpdateRequest, ComplainTicket>()
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<TicketUpdateRequest, ProjectTicket>()
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<Ticket, Ticket>().ReverseMap()
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));


        CreateMap<InternalTicket, InternalTicket>().ReverseMap()
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));


        CreateMap<ComplainTicket, ComplainTicket>().ReverseMap()
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        
        
        CreateMap<ProjectTicket, ProjectTicket>().ReverseMap()
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
    