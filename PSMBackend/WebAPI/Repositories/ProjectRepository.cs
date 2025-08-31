using System;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Models;
using PSMModel.Enums;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Project;

namespace PSMWebAPI.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;
    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Project> AddAsync(Project project)
    {
        try
        {
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

        query.OrderByDescending(p => p.Id);

        if (isRecent.HasValue && isRecent.Value)
        {
            query = query.Take(8);
        }

        return query;
    }

    #region Project Progress
    
    // Create new project progress
    public async Task<ProjectProgress> AddProjectProgressAsync(ProjectProgress projectProgress)
    {
        try
        {
            await _context.ProjectProgresses.AddAsync(projectProgress);
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
}
