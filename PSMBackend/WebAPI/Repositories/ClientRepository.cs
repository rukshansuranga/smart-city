using System;
using PSMDataAccess;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;
    public ClientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Client> AddAsync(Client client)
    {
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task<Client> UpdateAsync(Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task<Client> GetByIdAsync(string id)
    {
        return await _context.Clients.FindAsync(id);
    }
}

