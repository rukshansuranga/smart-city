using System;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Enums;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.DTOs.Response;
using PSMWebAPI.Utils;
using System.Security.Claims;
using PSMWebAPI.DTOs.Workpackage;
using PSMWebAPI.DTOs.Workpackage.LightPostComplain;

namespace PSMWebAPI.Repositories;

public class WorkpackageRepository : IWorkpackageRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkpackageRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Workpackage> AddWorkpackageAsync(Workpackage workPackage)
    {
        await _context.Workpackages.AddAsync(workPackage);
        await _context.SaveChangesAsync();
        return workPackage;
    }

    public async Task<T> GetByIdAsync<T>(int id) where T : class
    {
        var x = typeof(T);
        if (typeof(T) == typeof(ProjectComplain))
        {
            var entity = await _context.ProjectComplains
                .Include(w => w.Client)
                .Include(w => w.Project)
                .FirstOrDefaultAsync(w => w.WorkpackageId == id && w.IsActive == true);
            return entity as T;
        }

        if (typeof(T) == typeof(GeneralComplain))
        {
            var entity = await _context.GeneralComplains
                .Include(w => w.Client)
                .FirstOrDefaultAsync(w => w.WorkpackageId == id && w.IsActive == true);
            return entity as T;
        }

        if (typeof(T) == typeof(LightPostComplain))
        {
            var entity = await _context.LightPostComplains
                .Include(w => w.Client)
                .Include(w => w.LightPost)
                .FirstOrDefaultAsync(w => w.WorkpackageId == id && w.IsActive == true);
            return entity as T;
        }

        if (typeof(T) == typeof(Workpackage))
        {
            var entity = await _context.Workpackages
                .Include(w => w.TicketPackages)
                .FirstOrDefaultAsync(w => w.WorkpackageId == id && w.IsActive == true);
            return entity as T;
        }

        return null;
    }
    public async Task<T> AddWorkpackageAsync<T>(T workPackage) where T : class
    {
        try
        {
            // var prop = typeof(T).GetProperty("CreatedAt");
            // if (prop != null && prop.CanWrite)
            // {
            //     prop.SetValue(workPackage, PSMDateTime.Now);
            // }

            _context.Set<T>().Add(workPackage);
            await _context.SaveChangesAsync();
            return workPackage;
        }
        catch (Exception ex)
        {
            throw new Exception("Error setting CreatedAt property: " + ex.Message);
        }

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
            var user = _httpContextAccessor.HttpContext?.User;

            var userId = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var isAdmin = AuthenticationHelper.IsUserInRole(user, "admin");

            var qurery = _context.Workpackages.Include(t => t.TicketPackages).AsQueryable();

            var statusList = new List<int> { (int)WorkpackageStatus.New, (int)WorkpackageStatus.InProgress };
            if (!string.IsNullOrEmpty(complainPaging.Status))
            {
                qurery = qurery.Where(s => statusList.Contains((int)s.Status));
            }

            if (complainPaging.Duration > 0)
            {
                qurery = qurery.Where(s => s.CreatedAt >= PSMDateTime.Now.PlusDays(-complainPaging.Duration));
            }

            if (complainPaging.Type.HasValue)
            {
                qurery = qurery.Where(s => s.WorkpackageType == complainPaging.Type.Value.ToString());
            }

            if(complainPaging.TicketWorkpackageType.HasValue)
            {
                qurery = qurery.Where(s => s.WorkpackageType == complainPaging.TicketWorkpackageType.Value.ToString());
            }

            if (!isAdmin)
            {
                qurery = qurery.Where(s => s.CreatedBy == userId);
            }

            var count = await qurery.CountAsync();

            //var sql = qurery.ToQueryString();

            var list = await qurery
                .OrderByDescending(s => s.WorkpackageId)
                .Skip((complainPaging.PageIndex - 1) * complainPaging.PageSize)
                .Take(complainPaging.PageSize)
                .ToListAsync();

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
        var clientId = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var query = _context.GeneralComplains.Include(x => x.Client).Include(x => x.Comments).Include(x => x.TicketPackages).ThenInclude(x => x.Ticket)
            .Where(s => s.IsPrivate == request.IsPrivate)
            .Where(s => s.IsActive == true);

        if(request.IsPrivate)
        {
            query = query.Where(s => s.ClientId == clientId);
        }

        query = query.OrderBy(s => s.WorkpackageId);

        return query;
    }

    #endregion

    #region Light Post Complain


    public async Task<IEnumerable<LightPostComplainDetail>> GetDetailLightPostComplintsByPostIdAndName(string postNo, string name, string FirstName)
    {
        var complainsByPostNo = await _context.LightPostComplains.Include(c => c.Client).Include(t => t.TicketPackages)
            .Where(x => x.IsActive == true && x.LightPostNumber == postNo && x.Subject == name)
            .Select(x => new LightPostComplainDetail
            {
                WorkpackageId = x.WorkpackageId,
                Subject = x.Subject,
                ClientId = x.ClientId,
                ClientName = x.Client.FirstName,
                ComplainDate = x.CreatedAt,
                Status = x.Status.ToString(),
                TicketList = x.TicketPackages.Select(y => new TicketResponse { Id = y.TicketId, Title = y.Ticket.Subject }).ToList()
            })
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

    public async Task<IEnumerable<LightPostComplain>> GetLightPostActiveWorkpackagesByMe(string postNo)
    {
        var clientId = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var activeComplainByMe = await _context.LightPostComplains
            .Where(x => x.LightPostNumber == postNo && x.ClientId == clientId && (x.Status == WorkpackageStatus.New || x.Status == WorkpackageStatus.InProgress))
            .ToListAsync();
        return activeComplainByMe;
    }

    public async Task<IEnumerable<ActiveLightPostMarkers>> GetActiveLightPostList()
    {
        var list = await _context.LightPostComplains
            .Include(lp => lp.LightPost)
            .Where(x => x.IsActive == true && x.Status == WorkpackageStatus.New && x.WorkpackageType == WorkpackageType.LightPostComplain.ToString())
            .GroupBy(x => x.LightPostNumber)
            .Select(g => new ActiveLightPostMarkers { LightPostNumber = g.Key, LightPost = g.FirstOrDefault().LightPost, Complains = g.ToList() })
            .ToListAsync();

        return list;
    }

    public async Task<IEnumerable<ActiveLightPostMarkers>> GetActiveAndAssignedLightPostList()
    {
        var list = await _context.LightPostComplains
            .Include(lp => lp.LightPost)
            .Where(x => x.IsActive == true && (x.Status == WorkpackageStatus.New || x.Status == WorkpackageStatus.Assigned) && x.WorkpackageType == WorkpackageType.LightPostComplain.ToString())
            .GroupBy(x => x.LightPostNumber)
            .Select(g => new ActiveLightPostMarkers { LightPostNumber = g.Key, LightPost = g.FirstOrDefault().LightPost, Complains = g.ToList() })
            .ToListAsync();

        return list;
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
