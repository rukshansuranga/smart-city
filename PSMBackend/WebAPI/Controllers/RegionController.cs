using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMWebAPI.Repositories;

namespace PSMWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionRepository _rideRepository;

        public RegionController(IRegionRepository rideRepository)
        {
            _rideRepository = rideRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetRegions()
        {
            var regionList = await _rideRepository.GetRegions();
            return Ok(regionList); // Returns 200 OK response with the list of work packages
        }
    }
}
