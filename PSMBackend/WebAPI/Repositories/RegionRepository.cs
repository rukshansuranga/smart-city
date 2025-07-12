using System;
using PSMDataAccess;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public class RegionRepository : IRegionRepository
{
    private readonly ApplicationDbContext _context;
    public RegionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Region>> GetRegions()
    {
        var regions = _context.Regions;

        return regions;
    }
}
