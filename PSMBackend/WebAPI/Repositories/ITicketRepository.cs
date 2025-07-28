using System;
using PSMModel.Models;
using PSMWebAPI.DTOs;

namespace PSMWebAPI.Repositories;

public interface ITicketRepository
{
      Task<Ticket> AddAsync(Ticket ticket);
      Task<Ticket> UpdateAsync(Ticket ticket);
      Task<Ticket> GetByIdAsync(int id);
      Task<PageResponse<Ticket>> GetPagingAsync(TicketPaging paging);
      Task<IEnumerable<Ticket>> GetTicketListByWorkPackageIdAsync(int workPackageId);
}
