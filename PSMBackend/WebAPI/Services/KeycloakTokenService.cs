using Microsoft.Extensions.Options;
using PSMWebAPI.DTOs;
using System.Text;
using System.Text.Json;

namespace PSMWebAPI.Services
{
    public interface IKeycloakTokenService
    {
        Task<KeycloakTokenResponse?> GetClientCredentialsTokenAsync();
        Task<KeycloakTokenResponse?> GetAdminTokenAsync();
    }

    public class KeycloakTokenService : IKeycloakTokenService
    {
        private readonly HttpClient _httpClient;
        private readonly KeycloakClientCredentialsOptions _clientCredentialsOptions;
        private readonly KeycloakAdminTokenOptions _adminTokenOptions;
        private readonly ILogger<KeycloakTokenService> _logger;
        
        // Token caching with expiry
        private KeycloakTokenResponse? _cachedAdminToken;
        private DateTime _adminTokenExpiry = DateTime.MinValue;

        public KeycloakTokenService(
            HttpClient httpClient,
            IOptions<KeycloakClientCredentialsOptions> clientCredentialsOptions,
            IOptions<KeycloakAdminTokenOptions> adminTokenOptions,
            ILogger<KeycloakTokenService> logger)
        {
            _httpClient = httpClient;
            _clientCredentialsOptions = clientCredentialsOptions.Value;
            _adminTokenOptions = adminTokenOptions.Value;
            _logger = logger;
        }

        public async Task<KeycloakTokenResponse?> GetClientCredentialsTokenAsync()
        {
            try
            {
                var requestBody = new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "client_credentials"),
                    new("client_id", _clientCredentialsOptions.ClientId),
                    new("client_secret", _clientCredentialsOptions.ClientSecret)
                };

                var formContent = new FormUrlEncodedContent(requestBody);

                var response = await _httpClient.PostAsync(_clientCredentialsOptions.TokenUrl, formContent);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonSerializer.Deserialize<KeycloakTokenResponse>(jsonContent);
                    
                    _logger.LogInformation("Successfully obtained client credentials token");
                    return tokenResponse;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to obtain client credentials token. Status: {StatusCode}, Content: {Content}", 
                        response.StatusCode, errorContent);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while obtaining client credentials token");
                return null;
            }
        }

        public async Task<KeycloakTokenResponse?> GetAdminTokenAsync()
        {
            try
            {
                // Check if we have a valid cached token
                if (_cachedAdminToken != null && DateTime.UtcNow < _adminTokenExpiry)
                {
                    _logger.LogDebug("Using cached admin token");
                    return _cachedAdminToken;
                }

                _logger.LogInformation("Requesting new admin token from Keycloak");
                
                var requestBody = new List<KeyValuePair<string, string>>();

                if (_adminTokenOptions.GrantType.ToLower() == "client_credentials")
                {
                    // Client Credentials flow (recommended for admin operations)
                    requestBody.AddRange(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("client_id", _adminTokenOptions.ClientId),
                        new KeyValuePair<string, string>("client_secret", _adminTokenOptions.ClientSecret)
                    });
                }
                else if (_adminTokenOptions.GrantType.ToLower() == "password")
                {
                    // Password flow (fallback)
                    requestBody.AddRange(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("client_id", _adminTokenOptions.ClientId),
                        new KeyValuePair<string, string>("username", _adminTokenOptions.Username ?? ""),
                        new KeyValuePair<string, string>("password", _adminTokenOptions.Password ?? "")
                    });
                }
                else
                {
                    _logger.LogError("Invalid grant type specified for admin token: {GrantType}", _adminTokenOptions.GrantType);
                    return null;
                }

                var formContent = new FormUrlEncodedContent(requestBody);

                // Clear any existing authorization headers before token request
                _httpClient.DefaultRequestHeaders.Authorization = null;
                
                var response = await _httpClient.PostAsync(_adminTokenOptions.TokenUrl, formContent);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonSerializer.Deserialize<KeycloakTokenResponse>(jsonContent);
                    
                    if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
                    {
                        // Cache the token with a buffer (subtract 30 seconds to be safe)
                        _cachedAdminToken = tokenResponse;
                        _adminTokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 30);
                        
                        _logger.LogInformation("Successfully obtained admin token using {GrantType} flow. Expires in {ExpiresIn} seconds", 
                            _adminTokenOptions.GrantType, tokenResponse.ExpiresIn);
                        return tokenResponse;
                    }
                    
                    _logger.LogError("Received empty or invalid token response from Keycloak");
                    return null;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to obtain admin token. Status: {StatusCode}, Content: {Content}", 
                        response.StatusCode, errorContent);
                    
                    // Clear cached token on failure
                    _cachedAdminToken = null;
                    _adminTokenExpiry = DateTime.MinValue;
                    
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while obtaining admin token");
                
                // Clear cached token on exception
                _cachedAdminToken = null;
                _adminTokenExpiry = DateTime.MinValue;
                
                return null;
            }
        }
    }
}
