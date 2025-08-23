using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMWebAPI.Repositories;

namespace PSMWebAPI.Controllers
{
    // ...removed [AllowAnonymous] to enforce authentication...
    [Route("api/[controller]")]
    [ApiController]
    public class GarbageController : ControllerBase
    {
        private readonly IGarbageRepository _garbageRepository;

        public GarbageController(IGarbageRepository garbageRepository)
        {
            _garbageRepository = garbageRepository;
        }

        [HttpGet("{regionNo}")]
        public async Task<IActionResult> GetSheduleByRegion(string regionNo)
        {
            var shedule = await _garbageRepository.GetGCSheduleByRegion(regionNo);
            return Ok(shedule); // Returns 200 OK response with the list of work packages
        }
    }
}
