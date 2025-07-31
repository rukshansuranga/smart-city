using System;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Enums;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.DTOs.Response;
using PSMWebAPI.Utils;

namespace PSMWebAPI.Repositories;

public class WorkpackageRepository : IWorkpackageRepository
{
    private readonly ApplicationDbContext _context;
    public WorkpackageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Workpackage> AddWorkpackageAsync(Workpackage workPackage)
    {
        await _context.Workpackages.AddAsync(workPackage);
        await _context.SaveChangesAsync();
        return workPackage;
    }

    public async Task<T> GetByIdAsync<T>(int id) where T : class
    {
        // Uses FindAsync to search for a product by its primary key (ID)
        return await _context.Set<T>().FindAsync(id);
    }
    
    public async Task<T> AddWorkpackageAsync<T>(T workPackage) where T : class
    {
        _context.Set<T>().Add(workPackage);
        await _context.SaveChangesAsync();
        return workPackage;
    }

    public async Task<T> UpdateWorkpackageAsync<T>(T workpackage) where T : class
    {
        try
        {
            var prop = typeof(T).GetProperty("UpdatedAt");
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(workpackage, PSMDateTime.Now);
            }
            _context.Set<T>().Update(workpackage);
            await _context.SaveChangesAsync();
            return workpackage;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public async Task<PageResponse<Workpackage>> GetWorkpackages(ComplainPaging complainPaging)
    {
        try
        {
            // var query = _context.Workpackages
            // .Where(s => complainPaging.Status.Contains(s.Status))
            // .Where(s => s.CreatedDate >= PSMDateTime.Now.PlusDays(-complainPaging.Duration))
            // .OrderBy(s => s.WorkpackageId)
            // .Skip((complainPaging.PageIndex - 1) * complainPaging.PageSize)
            // .Take(complainPaging.PageSize);

            // var list = await query.ToListAsync();

            // var count = _context.Workpackages
            // .Where(s => complainPaging.Status.Contains(s.Status))
            // .Where(s => s.CreatedDate >= PSMDateTime.Now.PlusDays(-complainPaging.Duration))
            // .Count();

            var qurery = _context.Workpackages.Include(t => t.TicketPackages).Where(s => s.IsActive == true).AsQueryable();

            var statusList = new List<string> { "New", "InProgress" };
            if (complainPaging.Status.HasValue)
            {
                qurery = qurery.Where(s => statusList.Contains(s.Status.ToString()));
            }

            if (complainPaging.Duration > 0)
            {
                qurery = qurery.Where(s => s.CreatedAt >= PSMDateTime.Now.PlusDays(-complainPaging.Duration));
            }

            if (!string.IsNullOrEmpty(complainPaging.Type))
            {
                qurery = qurery.Where(s => s.WorkpackageType == complainPaging.Type);
            }

            var count = await qurery.CountAsync();

            var list = await qurery
                .OrderByDescending(s => s.WorkpackageId)
                .Skip((complainPaging.PageIndex - 1) * complainPaging.PageSize)
                .Take(complainPaging.PageSize)
                .ToListAsync();

            // var count = _context.Workpackages
            //     .Where(s => complainPaging.Status.Contains(s.Status))
            //     .Where(s => s.CreatedDate >= PSMDateTime.Now.PlusDays(-complainPaging.Duration))
            //     .Count();

            return new PageResponse<Workpackage> { Records = list, TotalItems = count };
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid paging parameters: " + ex.Message);
        }
    }

    public async Task<IEnumerable<Workpackage>> GetWorkpackagesByTicketId(int ticketId)
    {
        var query = _context.Workpackages
                        .Where(s => s.IsActive == true)
                        .Where(s => s.TicketPackages.Any(c => c.Ticket.TicketId == ticketId));

        var list = await query.ToListAsync();

        return list;
    }

    public async Task DeleteWorkpackageMappingByTicketId(int ticketId, int workpackageId)
    {
        var selectedTicketPackage = await _context.TicketPackages
                        .Where(s => s.TicketId == ticketId && s.WorkpackageId == workpackageId).FirstOrDefaultAsync();

        _context.TicketPackages.Remove(selectedTicketPackage);
        await _context.SaveChangesAsync();
    }

    public async Task AddWorkpackageMappingByTicketId(int ticketId, int workpackageId)
    {
        var newTicketPackaged = new TicketPackage { TicketId = ticketId, WorkpackageId = workpackageId };
        _context.TicketPackages.Add(newTicketPackaged);
        await _context.SaveChangesAsync();
    }



    #region General Complain

    public async Task<IEnumerable<GeneralComplain>> GetGeneralComplain(GeneralComplainGetPagingRequest request)
    {
        //TODO: Implement paging

        // var query = _context.GeneralComplains.Include(x => x.Client).Include(x => x.Comments).Include(x => x.TicketPackages).ThenInclude(x => x.Ticket)
        //     .Where(s => s.IsPrivate == request.IsPrivate)
        //     .OrderBy(s => s.WorkpackageId)
        //     .Skip((request.PageNumber - 1) * request.PageSize)
        //     .Take(request.PageSize);

        var query = _context.GeneralComplains.Include(x => x.Client).Include(x => x.Comments).Include(x => x.TicketPackages).ThenInclude(x => x.Ticket)
            .Where(s => s.IsPrivate == request.IsPrivate)
            .Where(s => s.IsActive == true)
            .OrderBy(s => s.WorkpackageId);
            

        return query;
    }

    #endregion

    #region Light Post Complain


    public async Task<IEnumerable<LightPostComplainDetail>> GetDetailLightPostComplintsByPostIdAndName(string postNo, string name)
    {
        var complainsByPostNo = await _context.LightPostComplains.Include(c => c.Client).Include(t => t.TicketPackages)
            .Where(x => x.IsActive == true && x.LightPostNumber == postNo && x.Subject == name)
            .Select(x => new LightPostComplainDetail { WorkpackageId = x.WorkpackageId, Subject = x.Subject, ClientName = x.Client.FirstName, ComplainDate = PSMDateTime.FormatDate(x.CreatedAt), Status = x.Status.ToString(), TicketList = x.TicketPackages.Select(y => new TicketResponse { Id = y.TicketId, Title = y.Ticket.Subject }).ToList() })
            .ToListAsync();

        return complainsByPostNo;
    }

    public async Task<IEnumerable<LightpostComplainSummary>> GetSummaryLightPostComplintsByPostId(string postNo)
    {
        var complainsSummary = await _context.LightPostComplains
            .Where(x => x.IsActive == true && "New,Open,Progress".Contains(x.Status.ToString()) && x.LightPostNumber == postNo)
            .GroupBy(g => g.Subject)
            .Select(x => new LightpostComplainSummary { Name = x.Key, Count = x.Count() }).ToListAsync();
        return complainsSummary;
    }

    public async Task<IEnumerable<LightPost>> GetLightPostsByCenterPoint(double latitude, double longitude)
    {
        var nearPostList = _context.LightPosts
            .Where(com => Math.Abs(com.Latitude - latitude) < 0.05 && Math.Abs(com.Longitude - longitude) < 0.05);

        return nearPostList;
    }




    #endregion

    #region Project Complain
    public async Task<IEnumerable<ProjectComplain>> GetProjectComplainsByProjectId(int projectId)
    {
        var complains = _context.ProjectComplains.Include(c => c.Client).Include(t => t.TicketPackages)
            .Where(x => x.IsActive == true && x.ProjectId == projectId);
        return complains;
    }


    public async Task<ProjectComplain> GetProjectComplainByWorkpackageId(int workPackageId)
    {
        return await _context.ProjectComplains.Include(c => c.Client).Include(t => t.TicketPackages)
            .FirstOrDefaultAsync(x => x.IsActive == true && x.WorkpackageId == workPackageId);
    }
    #endregion


}
