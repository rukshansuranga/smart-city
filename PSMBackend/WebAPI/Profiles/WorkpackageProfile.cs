using System;
using AutoMapper;
using PSMModel.Models;
using PSMWebAPI.DTOs.Workpackage.ProjectComplain;

namespace PSMWebAPI.Profiles;

public class WorkpackageProfile : Profile
{
    public WorkpackageProfile()
    {
        CreateMap<ProjectComplainPostRequest, ProjectComplain>().ReverseMap();
 
        CreateMap<ProjectComplain, ProjectComplain>().ReverseMap();
    }
}
