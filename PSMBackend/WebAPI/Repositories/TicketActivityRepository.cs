using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Models;

namespace PSMWebAPI.Repositories
{
    public class TicketActivityRepository : ITicketActivityRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public TicketActivityRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContext = httpContextAccessor;
        }

        public async Task<IEnumerable<TicketActivity>> GetAllAsync()
        {
            return await _context.TicketActivities.ToListAsync();
        }

        public async Task<TicketActivity> GetByIdAsync(int id)
        {
            return await _context.TicketActivities.FindAsync(id);
        }

        public async Task AddAsync(TicketActivity activity)
        {

            activity.CreatedBy = _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _context.TicketActivities.AddAsync(activity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TicketActivity activity)
        {
            var existing = await _context.TicketActivities.FindAsync(activity.Id);
            if (existing != null)
            {
                existing.UpdatedBy = _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _context.Entry(existing).CurrentValues.SetValues(activity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var activity = await _context.TicketActivities.FindAsync(id);
            if (activity != null)
            {
                _context.TicketActivities.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
