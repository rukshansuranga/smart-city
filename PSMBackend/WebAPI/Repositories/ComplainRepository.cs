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
using PSMWebAPI.DTOs.Complain;
using PSMWebAPI.DTOs.Complain.LightPostComplain;

namespace PSMWebAPI.Repositories;

public class ComplainRepository : IComplainRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ComplainRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<T> GetByIdAsync<T>(int id) where T : class
    {

        if (typeof(T) == typeof(GeneralComplain))
        {
            var entity = await _context.GeneralComplains
                .Include(w => w.Client)
                .FirstOrDefaultAsync(w => w.ComplainId == id && w.IsActive == true);
            return entity as T;
        }

        if (typeof(T) == typeof(LightPostComplain))
        {
            var entity = await _context.LightPostComplains
                .Include(w => w.Client)
                .Include(w => w.LightPost)
                .FirstOrDefaultAsync(w => w.ComplainId == id && w.IsActive == true);
            return entity as T;
        }

        return null;
    }
    public async Task<T> AddComplainAsync<T>(T complain) where T : Complain
    {
        try
        {
            complain.ClientId = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            complain.CreatedBy = complain.ClientId;

            _context.Set<T>().Add(complain);
            await _context.SaveChangesAsync();
            return complain;
        }
        catch (Exception ex)
        {
            throw new Exception("Error setting CreatedAt property: " + ex.Message);
        }

    }
    public async Task<T> UpdateComplainAsync<T>(T complain) where T : Complain
    {
        try
        {
            var prop = typeof(T).GetProperty("UpdatedAt");
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(complain, PSMDateTime.Now);
            }
            _context.Set<T>().Update(complain);
            await _context.SaveChangesAsync();
            return complain;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public async Task<PageResponse<Complain>> GetComplains(ComplainPaging complainPaging)
    {
        try
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var userId = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var isAdmin = AuthenticationHelper.IsUserInRole(user, "admin");

            var qurery = _context.Complains.Include(t => t.TicketComplains).AsQueryable();

            var statusList = new List<int> { (int)ComplainStatus.New, (int)ComplainStatus.InProgress };
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
                qurery = qurery.Where(s => s.ComplainType == complainPaging.Type.Value.ToString());
            }

            // if(complainPaging.TicketComplainType.HasValue)
            // {
            //     qurery = qurery.Where(s => s.ComplainType == complainPaging.TicketComplainType.Value.ToString());
            // }

            if (!isAdmin)
            {
                qurery = qurery.Where(s => s.CreatedBy == userId);
            }

            var count = await qurery.CountAsync();

            //var sql = qurery.ToQueryString();

            var list = await qurery
                .OrderByDescending(s => s.ComplainId)
                .Skip((complainPaging.PageIndex - 1) * complainPaging.PageSize)
                .Take(complainPaging.PageSize)
                .ToListAsync();

            return new PageResponse<Complain> { Records = list, TotalItems = count };
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid paging parameters: " + ex.Message);
        }
    }
    public async Task<IEnumerable<Complain>> GetComplainsByTicketId(int ticketId)
    {


        var query = _context.Complains
                        .Where(s => s.IsActive == true)
                        .Where(s => s.TicketComplains.Any(c => c.Ticket.TicketId == ticketId));
                        
        var list = await query.ToListAsync();

        return list;
    }

    public async Task DeleteComplainMappingByTicketId(int ticketId, int ComplainId)
    {
        var selectedTicketComplain = await _context.TicketComplains
                        .Where(s => s.TicketId == ticketId && s.ComplainId == ComplainId).FirstOrDefaultAsync();

        _context.TicketComplains.Remove(selectedTicketComplain);
        await _context.SaveChangesAsync();
    }

    public async Task AddComplainMappingByTicketId(int ticketId, int ComplainId)
    {
        var newTicketComplain = new TicketComplain { TicketId = ticketId, ComplainId = ComplainId };
        _context.TicketComplains.Add(newTicketComplain);
        await _context.SaveChangesAsync();
    }

    #region General Complain

    public async Task<IEnumerable<GeneralComplain>> GetGeneralComplain(GeneralComplainGetPagingRequest request)
    {
        var clientId = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var query = _context.GeneralComplains.Include(x => x.Client).Include(x => x.TicketComplains).ThenInclude(x => x.Ticket)
            .Where(s => s.IsPrivate == request.IsPrivate)
            .Where(s => s.IsActive == true);

        if(request.IsPrivate)
        {
            query = query.Where(s => s.ClientId == clientId);
        }

        query = query.OrderBy(s => s.ComplainId);

        return query;
    }

    #endregion

    #region Light Post Complain


    public async Task<IEnumerable<LightPostComplainDetail>> GetDetailLightPostComplintsByPostIdAndName(string postNo, string name, string FirstName)
    {
        var complainsByPostNo = await _context.LightPostComplains.Include(c => c.Client).Include(t => t.TicketComplains)
            .Where(x => x.IsActive == true && x.LightPostNumber == postNo && x.Subject == name)
            .Select(x => new LightPostComplainDetail
            {
                ComplainId = x.ComplainId,
                Subject = x.Subject,
                ClientId = x.ClientId,
                ClientName = x.Client.FirstName,
                ComplainDate = x.CreatedAt,
                Status = x.Status.ToString(),
                TicketList = x.TicketComplains.Select(y => new TicketResponse { Id = y.TicketId, Title = y.Ticket.Subject }).ToList()
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

    public async Task<IEnumerable<LightPostComplain>> GetLightPostActiveComplainsByMe(string postNo)
    {
        var clientId = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var activeComplainByMe = await _context.LightPostComplains
            .Where(x => x.LightPostNumber == postNo && x.ClientId == clientId && (x.Status == ComplainStatus.New || x.Status == ComplainStatus.InProgress))
            .ToListAsync();
        return activeComplainByMe;
    }

    public async Task<IEnumerable<ActiveLightPostMarkers>> GetActiveLightPostList()
    {
        var list = await _context.LightPostComplains
            .Include(lp => lp.LightPost)
            .Where(x => x.IsActive == true && x.Status == ComplainStatus.New && x.ComplainType == ComplainType.LightPostComplain.ToString())
            .GroupBy(x => x.LightPostNumber)
            .Select(g => new ActiveLightPostMarkers { LightPostNumber = g.Key, LightPost = g.FirstOrDefault().LightPost, Complains = g.ToList() })
            .ToListAsync();

        return list;
    }

    public async Task<IEnumerable<ActiveLightPostMarkers>> GetActiveAndAssignedLightPostList()
    {
        var list = await _context.LightPostComplains
            .Include(lp => lp.LightPost)
            .Where(x => x.IsActive == true && (x.Status == ComplainStatus.New || x.Status == ComplainStatus.Assigned) && x.ComplainType == ComplainType.LightPostComplain.ToString())
            .GroupBy(x => x.LightPostNumber)
            .Select(g => new ActiveLightPostMarkers { LightPostNumber = g.Key, LightPost = g.FirstOrDefault().LightPost, Complains = g.ToList() })
            .ToListAsync();

        return list;
    }

    public async Task<IEnumerable<ActiveLightPostMarkers>> GetLightPostListByLocation(double latitude, double longitude, int[] statuses)
    {
        try
        {
            // Define delegate for point-in-circle check
            Func<double, double, double, double, double, bool> pointInCircle = (centerLat, centerLng, pointLat, pointLng, radius) =>
            {
                var distance = Math.Sqrt(Math.Pow(centerLat - pointLat, 2) + Math.Pow(centerLng - pointLng, 2));
                return distance <= radius;
            };

            var query = _context.LightPostComplains
                .Include(lp => lp.LightPost)
                .Where(x => x.IsActive == true && statuses.Contains((int)x.Status))
                .AsEnumerable() // switch to in-memory for delegate
                .Where(x => pointInCircle(latitude, longitude, x.LightPost.Latitude, x.LightPost.Longitude, 10))
                .GroupBy(x => x.LightPostNumber)
                .Select(g => new ActiveLightPostMarkers { LightPostNumber = g.Key, LightPost = g.FirstOrDefault().LightPost, Complains = g.ToList() })
                .ToList();

            return query;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region Project Complain

    public async Task<IEnumerable<ProjectComplain>> GetProjectComplainsByProjectId(int projectId)
    {
        var complains = _context.ProjectComplains.Include(c => c.Client).Include(t => t.TicketComplains)
            .Where(x => x.IsActive == true && x.ProjectId == projectId);
        return complains;
    }

    public async Task<ProjectComplain> GetProjectComplainByComplainId(int ComplainId)
    {
        return await _context.ProjectComplains.Include(c => c.Client).Include(t => t.TicketComplains)
            .FirstOrDefaultAsync(x => x.IsActive == true && x.ComplainId == ComplainId);
    }

    #endregion

}
