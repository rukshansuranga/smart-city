using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMWebAPI.DTOs.Common;
using PSMWebAPI.Repositories;

namespace PSMWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        
        public async Task<IActionResult> GetUsers()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return Ok(ApiResponse<object>.Failure("User is not authenticated"));
            }

            var users = await _userRepository.GetUsers();
            
            return Ok(ApiResponse<object>.Success(users, "Users retrieved successfully")); // Returns 200 OK response with the list of users
        }

    }
}
