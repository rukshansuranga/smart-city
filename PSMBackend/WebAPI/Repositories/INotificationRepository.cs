using System.Collections.Generic;
using System.Threading.Tasks;
using PSMModel.Models;
using PSMWebAPI.DTOs.Complain;

namespace PSMWebAPI.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<Notification> GetByIdAsync(int id);
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(int id);
        Task<IEnumerable<Notification>> GetNotificationsByClientAsync(string clientId);
        Task ReadNotificationAsync(int notificationId);
        Task AddRating(RatingRequest ratingRequest);
        Task<int> GetUnreadNotificationCountAsync(string clientId);
    }
}
