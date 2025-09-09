using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMWebAPI.DTOs.Common;
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
            try
            {
                var shedule = await _garbageRepository.GetGCSheduleByRegion(regionNo);
                return Ok(ApiResponse<object>.Success(shedule, "Schedule retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Failure($"Failed to retrieve schedule: {ex.Message}"));
            }
        }
    }
}
