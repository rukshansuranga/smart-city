using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSMWebAPI.Services;

namespace PSMWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IKeycloakTokenService _tokenService;
        private readonly ILogger<TokenController> _logger;

        public TokenController(IKeycloakTokenService tokenService, ILogger<TokenController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Get access token using Client Credentials flow
        /// </summary>
        /// <returns>Access token response</returns>
        [HttpPost("client-credentials")]
        [AllowAnonymous]
        public async Task<IActionResult> GetClientCredentialsToken()
        {
            try
            {
                var tokenResponse = await _tokenService.GetClientCredentialsTokenAsync();

                if (tokenResponse == null)
                {
                    return BadRequest(new { error = "Failed to obtain access token" });
                }

                return Ok(new
                {
                    access_token = tokenResponse.AccessToken,
                    token_type = tokenResponse.TokenType,
                    expires_in = tokenResponse.ExpiresIn,
                    scope = tokenResponse.Scope
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while obtaining client credentials token");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}
