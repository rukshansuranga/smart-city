using System.Collections.Generic;
using System.Threading.Tasks;
using PSMModel.Models;

namespace PSMWebAPI.Repositories
{
    public interface ITicketActivityRepository
    {
        Task<IEnumerable<TicketActivity>> GetAllAsync();
        Task<TicketActivity> GetByIdAsync(int id);
        Task AddAsync(TicketActivity activity);
        Task UpdateAsync(TicketActivity activity);
        Task DeleteAsync(int id);
    }
}
