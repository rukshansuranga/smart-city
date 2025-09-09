using System;
using AutoMapper;
using PSMModel.Models;
using PSMWebAPI.DTOs.Client;

namespace PSMWebAPI.Profiles;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        // Mapping between ClientPostRequest and Client
        CreateMap<ClientPostRequest, Client>().ReverseMap();

        CreateMap<Client, Client>().ReverseMap()
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
