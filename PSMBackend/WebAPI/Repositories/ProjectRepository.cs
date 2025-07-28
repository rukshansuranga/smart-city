using System;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Models;
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
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<IEnumerable<Project>> GetAllProjects()
    {
        return await _context.Projects.ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        var project = await _context.Projects.Include(p => p.AwadedTener).ThenInclude(x => x.Company).FirstOrDefaultAsync(p => p.Id == id);
        //return await _context.Projects.Include(p => p.AwadedTener).FindAsync(id);
        return project;
    }

    public async Task<PageResponse<Project>> GetPagingAsync(ProjectPaging paging)
    {
        try
        {
            var query = _context.Projects.Include(x => x.AwadedTener).AsQueryable();

            if (!string.IsNullOrEmpty(paging.SearchText))
            {
                query = query.Where(t => t.Name.Contains(paging.SearchText) || t.Description.Contains(paging.SearchText));
            }

            if (paging.StartDate.HasValue)
            {
                query = query.Where(t => t.StartDate >= paging.StartDate.Value);
            }

            if (paging.EndDate.HasValue)
            {
                query = query.Where(t => t.EndDate <= paging.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(paging.SelectedType))
            {
                query = query.Where(t => t.Type == paging.SelectedType);
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

    public async Task<IEnumerable<Project>> GetProjectByTypeAndStatusAndName(string? type, string? status, string? name, string? city,bool? isRecent = false)
    {
        var query = _context.Projects.AsQueryable();

        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(p => p.Type == type);
        }

        if (!string.IsNullOrEmpty(status))
        {  
            query = query.Where(p => p.Status == status);
        }

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()) || p.Description.ToLower().Contains(name.ToLower()));
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
}
