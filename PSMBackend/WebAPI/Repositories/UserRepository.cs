using System;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Models;
using PSMWebAPI.Services;
using PSMWebAPI.DTOs;
using PSMModel.Enums;

namespace PSMWebAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IKeycloakUserService _keycloakUserService;
    private readonly ILogger<UserRepository> _logger;
    
    public UserRepository(
        ApplicationDbContext context, 
        IKeycloakUserService keycloakUserService,
        ILogger<UserRepository> logger)
    {
        _context = context;
        _keycloakUserService = keycloakUserService;
        _logger = logger;
    }

    // Implementation of IUserRepository methods would go here
    public async Task<PageResponse<User>> GetUsers(int pageNumber, int pageSize)
    {
        var query = _context.Users.AsQueryable();
        
        var totalCount = await query.CountAsync();
        
        var users = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            
        return new PageResponse<User>
        {
            TotalItems = totalCount,
            Records = users
        };
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByUserId(string userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<User> UserAddAsync(User user,string userName, string? password = null)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // First, create user in Keycloak
            var keycloakUser = await _keycloakUserService.CreateUserAsync(user, userName, password);
            if (keycloakUser == null)
            {
                _logger.LogError("Failed to create user in Keycloak for user: {UserId}", user.UserId);
                throw new InvalidOperationException("Failed to create user in Keycloak");
            }

            _logger.LogInformation("Successfully created user in Keycloak: {UserId}", user.UserId);

            // Then save user to local database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            
            _logger.LogInformation("Successfully created user in database: {UserId}", user.UserId);
            return user;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to create user: {UserId}", user.UserId);
            
            // If local DB failed but Keycloak succeeded, try to cleanup Keycloak user
            if (ex is not InvalidOperationException) // Only cleanup if Keycloak creation was successful
            {
                try
                {
                    var existingKeycloakUser = await _keycloakUserService.GetUserByUsernameAsync(user.UserId);
                    if (existingKeycloakUser != null)
                    {
                        await _keycloakUserService.DeleteUserAsync(existingKeycloakUser.Id);
                        _logger.LogInformation("Cleaned up Keycloak user after database failure: {UserId}", user.UserId);
                    }
                }
                catch (Exception cleanupEx)
                {
                    _logger.LogError(cleanupEx, "Failed to cleanup Keycloak user after database failure: {UserId}", user.UserId);
                }
            }
            
            throw;
        }
    }

    public async Task<User?> UpdateUserAsync(string userId, User updatedUser)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Find the existing user
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (existingUser == null)
            {
                _logger.LogWarning("User not found for update: {UserId}", userId);
                return null;
            }

            // Update the local database first
            existingUser.Mobile = updatedUser.Mobile;
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Email = updatedUser.Email;
            existingUser.AddressLine1 = updatedUser.AddressLine1;
            existingUser.AddressLine2 = updatedUser.AddressLine2;
            existingUser.City = updatedUser.City;
            existingUser.Designation = updatedUser.Designation;
            existingUser.AuthType = updatedUser.AuthType;
            existingUser.Council = updatedUser.Council;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            // Update user in Keycloak
            try
            {
                await _keycloakUserService.UpdateUserAsync(userId, existingUser);
                _logger.LogInformation("Successfully updated user in Keycloak: {UserId}", userId);
            }
            catch (Exception keycloakEx)
            {
                _logger.LogWarning(keycloakEx, "Failed to update user in Keycloak, but local update succeeded: {UserId}", userId);
                // Don't throw here - local update succeeded, Keycloak update is not critical for this operation
            }

            await transaction.CommitAsync();
            
            _logger.LogInformation("Successfully updated user in database: {UserId}", userId);
            return existingUser;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to update user: {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Find the existing user
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (existingUser == null)
            {
                _logger.LogWarning("User not found for deletion: {UserId}", userId);
                return false;
            }

            // First delete from Keycloak
            try
            {
                var keycloakUser = await _keycloakUserService.GetUserByIdAsync(userId);
                if (keycloakUser != null)
                {
                    var keycloakDeleteSuccess = await _keycloakUserService.DeleteUserAsync(keycloakUser.Id);
                    if (!keycloakDeleteSuccess)
                    {
                        _logger.LogWarning("Failed to delete user from Keycloak: {UserId}", userId);
                        // Continue with local deletion even if Keycloak deletion failed
                    }
                    else
                    {
                        _logger.LogInformation("Successfully deleted user from Keycloak: {UserId}", userId);
                    }
                }
                else
                {
                    _logger.LogWarning("User not found in Keycloak for deletion: {UserId}", userId);
                }
            }
            catch (Exception keycloakEx)
            {
                _logger.LogError(keycloakEx, "Error deleting user from Keycloak: {UserId}", userId);
                // Continue with local deletion even if Keycloak deletion failed
            }

            // Delete from local database
            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            
            _logger.LogInformation("Successfully deleted user from database: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Failed to delete user: {UserId}", userId);
            throw;
        }
    }

    public async Task<IEnumerable<User>> GetAllUsersByUserTypeAsync(List<AuthType> userTypes)
    {
        try
        {
            var users = _context.Users.AsQueryable();

            if (userTypes != null && userTypes.Count > 0)
            {
                users = users.Where(u => u.AuthType.HasValue && userTypes.Contains(u.AuthType.Value));
            }

            return await users.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve users by type");
            throw;
        }
    }
}


