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

    public async Task<IEnumerable<Company>> GetCompanies()
    {
        return await _context.Companies.ToListAsync();
    }

    public async Task<Company> GetCompanyByIdAsync(int id)
    {
        return await _context.Companies.FindAsync(id);
    }
}
