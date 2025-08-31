using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMWebAPI.Repositories;

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
            var users = await _miscRepository.GetContractors();
            return Ok(users); // Returns 200 OK response with the list of work packages
        }

        [HttpGet]
        [Route("contractor/{id}")] 
        public async Task<IActionResult> GetContractorById(string id)
        {
            var contractor = await _miscRepository.GetContractorByIdAsync(id);
            if (contractor == null)
            {
                return NotFound(); // Returns 404 Not Found if the contractor does not exist
            }
            return Ok(contractor); // Returns 200 OK response with the contractor details
        }
    }
}
