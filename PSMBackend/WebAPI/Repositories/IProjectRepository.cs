using System;
using PSMModel.Models;
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
}
