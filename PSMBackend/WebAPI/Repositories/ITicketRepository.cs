using System;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Ticket;

namespace PSMWebAPI.Repositories;

public interface ITicketRepository
{
      Task<Ticket> AddAsync(Ticket ticket);
      Task<Ticket> UpdateAsync(Ticket ticket);
      Task<Ticket> GetByIdAsync(int id);
      Task<PageResponse<Ticket>> GetPagingAsync(TicketPaging paging);
      Task<IEnumerable<Ticket>> GetTicketListByWorkPackageIdAsync(int workPackageId);
      Task<Ticket> UpdateTicketHistoryAsync(Ticket updatedTicket, int userId);
      Task<bool> StartWorkOnTicketAsync(int ticketId);

}
