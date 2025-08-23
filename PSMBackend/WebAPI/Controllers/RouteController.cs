using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;
using NodaTime;
using Microsoft.AspNetCore.Authorization;

namespace PSMWebAPI.Controllers
{
    // ...removed [AllowAnonymous] to enforce authentication...
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteRepository _routeRepository;

        public RouteController(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetRideByRegionAndDate([FromQuery] GarbageTrackingRequest  request)
        {
            var currentRide = await _routeRepository.GetLatestRoute(request.RegionNo, request.Date?? PSMDateTime.Now.Date);
            return Ok(currentRide); // Returns 200 OK response with the list of work packages
        }
    }
}
