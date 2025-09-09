using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMWebAPI.DTOs.Common;
using PSMWebAPI.DTOs;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;
using PSMModel.Models;
using PSMModel.Enums;

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
        [AllowAnonymous] // Temporarily disable auth for testing
        //paging PagingRequest
        public async Task<IActionResult> GetUsers([FromQuery] PagingRequest pagingRequest)
        {
            // Comment out auth check for testing
            // if (User.Identity?.IsAuthenticated != true)
            // {
            //     return Ok(ApiResponse.Failure("User is not authenticated"));
            // }

            var pageResponse = await _userRepository.GetUsers(pagingRequest.PageNumber, pagingRequest.PageSize);

            return Ok(ApiResponse<object>.Success(pageResponse, "Users retrieved successfully")); // Returns 200 OK response with the list of users
        }

        [HttpGet("{userId}")]
        [AllowAnonymous] // Temporarily disable auth for testing
        public async Task<IActionResult> GetUserByUserId(string userId)
        {
            try
            {
                // Comment out auth check for testing
                // if (User.Identity?.IsAuthenticated != true)
                // {
                //     return Ok(ApiResponse<User>.Failure("User is not authenticated"));
                // }

                var user = await _userRepository.GetUserByUserId(userId);
                
                if (user == null)
                {
                    return NotFound(ApiResponse<User>.Failure("User not found"));
                }

                return Ok(ApiResponse<User>.Success(user, "User retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure($"An error occurred while retrieving user: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                if (User.Identity?.IsAuthenticated != true)
                {
                    return Ok(ApiResponse<CreateUserResponse>.Failure("User is not authenticated"));
                }

                // Model validation is handled by data annotations
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<CreateUserResponse>.Failure($"Validation errors: {string.Join(", ", errors)}"));
                }

                // Parse enums if provided
                Designation? designation = null;
                if (!string.IsNullOrEmpty(request.Designation) && 
                    Enum.TryParse<Designation>(request.Designation, out var parsedDesignation))
                {
                    designation = parsedDesignation;
                }

                // Create the user object
                var user = new User
                {
                    UserId = "Id is generate from Keycloak",
                    Mobile = request.Mobile,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    AddressLine1 = request.AddressLine1,
                    AddressLine2 = request.AddressLine2,
                    City = request.City,
                    Designation = designation,
                    AuthType = request.AuthType,
                    Council = request.Council
                };

                // Add user (this will also create the user in Keycloak)
                var createdUser = await _userRepository.UserAddAsync(user, request.Username, request.Password);

                var response = new CreateUserResponse
                {
                    Success = true,
                    Message = "User created successfully in both local database and Keycloak",
                    UserId = createdUser.UserId
                };

                return Ok(ApiResponse<CreateUserResponse>.Success(response, "User created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                // This is thrown when Keycloak user creation fails
                return BadRequest(ApiResponse.Failure($"Failed to create user in Keycloak: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("An error occurred while creating user", ex.Message));
            }
        }

        [HttpPut("{userId}")]
        [AllowAnonymous] // Temporarily disable auth for testing
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserRequest request)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<UpdateUserResponse>.Failure($"Validation errors: {string.Join(", ", errors)}"));
                }

                // Parse enums if provided
                Designation? designation = null;
                if (!string.IsNullOrEmpty(request.Designation) && 
                    Enum.TryParse<Designation>(request.Designation, out var parsedDesignation))
                {
                    designation = parsedDesignation;
                }

                // Create the updated user object
                var updatedUser = new User
                {
                    UserId = userId, // Keep the original userId
                    Mobile = request.Mobile,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    AddressLine1 = request.AddressLine1,
                    AddressLine2 = request.AddressLine2,
                    City = request.City,
                    Designation = designation,
                    AuthType = request.AuthType,
                    Council = request.Council
                };

                // Update user
                var result = await _userRepository.UpdateUserAsync(userId, updatedUser);
                
                if (result == null)
                {
                    return NotFound(ApiResponse<UpdateUserResponse>.Failure("User not found"));
                }

                var response = new UpdateUserResponse
                {
                    Success = true,
                    Message = "User updated successfully",
                    UserId = result.UserId
                };

                return Ok(ApiResponse<UpdateUserResponse>.Success(response, "User updated successfully"));
            }
            catch (Exception ex)
            {
                 return StatusCode(500, ApiResponse.Failure("An error occurred while updating user", ex.Message));
            }
        }

        [HttpDelete("{userId}")]
        [AllowAnonymous] // Temporarily disable auth for testing
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                // Comment out auth check for testing
                // if (User.Identity?.IsAuthenticated != true)
                // {
                //     return Ok(ApiResponse<DeleteUserResponse>.Failure("User is not authenticated"));
                // }

                // Delete user
                var result = await _userRepository.DeleteUserAsync(userId);
                
                if (!result)
                {
                    return NotFound(ApiResponse<DeleteUserResponse>.Failure("User not found"));
                }

                var response = new DeleteUserResponse
                {
                    Success = true,
                    Message = "User deleted successfully",
                    UserId = userId
                };

                return Ok(ApiResponse<DeleteUserResponse>.Success(response, "User deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure("An error occurred while deleting user", ex.Message));
            }
        }

        [HttpGet("by-user-type")]
        public async Task<IActionResult> GetUsersByType([FromQuery] string[] authTypes)
        {
            try
            {
                // Convert string array to AuthType list
                var userTypes = new List<AuthType>();
                
                if (authTypes != null && authTypes.Length > 0)
                {
                    foreach (var authType in authTypes)
                    {
                        if (Enum.TryParse<AuthType>(authType, true, out var parsedAuthType))
                        {
                            userTypes.Add(parsedAuthType);
                        }
                        else if (int.TryParse(authType, out var intValue) && 
                                Enum.IsDefined(typeof(AuthType), intValue))
                        {
                            userTypes.Add((AuthType)intValue);
                        }
                    }
                }
                
                if (userTypes.Count == 0)
                {
                    return BadRequest(ApiResponse.Failure("No valid auth types provided"));
                }

                var users = await _userRepository.GetAllUsersByUserTypeAsync(userTypes);
                return Ok(ApiResponse<IEnumerable<User>>.Success(users, "Users retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Failure($"An error occurred while retrieving users", ex.Message));
            }
        }

    }
}
