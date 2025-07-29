using System;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Models;
using PSMWebAPI.DTOs;

namespace PSMWebAPI.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly ApplicationDbContext _context;
    public TicketRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Ticket> AddAsync(Ticket ticket)
    {
        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<PageResponse<Ticket>> GetPagingAsync(TicketPaging paging)
    {
        try
        {
            var query = _context.Tickets.Include(x => x.User)
                .OrderByDescending(t => t.TicketId)
                .Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize);

            var count = _context.Tickets.Count();

            var records = await query.ToListAsync();

            var response = new PageResponse<Ticket> { Records = records, TotalItems = count };

            return response;
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid paging parameters: " + ex.Message);
        }

    }

    public async Task<Ticket> GetByIdAsync(int id)
    {
        return await _context.Tickets.FindAsync(id);
    }

    public async Task<Ticket> UpdateAsync(Ticket ticket)
    {
        try
        {
            //await UpdateTicketAsync(ticket, ticket.UserId);

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
            .Where(t => t.TicketPackages.Any(tp => tp.WorkPackageId == workPackageId))
            .Include(t => t.User)
            .ToListAsync();

        return tickets;
    }

    public async Task<Ticket> UpdateTicketHistoryAsync(Ticket updatedTicket, int userId)
    {
        var existingTicket = await _context.Tickets.FindAsync(updatedTicket.TicketId);

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
}
