using System;
using AutoMapper;
using PSMModel.Models;
using PSMWebAPI.DTOs.Notification;

namespace PSMWebAPI.Profiles;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<NotificationPostRequest, Notification>();
        CreateMap<NotificationUpdateRequest, Notification>();

        CreateMap<Notification, Notification>().ReverseMap()
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
