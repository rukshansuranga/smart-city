namespace PSMWebAPI.Services
{
    public class KeycloakAdminTokenOptions
    {
        public string TokenUrl { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string? Username { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string GrantType { get; set; } = string.Empty; // "client_credentials" or "password"
    }
}
