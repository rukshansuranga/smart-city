using System;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Ticket;

namespace PSMWebAPI.Repositories;

public interface ITicketRepository
{
      Task<IEnumerable<Ticket>> GetResolvedTicketsAsync();
      Task<T> AddAsync<T>(T ticket) where T : Ticket;
      Task<T> UpdateAsync<T>(T ticket) where T : Ticket;
      Task<T> GetByIdAsync<T>(int id) where T : Ticket;
      Task<PageResponse<T>> GetPagingAsync<T>(TicketPaging paging) where T : Ticket;
      Task<IEnumerable<ComplainTicket>> GetTicketListByComplainIdAsync(int ComplainId);
      Task<Ticket> UpdateTicketHistoryAsync(Ticket updatedTicket, string userId);
      Task<bool> StartWorkOnTicketAsync(int ticketId);
      Task<bool> ResolvedOnTicketAsync(int ticketId);
      Task<bool> CloseOnTicketAsync(int ticketId);
      Task<BoardTicket> GetTicketListByUserIdAsync(string userId);
      Task AddComplainsAsync(int ticketId, IEnumerable<int> ComplainIds);
      Task RemoveComplainsAsync(int ticketId, IEnumerable<int> ComplainIds);
}
