using System;
using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.tender;

namespace PSMWebAPI.Repositories;

public class TenderRepository : ITenderRepository
{
    private readonly ApplicationDbContext _context;
    public TenderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Tender> AddAsync(Tender tender)
    {
       await _context.Tenders.AddAsync(tender);
        await _context.SaveChangesAsync();
        return tender;
    }

    public async Task<Tender?> GetAwadedTenderByProjectId(int id)
    {
        var tender = await _context.Tenders
            .Include(t => t.Company)
            .FirstOrDefaultAsync(t => t.TenderId == id);

        return tender;
    }

    public async Task<Tender> GetByIdAsync(int id)
    {
        return await _context.Tenders.FindAsync(id);
    }

    

    public async Task<IEnumerable<Tender>> GetTendersByProjectIdAsync(int projectId)
    {
        return _context.Tenders.Include(t => t.Project)
            .Include(t => t.Company)
            .Where(t => t.ProjectId == projectId); 
    }
  
    public async Task<Tender> UpdateAsync(Tender tender)
    {
        try
        {
            _context.Tenders.Update(tender);
            await _context.SaveChangesAsync();
            return tender;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
