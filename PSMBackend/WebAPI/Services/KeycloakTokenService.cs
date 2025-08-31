using Microsoft.Extensions.Options;
using PSMWebAPI.DTOs;
using System.Text;
using System.Text.Json;

namespace PSMWebAPI.Services
{
    public interface IKeycloakTokenService
    {
        Task<KeycloakTokenResponse?> GetClientCredentialsTokenAsync();
    }

    public class KeycloakTokenService : IKeycloakTokenService
    {
        private readonly HttpClient _httpClient;
        private readonly KeycloakClientCredentialsOptions _options;
        private readonly ILogger<KeycloakTokenService> _logger;

        public KeycloakTokenService(
            HttpClient httpClient,
            IOptions<KeycloakClientCredentialsOptions> options,
            ILogger<KeycloakTokenService> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<KeycloakTokenResponse?> GetClientCredentialsTokenAsync()
        {
            try
            {
                var requestBody = new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "client_credentials"),
                    new("client_id", _options.ClientId),
                    new("client_secret", _options.ClientSecret)
                };

                var formContent = new FormUrlEncodedContent(requestBody);

                var response = await _httpClient.PostAsync(_options.TokenUrl, formContent);

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
                    _logger.LogError("Failed to obtain token. Status: {StatusCode}, Content: {Content}", 
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
    }
}
