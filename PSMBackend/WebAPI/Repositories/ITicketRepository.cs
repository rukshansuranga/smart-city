using System;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Ticket;

namespace PSMWebAPI.Repositories;

public interface ITicketRepository
{
      Task<IEnumerable<Ticket>> GetResolvedTicketsAsync();
      Task<Ticket> AddAsync(Ticket ticket);
      Task<Ticket> UpdateAsync(Ticket ticket);
      Task<Ticket> GetByIdAsync(int id);
      Task<PageResponse<Ticket>> GetPagingAsync(TicketPaging paging);
      Task<IEnumerable<Ticket>> GetTicketListByWorkPackageIdAsync(int workPackageId);
      Task<Ticket> UpdateTicketHistoryAsync(Ticket updatedTicket, string userId);
      Task<bool> StartWorkOnTicketAsync(int ticketId);
      Task<bool> ResolvedOnTicketAsync(int ticketId);
      Task<bool> CloseOnTicketAsync(int ticketId);
      Task<IEnumerable<Ticket>> GetTicketListByUserIdAsync(string userId);
      Task AddWorkpackagesAsync(int ticketId, IEnumerable<int> workpackageIds);
      Task RemoveWorkpackagesAsync(int ticketId, IEnumerable<int> workpackageIds);
}
