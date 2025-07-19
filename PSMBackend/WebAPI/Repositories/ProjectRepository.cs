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

    public async Task<Project> GetByIdAsync(int id)
    {
        return await _context.Projects.FindAsync(id);
    }

    public async Task<PageResponse<Project>> GetPagingAsync(ProjectPaging paging)
    {
        try
        {
            var query = _context.Projects.Include(x => x.Tender).AsQueryable();

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
}
