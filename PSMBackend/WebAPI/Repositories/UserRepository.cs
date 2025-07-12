using System;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    // Implementation of IUserRepository methods would go here
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}

