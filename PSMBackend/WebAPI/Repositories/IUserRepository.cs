using System;
using PSMModel.Enums;
using PSMModel.Models;
using PSMWebAPI.DTOs;

namespace PSMWebAPI.Repositories;

public interface IUserRepository
{

    //can you implement pagination
    Task<PageResponse<User>> GetUsers(int pageNumber, int pageSize);
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetUserByUserId(string userId);
    Task<User> UserAddAsync(User user, string userName, string? password = null);
    Task<User?> UpdateUserAsync(string userId, User user);
    Task<bool> DeleteUserAsync(string userId);
    Task<IEnumerable<User>> GetAllUsersByUserTypeAsync(List<AuthType> userTypes);
}
