using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Enums;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Ticket;
using PSMWebAPI.Utils;

namespace PSMWebAPI.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TicketRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<Ticket> AddAsync(Ticket ticket)
    {
         ticket.CreatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await _context.Tickets.AddAsync(ticket);

        // Update related Workpackage status to Assigned
        if (ticket.TicketPackages != null)
        {
            foreach (var tp in ticket.TicketPackages)
            {
                var workpackage = await _context.Workpackages.FirstOrDefaultAsync(w => w.WorkpackageId == tp.WorkpackageId);
                if (workpackage != null)
                {
                    workpackage.Status = PSMModel.Enums.WorkpackageStatus.Assigned;
                }
            }
        }

        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<PageResponse<Ticket>> GetPagingAsync(TicketPaging paging)
    {
        try
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var userId = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var isAdmin = AuthenticationHelper.IsUserInRole(user, "admin");

            // var query = _context.Tickets.Include(x => x.User)
            //     .OrderByDescending(t => t.TicketId)
            //     .Skip((paging.PageIndex - 1) * paging.PageSize)
            //     .Take(paging.PageSize);

            var query = _context.Tickets.Include(x => x.User).AsQueryable();

            
            if(!isAdmin)
            {
                query = query.Where(s => s.UserId == userId);
            }

            var count = await query.CountAsync();

            var list = await query
                .OrderByDescending(s => s.TicketId)
                .Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync();

            var response = new PageResponse<Ticket> { Records = list, TotalItems = count };

            return response;
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid paging parameters: " + ex.Message);
        }

    }

    public async Task<Ticket> GetByIdAsync(int id)
    {
        return await _context.Tickets
            .Include(x => x.TicketPackages).ThenInclude(x => x.Workpackage).FirstOrDefaultAsync(x => x.TicketId == id);
    }

    public async Task<Ticket> UpdateAsync(Ticket ticket)
    {
        try
        {
            ticket.UpdatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ticket.UpdatedAt = Utils.PSMDateTime.Now;

            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public async Task<IEnumerable<Ticket>> GetTicketListByWorkPackageIdAsync(int workPackageId)
    {
        var tickets = await _context.Tickets
            .Where(t => t.TicketPackages.Any(tp => tp.WorkpackageId == workPackageId))
            .Include(t => t.User)
            .ToListAsync();

        return tickets;
    }

    public async Task<Ticket> UpdateTicketHistoryAsync(Ticket updatedTicket, string userId)
    {
        var existingTicket = await _context.Tickets.FindAsync(updatedTicket.TicketId);

        updatedTicket.CreatedAt = existingTicket.CreatedAt;

        if (existingTicket == null) return null;

        var properties = typeof(Ticket).GetProperties();
        foreach (var prop in properties)
        {
            var oldValue = prop.GetValue(existingTicket)?.ToString();
            var newValue = prop.GetValue(updatedTicket)?.ToString();
            if (oldValue != newValue)
            {
                var history = new TicketHistory
                {
                    TicketId = updatedTicket.TicketId,
                    PropertyName = prop.Name,
                    OldValue = oldValue,
                    NewValue = newValue,
                    ChangedDate = DateTime.UtcNow,
                    ChangedBy = userId
                };
                _context.TicketHistories.Add(history);
            }
        }

        // Update the ticket
        _context.Entry(existingTicket).CurrentValues.SetValues(updatedTicket);
        await _context.SaveChangesAsync();

        return existingTicket;
    }

    public async Task<bool>   StartWorkOnTicketAsync(int ticketId)
    {
        //change ticket status to InProgress
        var ticket = await _context.Tickets.FindAsync(ticketId);
        if (ticket == null) return false;

        ticket.Status = TicketStatus.InProgress;
        
        //change the general complains status to InProgress
        //change the related workpackages status to Resolved
        var workpackages = await _context.TicketPackages
            .Where(tp => tp.TicketId == ticketId)
            .Include(tp => tp.Workpackage)
            .Select(tp => tp.Workpackage)
            .ToListAsync();

        if (workpackages != null)
        {
            foreach (var workpackage in workpackages)
            {
                if (workpackage != null)
                {
                    workpackage.Status = WorkpackageStatus.InProgress;
                    //todo: send notification to the user

                    var workpackageTypeString = workpackage.WorkpackageType switch
                    {
                        "GeneralComplain" => "General Complain",
                        "LightPostComplain" => "Light Post Complain",
                        "ProjectComplain" => "Project Complain",
                        "GarbageComplain" => "Garbage Complain",
                        "Workpackage" => "Workpackage",
                        _ => "Unknown"
                    };

                    Notification notification = new Notification
                    {
                        Subject = $"{workpackageTypeString} In Progress",
                        Message = $"Workpackage {workpackage.Subject} is Started",
                        WorkpackageId = workpackage.WorkpackageId,
                        TicketId = ticketId,
                        ClientId = workpackage.ClientId,
                        Status = NotificationStatus.Created,
                        Type = NotificationType.Info,
                        CreatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                        CreatedAt = Utils.PSMDateTime.Now
                    };

                    _context.Notifications.Add(notification);
                }
            }   
        }
        

        TicketActivity activity = new TicketActivity
        {
            TicketId = ticketId,
            Transition = TicketStatus.InProgress,
            UserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
        };

        _context.TicketActivities.Add(activity);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Ticket>> GetTicketListByUserIdAsync(string userId)
    {
        var tickets = await _context.Tickets
                                .Where(t => t.UserId == userId)
                                .Include(t => t.TicketPackages).ThenInclude(tp => tp.Workpackage).ThenInclude(c => c.Client)
                                .Include(t => t.User)
                                .ToListAsync();

        return tickets;
    }

    public async Task<bool> ResolvedOnTicketAsync(int ticketId)
    {
         //change ticket status to Resolved
        var ticket = await _context.Tickets.FindAsync(ticketId);
        if (ticket == null) return false;

        ticket.Status = TicketStatus.Resolved;

        TicketActivity activity = new TicketActivity
        {
            TicketId = ticketId,
            Transition = TicketStatus.Resolved,
            UserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
        };

        _context.TicketActivities.Add(activity);

        Notification notification = new Notification
        {
            Subject = $"Ticket {ticketId} is resolved",
            Message = $"{ticket.Subject} is now resolved.",
            TicketId = ticketId,
            UserId = ticket.CreatedBy,
            Status = NotificationStatus.Created,
            Type = NotificationType.Info,
            CreatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            CreatedAt = Utils.PSMDateTime.Now
        };

        _context.Notifications.Add(notification);

        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<IEnumerable<Ticket>> GetResolvedTicketsAsync()
    {
        return await _context.Tickets
            .Where(t => t.Status == TicketStatus.Resolved)
            .ToListAsync();
    }

    public async Task<bool> CloseOnTicketAsync(int ticketId)
    {
        try
        {
            //change ticket status to Closed
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return false;

            ticket.Status = TicketStatus.Closed;

            TicketActivity activity = new TicketActivity
            {
                TicketId = ticketId,
                Transition = TicketStatus.Closed,
                UserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            };

            _context.TicketActivities.Add(activity);

            var workpackages = await _context.TicketPackages
                        .Where(tp => tp.TicketId == ticketId)
                        .Include(tp => tp.Workpackage)
                        .Select(tp => tp.Workpackage)
                        .ToListAsync();

            if (workpackages != null)
            {
                foreach (var workpackage in workpackages)
                {
                    if (workpackage != null)
                    {
                        workpackage.Status = WorkpackageStatus.Closed;
                        //todo: send notification to the user

                        var workpackageTypeString = workpackage.WorkpackageType switch
                        {
                            "GeneralComplain" => "General Complain",
                            "LightPostComplain" => "Light Post Complain",
                            "ProjectComplain" => "Project Complain",
                            "GarbageComplain" => "Garbage Complain",
                            "Workpackage" => "Workpackage",
                            _ => "Unknown"
                        };

                        Notification notification = new Notification
                        {
                            Subject = $"{workpackageTypeString} In Close",
                            Message = $"Workpackage {workpackage.Subject} is now closed. Can you give your feedback?",
                            WorkpackageId = workpackage.WorkpackageId,
                            TicketId = ticketId,
                            ClientId = workpackage.ClientId,
                            Status = NotificationStatus.Sent,
                            Type = NotificationType.Rating,
                            CreatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                            CreatedAt = Utils.PSMDateTime.Now
                        };

                        _context.Notifications.Add(notification);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }catch(Exception ex)
        {
            // Log the exception (not implemented here)
            return false;
        }
    }

    public async Task AddWorkpackagesAsync(int ticketId, IEnumerable<int> workpackageIds)
    {
        try
        {
            var ticket = await _context.Tickets.Include(x => x.TicketPackages).FirstOrDefaultAsync(t => t.TicketId == ticketId);
            if (ticket == null) throw new KeyNotFoundException("Ticket not found");

        var workpackages = await _context.Workpackages.Where(w => workpackageIds.Contains(w.WorkpackageId)).ToListAsync();
        if (workpackages.Count == 0) throw new KeyNotFoundException("No workpackages found");

        foreach (var workpackage in workpackages)
        {
            if(ticket.TicketPackages.Any(tp => tp.WorkpackageId == workpackage.WorkpackageId))
            {
                continue; // Skip if already added
            }

            ticket.TicketPackages.Add(new TicketPackage
            {
                TicketId = ticketId,
                WorkpackageId = workpackage.WorkpackageId
            });

            workpackage.Status = WorkpackageStatus.Assigned;
        }

         await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        // Log the exception (not implemented here)
        throw;
    }
}

    public async Task RemoveWorkpackagesAsync(int ticketId, IEnumerable<int> workpackageIds)
    {
        var ticket = await _context.Tickets.Include(t => t.TicketPackages).FirstOrDefaultAsync(t => t.TicketId == ticketId);
        if (ticket == null) throw new KeyNotFoundException("Ticket not found");

        var workpackagesToRemove =  ticket.TicketPackages.Where(tp => workpackageIds.Contains(tp.WorkpackageId)).ToList();
        if (workpackagesToRemove.Count == 0) throw new KeyNotFoundException("No workpackages found");

        foreach (var tp in workpackagesToRemove)
        {
            ticket.TicketPackages.Remove(tp);
            var workpackage = _context.Workpackages.Find(tp.WorkpackageId);
            if (workpackage != null)
            {
                if(_context.TicketPackages.Where(x => x.WorkpackageId == workpackage.WorkpackageId).Count() == 0)
                {
                    workpackage.Status = WorkpackageStatus.New; // or whatever status you want to set
                }
            }
        }

         await _context.SaveChangesAsync();
    }
}
