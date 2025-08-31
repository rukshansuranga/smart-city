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
    public async Task<T> AddAsync<T>(T ticket) where T : Ticket
    {
        try
        {
            ticket.CreatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _context.Set<T>().Add(ticket);

        if (typeof(T) == typeof(ComplainTicket) && (ticket as ComplainTicket)?.TicketPackages != null)
        {
            var ticketPackages = (ticket as ComplainTicket)?.TicketPackages;
            foreach (var tp in ticketPackages)
            {
                var complain = await _context.Complains.FirstOrDefaultAsync(w => w.ComplainId == tp.ComplainId);
                if (complain != null)
                {
                        complain.Status = ComplainStatus.Assigned;
                    }
                }
        }

            await _context.SaveChangesAsync();
            return ticket;
        }
        catch (Exception ex)
        {
            throw new Exception("Error setting CreatedAt property: " + ex.Message);
        }
    }

    public async Task<PageResponse<T>> GetPagingAsync<T>(TicketPaging paging) where T : Ticket
    {
        try
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var userId = user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var isAdmin = AuthenticationHelper.IsUserInRole(user, "admin");

            var query = _context.Set<T>().Include(x => (x as Ticket).User).AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(s => s.UserId == userId);
            }

            var count = await query.CountAsync();

            var list = await query
                .OrderByDescending(s => s.TicketId)
                .Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync();

            var response = new PageResponse<T> { Records = list, TotalItems = count };

            return response;

        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid paging parameters: " + ex.Message);
        }

    }

    public async Task<T> GetByIdAsync<T>(int id) where T : Ticket
    {
        if (typeof(T) == typeof(ComplainTicket))
        {
            var entity = await _context.ComplainTickets
                 .Include(x => x.TicketPackages).ThenInclude(x => x.Complain)
                 .Include(x => x.User)
                 .FirstOrDefaultAsync(w => w.TicketId == id && w.IsActive == true);
            return entity as T;
        }

        if (typeof(T) == typeof(ProjectTicket))
        {
            var entity = await _context.ProjectTickets
                 //.Include(x => x.TicketPackages).ThenInclude(x => x.Complain)
                 .Include(x => x.Project)
                 .Include(x => x.User)
                 .FirstOrDefaultAsync(w => w.TicketId == id && w.IsActive == true);
            return entity as T;
        }

        if (typeof(T) == typeof(InternalTicket))
        {
            var entity = await _context.InternalTickets
                 //.Include(x => x.TicketPackages).ThenInclude(x => x.Complain)
                 .Include(x => x.User)
                 .FirstOrDefaultAsync(w => w.TicketId == id && w.IsActive == true);
            return entity as T;
        }


        return await _context.Set<T>()
            .FirstOrDefaultAsync(x => x.TicketId == id);
    }

    public async Task<T> UpdateAsync<T>(T ticket) where T : Ticket
    {
        try
        {
            ticket.UpdatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ticket.UpdatedAt = Utils.PSMDateTime.Now;

            //_context.Tickets.Update(ticket);
            _context.Set<T>().Update(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public async Task<IEnumerable<ComplainTicket>> GetTicketListByComplainIdAsync(int ComplainId)
    {
        var tickets = await _context.ComplainTickets
            .Where(t => t.TicketPackages.Any(tp => tp.ComplainId == ComplainId))
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
        //change the related complains status to Resolved
        var complains = await _context.TicketPackages
            .Where(tp => tp.TicketId == ticketId)
            .Include(tp => tp.Complain)
            .Select(tp => tp.Complain)
            .ToListAsync();

        if (complains != null)
        {
            foreach (var complain in complains)
            {
                if (complain != null)
                {
                    complain.Status = ComplainStatus.InProgress;
                    //todo: send notification to the user

                    var complainTypeString = complain.ComplainType switch
                    {
                        "GeneralComplain" => "General Complain",
                        "LightPostComplain" => "Light Post Complain",
                        "ProjectComplain" => "Project Complain",
                        "GarbageComplain" => "Garbage Complain",
                        "Complain" => "Complain",
                        _ => "Unknown"
                    };

                    Notification notification = new Notification
                    {
                        Subject = $"{complainTypeString} In Progress",
                        Message = $"Complain {complain.Subject} is Started",
                        ComplainId = complain.ComplainId,
                        TicketId = ticketId,
                        ClientId = complain.ClientId,
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

    public async Task<IEnumerable<T>> GetTicketListByUserIdAsync<T>(string userId) where T : Ticket
    {
        if (typeof(T) == typeof(ComplainTicket))
        {
            var complainTickets = await _context.ComplainTickets
                .Where(t => t.UserId == userId)
                .Include(t => t.TicketPackages).ThenInclude(tp => tp.Complain).ThenInclude(c => c.Client)
                .ToListAsync();

            return complainTickets.Cast<T>();
        }

        var tickets = await _context.Tickets
                                .Where(t => t.UserId == userId)
                                //.Include(t => t.TicketPackages).ThenInclude(tp => tp.Complain).ThenInclude(c => c.Client)
                                .Include(t => t.User)
                                .ToListAsync();

        return tickets.Cast<T>();
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

            var complains = await _context.TicketPackages
                        .Where(tp => tp.TicketId == ticketId)
                        .Include(tp => tp.Complain)
                        .Select(tp => tp.Complain)
                        .ToListAsync();

            if (complains != null)
            {
                foreach (var complain in complains)
                {
                    if (complain != null)
                    {
                        complain.Status = ComplainStatus.Closed;
                        //todo: send notification to the user

                        var complainTypeString = complain.ComplainType switch
                        {
                            "GeneralComplain" => "General Complain",
                            "LightPostComplain" => "Light Post Complain",
                            "ProjectComplain" => "Project Complain",
                            "GarbageComplain" => "Garbage Complain",
                            "Complain" => "Complain",
                            _ => "Unknown"
                        };

                        Notification notification = new Notification
                        {
                            Subject = $"{complainTypeString} In Close",
                            Message = $"Complain {complain.Subject} is now closed. Can you give your feedback?",
                            ComplainId = complain.ComplainId,
                            TicketId = ticketId,
                            ClientId = complain.ClientId,
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

    public async Task AddComplainsAsync(int ticketId, IEnumerable<int> ComplainIds)
    {
        try
        {
            var ticket = await _context.ComplainTickets.Include(x => x.TicketPackages).FirstOrDefaultAsync(t => t.TicketId == ticketId);
            if (ticket == null) throw new KeyNotFoundException("Ticket not found");

        var complains = await _context.Complains.Where(w => ComplainIds.Contains(w.ComplainId)).ToListAsync();
        if (complains.Count == 0) throw new KeyNotFoundException("No complains found");

        foreach (var complain in complains)
        {
            if(ticket.TicketPackages.Any(tp => tp.ComplainId == complain.ComplainId))
            {
                continue; // Skip if already added
            }

            ticket.TicketPackages.Add(new TicketPackage
            {
                TicketId = ticketId,
                ComplainId = complain.ComplainId
            });

            complain.Status = ComplainStatus.Assigned;
        }

         await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        // Log the exception (not implemented here)
        throw;
    }
}

    public async Task RemoveComplainsAsync(int ticketId, IEnumerable<int> ComplainIds)
    {
        var ticket = await _context.ComplainTickets.Include(t => t.TicketPackages).FirstOrDefaultAsync(t => t.TicketId == ticketId);
        if (ticket == null) throw new KeyNotFoundException("Ticket not found");

        var complainsToRemove =  ticket.TicketPackages.Where(tp => ComplainIds.Contains(tp.ComplainId)).ToList();
        if (complainsToRemove.Count == 0) throw new KeyNotFoundException("No complains found");

        foreach (var tp in complainsToRemove)
        {
            ticket.TicketPackages.Remove(tp);
            var complain = _context.Complains.Find(tp.ComplainId);
            if (complain != null)
            {
                if(_context.TicketPackages.Where(x => x.ComplainId == complain.ComplainId).Count() == 0)
                {
                    complain.Status = ComplainStatus.New; // or whatever status you want to set
                }
            }
        }

         await _context.SaveChangesAsync();
    }

}
