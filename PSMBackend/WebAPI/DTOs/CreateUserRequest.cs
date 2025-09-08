using System.ComponentModel.DataAnnotations;
using PSMWebAPI.Attributes;
using PSMModel.Enums;

namespace PSMWebAPI.DTOs
{
    public class CreateUserRequest
    {
        // [Required(ErrorMessage = "User ID is required")]
        // [StringLength(255, MinimumLength = 3, ErrorMessage = "User ID must be between 3 and 255 characters")]
        // public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile is required")]
        public string Mobile { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(255, ErrorMessage = "First name cannot exceed 255 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(255, ErrorMessage = "Last name cannot exceed 255 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; } = string.Empty;

        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string? Designation { get; set; }
        public AuthType? AuthType { get; set; }

        [Required(ErrorMessage = "Council is required")]
        [ValidCouncil]
        public string Council { get; set; } = string.Empty;

        public string? Password { get; set; }
        public string Username { get; set; } = string.Empty;
    }

    public class CreateUserResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? KeycloakUserId { get; set; }
    }
}
