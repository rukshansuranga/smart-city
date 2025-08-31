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

    public async Task<Tender?> GetAwardedTenderByProjectId(int id)
    {
        var tender = await _context.Tenders
            .Include(t => t.Contractor)
            .FirstOrDefaultAsync(t => t.TenderId == id);

        return tender;
    }

    public async Task<Tender?> GetByIdAsync(int id)
    {
        return await _context.Tenders.FindAsync(id);
    }

    public async Task<IEnumerable<Tender>> GetTendersByProjectIdAsync(int projectId)
    {
        return await _context.Tenders.Include(t => t.Project)
            .Include(t => t.Contractor)
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<Tender> UpdateAsync(Tender tender)
    {
        try
        {
            _context.Tenders.Update(tender);
            await _context.SaveChangesAsync();
            return tender;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var tender = await _context.Tenders.FindAsync(id);
            if (tender == null)
            {
                return false;
            }

            _context.Tenders.Remove(tender);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    public Task<Tender?> GetTenderByProjectIdAndContractorIdAsync(int projectId, string contractorId)
    {
        return _context.Tenders
            .FirstOrDefaultAsync(t => t.ProjectId == projectId && t.ContractorId == contractorId);
    }

}
