using System;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using PSMDataAccess;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public class RouteRepository : IRouteRepository
{
    private readonly ApplicationDbContext _context;
    public RouteRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Ride> GetLatestRoute(string regionNo, LocalDate date)
    {
        var rides = await _context.Rides.Where(r => r.RegionNo == regionNo && r.StartTime.Date == date).Include(x => x.RidePoints).FirstOrDefaultAsync();
        return rides;
    }
}
