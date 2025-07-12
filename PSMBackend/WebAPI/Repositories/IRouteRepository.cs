using System;
using NodaTime;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public interface IRouteRepository
{
    Task<Ride> GetLatestRoute(string regionNo, LocalDate date);
}
