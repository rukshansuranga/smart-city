using System;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Models;
using PSMModel.Enums;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Project;
using PSMWebAPI.Utils;
using System.Security.Claims;

namespace PSMWebAPI.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor; 

    public ProjectRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Project> AddAsync(Project project)
    {
        try
        {
            project.CreatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            project.Status = ProjectStatus.New;
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return project;
        }catch(Exception ex)
        {
            throw ex;
        }
        
    }

    public async Task<IEnumerable<Project>> GetAllProjects()
    {
        return await _context.Projects.ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        //var project = await _context.Projects.Include(p => p.AwardedTender).ThenInclude(x => x.Company).FirstOrDefaultAsync(p => p.Id == id);
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        //return await _context.Projects.Include(p => p.AwardedTender).FindAsync(id);
        return project;
    }

    public async Task<PageResponse<Project>> GetPagingAsync(ProjectPaging paging)
    {
        try
        {
            //var query = _context.Projects.Include(x => x.AwardedTender).AsQueryable();
            var query = _context.Projects.AsQueryable();

            if (!string.IsNullOrEmpty(paging.SearchText))
            {
                query = query.Where(t => t.Subject.Contains(paging.SearchText) || t.Description.Contains(paging.SearchText));
            }

            if (paging.StartDate.HasValue)
            {
                query = query.Where(t => t.StartDate >= paging.StartDate.Value);
            }

            if (paging.EndDate.HasValue)
            {
                query = query.Where(t => t.EndDate <= paging.EndDate.Value);
            }

            if (paging.SelectedType.HasValue)
            {
                query = query.Where(t => t.Type == paging.SelectedType.Value);
            }

            var records = await query.ToListAsync();


            var pageRecords = records.OrderByDescending(t => t.Id)
            .Skip((paging.PageNumber - 1) * paging.PageSize)
            .Take(paging.PageSize);

            var count = records.Count();

            var response = new PageResponse<Project> { Records = pageRecords, TotalItems = count };

            return response;
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid paging parameters: " + ex.Message);
        }
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        try
        {
            project.UpdatedAt = PSMDateTime.Now;
            project.UpdatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return project;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<Project>> GetProjectByTypeAndStatusAndName(ProjectType? type, ProjectStatus? status, string? name, string? city,bool? isRecent = false)
    {
        var query = _context.Projects.AsQueryable();

        if (type.HasValue)
        {
            query = query.Where(p => p.Type == type.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(p => p.Subject.ToLower().Contains(name.ToLower()) || p.Description.ToLower().Contains(name.ToLower()));
        }

        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(p => p.City.ToLower().Contains(city.ToLower()));
        }

        query = query.OrderByDescending(p => p.Id);

        if (isRecent.HasValue && isRecent.Value)
        {
            query = query.Take(8);
        }

        return await query.ToListAsync();
    }

    #region Project Progress
    
    // Create new project progress
    public async Task<ProjectProgress> AddProjectProgressAsync(ProjectProgress projectProgress)
    {
        try
        {
            projectProgress.UpdatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _context.ProjectProgresses.AddAsync(projectProgress);
            await _context.SaveChangesAsync();

            //Get Project Cordinator
            var coordinator = await _context.ProjectCoordinators
                .FirstOrDefaultAsync(coor => coor.ProjectId == projectProgress.ProjectId && coor.CoordinatorType == ProjectCoordinatorType.Coordinator && coor.IsActive);

            //create a ticket
            var ticket = new ProjectTicket
            {
                Subject = $"New Ticket for approve project progress, ProjectID: {projectProgress.ProjectId}",
                Detail = $"New Ticket for approve project progress, ProjectID: {projectProgress.ProjectId}",
                Type = TicketType.Internal,
                Status = TicketStatus.Open,
                Priority = TicketPriority.Medium,
                UserId = coordinator?.CoordinatorId,
                ProjectId = projectProgress.ProjectId,

                CreatedAt = PSMDateTime.Now,
                CreatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            };
            await _context.ProjectTickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            return projectProgress;
        }
        catch
        {
            throw;
        }
    }

    // Get all project progress records
    public async Task<IEnumerable<ProjectProgress>> GetAllProjectProgressAsync()
    {
        return await _context.ProjectProgresses
            .Include(pp => pp.Project)
            .Include(pp => pp.ApprovedByUser)
            .ToListAsync();
    }

    // Get project progress by ID
    public async Task<ProjectProgress?> GetProjectProgressByIdAsync(int projectProgressId)
    {
        return await _context.ProjectProgresses
            .Include(pp => pp.Project)
            .Include(pp => pp.ApprovedByUser)
            .FirstOrDefaultAsync(pp => pp.ProjectProgressId == projectProgressId);
    }

    // Get project progress by project ID
    public async Task<IEnumerable<ProjectProgress>> GetProjectProgressByProjectIdAsync(int projectId)
    {
        try
        {
            return await _context.ProjectProgresses
                        .Include(pp => pp.Project)
                        .Include(pp => pp.ApprovedByUser)
                        .Where(pp => pp.ProjectId == projectId)
                        .OrderByDescending(pp => pp.ProgressDate)
                        .ToListAsync();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        
    }

    // Update project progress
    public async Task<ProjectProgress> UpdateProjectProgressAsync(ProjectProgress projectProgress)
    {
        try
        {
            projectProgress.UpdatedAt = PSMDateTime.Now;
            projectProgress.UpdatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _context.ProjectProgresses.Update(projectProgress);
            await _context.SaveChangesAsync();
            return projectProgress;
        }
        catch
        {
            throw;
        }
    }

    // Delete project progress
    public async Task<bool> DeleteProjectProgressAsync(int projectProgressId)
    {
        try
        {
            
            var projectProgress = await _context.ProjectProgresses.FindAsync(projectProgressId);
            if (projectProgress == null)
                return false;

            _context.ProjectProgresses.Remove(projectProgress);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            throw;
        }
    }

    // Get latest project progress for a project
    public async Task<ProjectProgress?> GetLatestProjectProgressAsync(int projectId)
    {
        return await _context.ProjectProgresses
            .Include(pp => pp.Project)
            .Include(pp => pp.ApprovedByUser)
            .Where(pp => pp.ProjectId == projectId)
            .OrderByDescending(pp => pp.ProgressDate)
            .FirstOrDefaultAsync();
    }

    // Get project progress by status
    public async Task<IEnumerable<ProjectProgress>> GetProjectProgressByStatusAsync(ProjectProgressApprovedStatus status)
    {
        return await _context.ProjectProgresses
            .Include(pp => pp.Project)
            .Include(pp => pp.ApprovedByUser)
            .Where(pp => pp.ProjectProgressApprovedStatus == status)
            .OrderByDescending(pp => pp.ProgressDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetRecentProjectsByType(ProjectType? type, int count = 10)
    {
        var query = _context.Projects.AsQueryable();

        if (type.HasValue)
        {
            query = query.Where(p => p.Type == type.Value);
        }

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetAllProjectsByContractorId(string contractorId)
    {
        try
        {
            var sql = @"SELECT p.* FROM public.""Projects"" p 
                       INNER JOIN public.""Tenders"" t ON p.""AwardedTenderId"" = t.""TenderId"" 
                       WHERE t.""ContractorId"" = {0}";
            var projectList = await _context.Projects.FromSqlRaw(sql, contractorId).ToListAsync();
            return projectList;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    #endregion

    #region Coordinator Management
    
    // Create new project coordinator
    public async Task<ProjectCoordinator> AddProjectCoordinatorAsync(ProjectCoordinator projectCoordinator)
    {
        try
        {
            projectCoordinator.CreatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _context.ProjectCoordinators.AddAsync(projectCoordinator);
            await _context.SaveChangesAsync();
            return projectCoordinator;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // Update project coordinator
    public async Task<ProjectCoordinator> UpdateProjectCoordinatorAsync(ProjectCoordinator projectCoordinator)
    {
        try
        {
            projectCoordinator.UpdatedAt = PSMDateTime.Now;
            projectCoordinator.UpdatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _context.ProjectCoordinators.Update(projectCoordinator);
            await _context.SaveChangesAsync();
            return projectCoordinator;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // Get project coordinator by ID
    public async Task<ProjectCoordinator?> GetProjectCoordinatorByIdAsync(int projectCoordinatorId)
    {
        return await _context.ProjectCoordinators
            .Include(pc => pc.Project)
            .Include(pc => pc.Coordinator)
            .FirstOrDefaultAsync(pc => pc.ProjectCoordinatorId == projectCoordinatorId);
    }

    // Get all project coordinators
    public async Task<IEnumerable<ProjectCoordinator>> GetAllProjectCoordinatorsAsync()
    {
        return await _context.ProjectCoordinators
            .Include(pc => pc.Project)
            .Include(pc => pc.Coordinator)
            .Where(pc => pc.IsActive)
            .OrderByDescending(pc => pc.AssignDate)
            .ToListAsync();
    }

    // Get project coordinators by project ID
    public async Task<IEnumerable<ProjectCoordinator>> GetProjectCoordinatorsByProjectIdAsync(int projectId)
    {
        try
        {
            return await _context.ProjectCoordinators
                        .Include(pc => pc.Project)
                        .Include(pc => pc.Coordinator)
                        .Where(pc => pc.ProjectId == projectId && pc.IsActive)
                        .OrderByDescending(pc => pc.AssignDate)
                        .ToListAsync();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // Delete project coordinator
    public async Task<bool> DeleteProjectCoordinatorAsync(int projectCoordinatorId)
    {
        try
        {
            var projectCoordinator = await _context.ProjectCoordinators.FindAsync(projectCoordinatorId);
            if (projectCoordinator == null)
                return false;

            projectCoordinator.IsActive = false; // Soft delete
            projectCoordinator.UpdatedAt = PSMDateTime.Now;
            projectCoordinator.UpdatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            _context.ProjectCoordinators.Update(projectCoordinator);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion
}
