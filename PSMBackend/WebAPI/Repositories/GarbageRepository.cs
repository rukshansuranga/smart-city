using System;
using PSMDataAccess;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public class GarbageRepository : IGarbageRepository
{
    private readonly ApplicationDbContext _context;

    public GarbageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GCShedule>> GetGCSheduleByRegion(string regionNo)
    {
        return  _context.GCShedules
            .Where(s => s.RegionNo == regionNo);
            
    }
}