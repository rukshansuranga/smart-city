using Microsoft.Extensions.Options;
using PSMWebAPI.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PSMModel.Models;

namespace PSMWebAPI.Services
{
    public interface IKeycloakUserService
    {
        Task<KeycloakUserResponse?> CreateUserAsync(User user,string userName, string? password = null);
        Task<KeycloakUserResponse?> GetUserByIdAsync(string keycloakUserId);
        Task<KeycloakUserResponse?> GetUserByUsernameAsync(string username);
        Task<bool> UpdateUserAsync(string keycloakUserId, User user);
        Task<bool> DeleteUserAsync(string keycloakUserId);
        Task<bool> ResetUserPasswordAsync(string keycloakUserId, string newPassword, bool temporary = false);
    }

    public class KeycloakUserService : IKeycloakUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IKeycloakTokenService _tokenService;
        private readonly KeycloakClientCredentialsOptions _options;
        private readonly ILogger<KeycloakUserService> _logger;
        private readonly string _realm;

        public KeycloakUserService(
            HttpClient httpClient,
            IKeycloakTokenService tokenService,
            IOptions<KeycloakClientCredentialsOptions> options,
            IConfiguration configuration,
            ILogger<KeycloakUserService> logger)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
            _options = options.Value;
            _logger = logger;
            _realm = "smartcity"; // You can make this configurable if needed
        }

        public async Task<KeycloakUserResponse?> CreateUserAsync(User user,string userName, string? password = null)
        {
            try
            {
                var token = await _tokenService.GetAdminTokenAsync();
                if (token == null)
                {
                    _logger.LogError("Failed to obtain Keycloak admin token");
                    return null;
                }

                var keycloakUser = new KeycloakUserRequest
                {
                    Username = userName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailVerified = !string.IsNullOrEmpty(user.Email),
                    Enabled = true,
                    Attributes = new Dictionary<string, List<string>>
                    {
                        // Custom attributes as defined in your user profile
                        ["mobile"] = new List<string> { user.Mobile ?? "" },
                        ["council"] = new List<string> { user.Council ?? "" },
                    }
                };

                // Add password if provided
                if (!string.IsNullOrEmpty(password))
                {
                    keycloakUser.Credentials = new List<object>
                    {
                        new
                        {
                            type = "password",
                            value = password
                        }
                    };
                }

                var baseUrl = GetKeycloakBaseUrl();
                var url = $"{baseUrl}/admin/realms/{_realm}/users";

                // Clear any existing headers and set fresh authorization
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var json = JsonSerializer.Serialize(keycloakUser, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                // Debug logging to see the actual JSON being sent
                _logger.LogInformation("Sending JSON to Keycloak: {Json}", json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Keycloak returns 201 for successful user creation
                    // The user ID is typically in the Location header
                    var location = response.Headers.Location?.ToString();
                    if (!string.IsNullOrEmpty(location))
                    {
                        var userId = location.Split('/').Last();
                        _logger.LogInformation("Successfully created user in Keycloak with ID: {UserId}", userId);
                        user.UserId = userId;
                        
                        // Return the created user details
                        return await GetUserByIdAsync(userId);
                    }
                    
                    _logger.LogInformation("Successfully created user in Keycloak");
                    return new KeycloakUserResponse
                    {
                        Username = user.UserId,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Enabled = true,
                        Id = user.UserId
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to create user in Keycloak. Status: {StatusCode}, Content: {Content}", 
                        response.StatusCode, errorContent);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while creating user in Keycloak");
                return null;
            }
        }

        public async Task<KeycloakUserResponse?> GetUserByIdAsync(string keycloakUserId)
        {
            try
            {
                var token = await _tokenService.GetAdminTokenAsync();
                if (token == null) return null;

                var baseUrl = GetKeycloakBaseUrl();
                var url = $"{baseUrl}/admin/realms/{_realm}/users/{keycloakUserId}";

                // Clear any existing headers and set fresh authorization
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<KeycloakUserResponse>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while getting user by ID from Keycloak");
                return null;
            }
        }

        public async Task<KeycloakUserResponse?> GetUserByUsernameAsync(string username)
        {
            try
            {
                var token = await _tokenService.GetAdminTokenAsync();
                if (token == null) return null;

                var baseUrl = GetKeycloakBaseUrl();
                var url = $"{baseUrl}/admin/realms/{_realm}/users?username={Uri.EscapeDataString(username)}&exact=true";

                // Clear any existing headers and set fresh authorization
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var users = JsonSerializer.Deserialize<KeycloakUserResponse[]>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    return users?.FirstOrDefault();
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while getting user by username from Keycloak");
                return null;
            }
        }

        public async Task<bool> UpdateUserAsync(string keycloakUserId, User user)
        {
            try
            {
                // Set authorization header with fresh token
                if (!await SetAuthorizationHeaderAsync())
                {
                    return false;
                }

                // First, get the current user from Keycloak to retrieve the username
                var currentKeycloakUser = await GetUserByIdAsync(keycloakUserId);
                if (currentKeycloakUser == null)
                {
                    _logger.LogError("Failed to fetch current user from Keycloak for update. User ID: {UserId}", keycloakUserId);
                    return false;
                }

                var keycloakUser = new KeycloakUserRequest
                {
                    Username = currentKeycloakUser.Username, // Include the username from current user
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailVerified = !string.IsNullOrEmpty(user.Email),
                    Enabled = true,
                    Attributes = new Dictionary<string, List<string>>
                    {
                        // Custom attributes as defined in your user profile
                        ["mobile"] = new List<string> { user.Mobile ?? "" },
                        ["council"] = new List<string> { user.Council ?? "" },
                        // Additional custom attributes for internal use
                        ["city"] = new List<string> { user.City ?? "" },
                        ["addressLine1"] = new List<string> { user.AddressLine1 ?? "" },
                        ["addressLine2"] = new List<string> { user.AddressLine2 ?? "" },
                        ["designation"] = new List<string> { user.Designation?.ToString() ?? "" },
                        ["authType"] = new List<string> { user.AuthType?.ToString() ?? "" }
                    }
                };

                var baseUrl = GetKeycloakBaseUrl();
                var url = $"{baseUrl}/admin/realms/{_realm}/users/{keycloakUserId}";

                var json = JsonSerializer.Serialize(keycloakUser, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                // Log the request details for debugging
                _logger.LogInformation("Making PUT request to Keycloak URL: {Url}", url);
                _logger.LogDebug("Request payload: {Json}", json);
                
                var response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully updated user in Keycloak with ID: {UserId}", keycloakUserId);
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to update user in Keycloak. Status: {StatusCode}, Content: {Content}", 
                        response.StatusCode, errorContent);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating user in Keycloak");
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(string keycloakUserId)
        {
            try
            {
                var token = await _tokenService.GetAdminTokenAsync();
                if (token == null) return false;

                var baseUrl = GetKeycloakBaseUrl();
                var url = $"{baseUrl}/admin/realms/{_realm}/users/{keycloakUserId}";

                // Clear any existing headers and set fresh authorization
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully deleted user from Keycloak with ID: {UserId}", keycloakUserId);
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to delete user from Keycloak. Status: {StatusCode}, Content: {Content}", 
                        response.StatusCode, errorContent);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting user from Keycloak");
                return false;
            }
        }

        public async Task<bool> ResetUserPasswordAsync(string keycloakUserId, string newPassword, bool temporary = false)
        {
            try
            {
                var token = await _tokenService.GetAdminTokenAsync();
                if (token == null) return false;

                // For password reset, Keycloak expects a different JSON structure
                var passwordResetRequest = new
                {
                    type = "password",
                    value = newPassword,
                    temporary = temporary
                };

                var baseUrl = GetKeycloakBaseUrl();
                var url = $"{baseUrl}/admin/realms/{_realm}/users/{keycloakUserId}/reset-password";

                // Clear any existing headers and set fresh authorization
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var json = JsonSerializer.Serialize(passwordResetRequest, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                // Log the request details for debugging
                _logger.LogInformation("Making PUT request to Keycloak for password reset. URL: {Url}", url);
                
                var response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully reset password for user in Keycloak with ID: {UserId}", keycloakUserId);
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to reset password for user in Keycloak. Status: {StatusCode}, Content: {Content}", 
                        response.StatusCode, errorContent);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while resetting user password in Keycloak");
                return false;
            }
        }

        private string GetKeycloakBaseUrl()
        {
            // Extract base URL from token URL
            var tokenUrl = _options.TokenUrl;
            var baseUrl = tokenUrl.Replace($"/realms/{_realm}/protocol/openid-connect/token", "");
            return baseUrl;
        }

        /// <summary>
        /// Helper method to set authorization header with fresh token and clear any existing headers
        /// </summary>
        private async Task<bool> SetAuthorizationHeaderAsync()
        {
            try
            {
                var token = await _tokenService.GetAdminTokenAsync();
                if (token == null || string.IsNullOrEmpty(token.AccessToken))
                {
                    _logger.LogError("Failed to obtain valid Keycloak admin token");
                    return false;
                }

                // Clear any existing headers and set fresh authorization
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token.AccessToken);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while setting authorization header");
                return false;
            }
        }
    }
}
