using System;
using PSMModel.Models;
using PSMModel.Enums;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Project;

namespace PSMWebAPI.Repositories;

public interface IProjectRepository
{
      Task<Project> AddAsync(Project ticket);
      Task<Project> UpdateAsync(Project ticket);
      Task<Project> GetByIdAsync(int id);
      Task<PageResponse<Project>> GetPagingAsync(ProjectPaging paging);
      Task<IEnumerable<Project>> GetAllProjects();
      Task<IEnumerable<Project>> GetProjectByTypeAndStatusAndName(ProjectType? type, ProjectStatus? status, string? name, string? city,bool? isRecent = false);
      Task<IEnumerable<Project>> GetAllProjectsByContractorId(string contractorId);
      #region Project Progress
      Task<ProjectProgress> AddProjectProgressAsync(ProjectProgress projectProgress);
      Task<IEnumerable<ProjectProgress>> GetAllProjectProgressAsync();
      Task<ProjectProgress?> GetProjectProgressByIdAsync(int projectProgressId);
      Task<IEnumerable<ProjectProgress>> GetProjectProgressByProjectIdAsync(int projectId);
      Task<ProjectProgress> UpdateProjectProgressAsync(ProjectProgress projectProgress);
      Task<bool> DeleteProjectProgressAsync(int projectProgressId);
      Task<ProjectProgress?> GetLatestProjectProgressAsync(int projectId);
      Task<IEnumerable<ProjectProgress>> GetProjectProgressByStatusAsync(ProjectProgressApprovedStatus status);
      #endregion
}
