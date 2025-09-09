using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Enums;   
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Complain;
using PSMWebAPI.Profiles;
using PSMWebAPI.Utils;

namespace PSMWebAPI.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationRepository(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task AddAsync(Notification notification)
        {
            notification.CreatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Notification notification)
        {
            var existing = await _context.Notifications.FindAsync(notification.Id);
            if (existing != null)
            {
                _mapper.Map(notification, existing);
                existing.UpdatedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByClientAsync(string clientId)
        {
            return await _context.Notifications
                .Include(w => w.Complain)
                .Include(t => t.Ticket)// Assuming Complain is a navigation property
                .Where(n => n.ClientId == clientId)
                .OrderDescending()
                .ToListAsync();
        }

        public async Task ReadNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);

            if (notification is not null)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddRating(RatingRequest ratingRequest)
    {
    var complain = await _context.Complains.FirstOrDefaultAsync(w => w.ComplainId == ratingRequest.ComplainId);
    if (complain != null)
        {
            complain.Rating = ratingRequest.Rating;
            complain.RatingReview = ratingRequest.Note;
            complain.RatedAt = PSMDateTime.Now;
            complain.RatedBy = ratingRequest.ClientId;
        }

        var notification = await _context.Notifications.FindAsync(ratingRequest.NotificationId);

        if (notification is not null)
        {
            notification.Status = NotificationStatus.Completed;
        }

        await _context.SaveChangesAsync();

    }

        public Task<int> GetUnreadNotificationCountAsync(string clientId)
        {
            return _context.Notifications
                .CountAsync(n => n.ClientId == clientId && !n.IsRead);
        }
    }
}
