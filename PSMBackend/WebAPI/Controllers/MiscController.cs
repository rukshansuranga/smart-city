using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMWebAPI.DTOs.Common;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;
using System.Text.Json;

namespace PSMWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiscController : ControllerBase
    {
        private readonly IMiscRepository _miscRepository;

        public MiscController(IMiscRepository miscRepository)
        {
            _miscRepository = miscRepository;
        }

        [HttpGet]
        [Route("companies")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _miscRepository.GetContractors();
                return Ok(ApiResponse<object>.Success(users, "Contractors retrieved successfully")); // Returns 200 OK response with the list of work packages
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving contractors", ex.Message));
            }
        }

        [HttpGet]
        [Route("contractor/{id}")] 
        public async Task<IActionResult> GetContractorById(string id)
        {
            try
            {
                var contractor = await _miscRepository.GetContractorByIdAsync(id);
                if (contractor == null)
                {
                    return NotFound(ApiResponse<object>.Failure($"Contractor with ID '{id}' not found")); // Returns 404 Not Found if the contractor does not exist
                }
                return Ok(ApiResponse<object>.Success(contractor, "Contractor retrieved successfully")); // Returns 200 OK response with the contractor details
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("Error retrieving contractor", ex.Message));
            }
        }

        #region Council Manage

        [HttpGet]
        [Route("councils")]
        public async Task<IActionResult> GetCouncils()
        {
            try
            {
                var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Utils", "councils.json");
                var jsonContent = await System.IO.File.ReadAllTextAsync(jsonFilePath);
                var councils = JsonSerializer.Deserialize<List<Council>>(jsonContent);
                return Ok(ApiResponse<List<Council>>.Success(councils ?? new List<Council>(), "Councils retrieved successfully"));
            }
            catch (FileNotFoundException)
            {
                return NotFound(ApiResponse<List<Council>>.Failure("Councils data file not found."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<Council>>.Failure($"Internal server error: {ex.Message}"));
            }
        }

        [HttpGet]
        [Route("council/{name}")]
        public async Task<IActionResult> GetCouncilByName(string name)
        {
            try
            {
                var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Utils", "councils.json");
                var jsonContent = await System.IO.File.ReadAllTextAsync(jsonFilePath);
                var councils = JsonSerializer.Deserialize<List<Council>>(jsonContent);
                
                var council = councils?.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
         
                if (council == null)
                {
                    return NotFound(ApiResponse<Council>.Failure($"Council with name '{name}' not found."));
                }

                return Ok(ApiResponse<Council>.Success(council, "Council retrieved successfully"));
            }
            catch (FileNotFoundException)
            {
                return NotFound(ApiResponse<List<Council>>.Failure("Councils data file not found."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<Council>>.Failure($"Internal server error: {ex.Message}"));
            }
        }

        #endregion
    }
}
