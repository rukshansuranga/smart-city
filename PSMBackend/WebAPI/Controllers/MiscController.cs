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
            var users = await _miscRepository.GetCompanies();
            return Ok(users); // Returns 200 OK response with the list of work packages
        }

        [HttpGet]
        [Route("company/{id:int}")] 
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await _miscRepository.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound(); // Returns 404 Not Found if the company does not exist
            }
            return Ok(company); // Returns 200 OK response with the company details
        }
    }
}
