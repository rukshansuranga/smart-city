using System;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public class MiscRepository : IMiscRepository
{
    private readonly ApplicationDbContext _context;

    public MiscRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contractor>> GetContractors()
    {
        return await _context.Contractors.ToListAsync();
    }

    public async Task<Contractor?> GetContractorByIdAsync(string id)
    {
        return await _context.Contractors.FindAsync(id);
    }
}
