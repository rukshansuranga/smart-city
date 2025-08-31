using System;
using AutoMapper;
using PSMModel.Models;
using PSMWebAPI.DTOs.Project;

namespace PSMWebAPI.Profiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectPostRequest, Project>()
            .ForMember(dest => dest.SpecificationDocument, opt => opt.Ignore()) // Handle file upload manually
            .ReverseMap();
        
        CreateMap<Project, Project>().ReverseMap()
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        // ProjectProgress mappings
        CreateMap<ProjectProgressRequest, ProjectProgress>()
            .ForMember(dest => dest.ProjectProgressId, opt => opt.Ignore()) // Auto-generated
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Handled by BaseEntity
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Set manually
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // Handled by BaseEntity
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore()) // Set manually
            .ForMember(dest => dest.IsActive, opt => opt.Ignore()) // Handled by BaseEntity
            .ForMember(dest => dest.Project, opt => opt.Ignore()) // Navigation property
            .ForMember(dest => dest.ApprovedByUser, opt => opt.Ignore()) // Navigation property
            .ReverseMap();
    }
}
