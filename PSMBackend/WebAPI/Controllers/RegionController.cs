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
            try
            {
                var regionList = await _rideRepository.GetRegions();
                return Ok(ApiResponse<object>.Success(regionList, "Regions retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Failure($"Failed to retrieve regions: {ex.Message}"));
            }
        }
    }
}
