using System;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
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

    public async Task<WorkPackage> AddWorkpackageAsync(WorkPackage workPackage)
    {
        await _context.WorkPackages.AddAsync(workPackage);
        await _context.SaveChangesAsync();
        return workPackage;
    }

    public async Task<WorkPackage> GetByIdAsync(int id)
    {
        // Uses FindAsync to search for a product by its primary key (ID)
        return await _context.WorkPackages.FindAsync(id);
    }

    public async Task<PageResponse<WorkPackage>> GetWorkPackages(ComplainPaging complainPaging)
    {
        try
        {
            var query = _context.WorkPackages
            .Where(s => complainPaging.Status.Contains(s.Status))
            .Where(s => s.CreatedDate >= PSMDateTime.Now.PlusDays(-complainPaging.Duration))
            .OrderBy(s => s.WorkPackageId)
            .Skip((complainPaging.PageIndex - 1) * complainPaging.PageSize)
            .Take(complainPaging.PageSize);

            var list = await query.ToListAsync();

            var count = _context.WorkPackages
            .Where(s => complainPaging.Status.Contains(s.Status))
            .Where(s => s.CreatedDate >= PSMDateTime.Now.PlusDays(-complainPaging.Duration))
            .Count();

            return new PageResponse<WorkPackage> { Records = list, TotalItems = count };
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid paging parameters: " + ex.Message);
        }
    }

    public async Task<IEnumerable<WorkPackage>> GetWorkPackagesByTicketId(int ticketId)
    {
        var query = _context.WorkPackages
                        .Where(s => s.TicketPackages.Any(c => c.Ticket.TicketId == ticketId));

        var list = await query.ToListAsync();

        return list;
    }

    public async Task DeleteWorkpackageMappingByTicketId(int ticketId, int workpackageId)
    {
        var selectedTicketPackage = await _context.TicketPackages
                        .Where(s => s.TicketId == ticketId && s.WorkPackageId == workpackageId).FirstOrDefaultAsync();

        _context.TicketPackages.Remove(selectedTicketPackage);
        await _context.SaveChangesAsync();
    }

    public async Task AddWorkpackageMappingByTicketId(int ticketId, int workpackageId)
    {
        var newTicketPackaged = new TicketPackage { TicketId = ticketId, WorkPackageId = workpackageId };
        _context.TicketPackages.Add(newTicketPackaged);
        await _context.SaveChangesAsync();
    }

    public async Task<WorkPackage> AddComplainAsync(LightPostComplint lightPostComplain)
    {
        await _context.WorkPackages.AddAsync(lightPostComplain);
        await _context.SaveChangesAsync();
        return lightPostComplain;
    }

    #region General Complain

    public async Task<WorkPackage> AddGeneralComplainAsync(GeneralComplain generalComplain)
    {
        await _context.WorkPackages.AddAsync(generalComplain);
        await _context.SaveChangesAsync();
        return generalComplain;
    }

    public async Task<IEnumerable<GeneralComplain>> GetGeneralComplain(GeneralComplainGetPagingRequest request)
    {
        var query = _context.GeneralComplains.Include(x => x.Client).Include(x => x.Comments).Include(x => x.TicketPackages).ThenInclude(x => x.Ticket)
            .Where(s => s.IsPrivate == request.IsPrivate)
            .OrderBy(s => s.WorkPackageId)
            .Skip((request.PageNumber - 1) * 10)
            .Take(10);

        return query;
    }

    #endregion

    #region Light Post Complain


    public async Task<IEnumerable<LightPostComplainDetail>> GetDetailLightPostComplintsByPostIdAndName(string postNo, string name)
    {
        var complainsByPostNo = await _context.LightPostComplints.Include(c => c.Client).Include(t => t.TicketPackages).Where(x => x.LightPostNumber == postNo && x.Name == name)
            .Select(x => new LightPostComplainDetail { WorkPackageId = x.WorkPackageId, Name = x.Name, ClientName = x.Client.Name, ComplainDate = PSMDateTime.FormatDate(x.CreatedDate), Status = x.Status, TicketList = x.TicketPackages.Select(y => new TicketResponse { Id = y.TicketId, Title = y.Ticket.Title }).ToList() })
            .ToListAsync();

        return complainsByPostNo;
    }

    public async Task<IEnumerable<LightpostComplainSummary>> GetSummaryLightPostComplintsByPostId(string postNo)
    {
        var complainsSummary = await _context.LightPostComplints.Where(x => "New,Open,Progress".Contains(x.Status) && x.LightPostNumber == postNo).GroupBy(g => g.Name).Select(x => new LightpostComplainSummary { Name = x.Key, Count = x.Count() }).ToListAsync();
        return complainsSummary;
    }

    public async Task<IEnumerable<LightPost>> GetLightPostsByCenterPoint(double latitude, double longitude)
    {
        var nearPostList = _context.LightPosts
            .Where(com => Math.Abs(com.Latitude - latitude) < 0.05 && Math.Abs(com.Longitude - longitude) < 0.05);

        return nearPostList;
    }


    #endregion


}
