using System.Text.Json.Serialization;

namespace PSMWebAPI.DTOs
{
    public class KeycloakUserRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("emailVerified")]
        public bool EmailVerified { get; set; } = false;

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = true;

        [JsonPropertyName("credentials")]
        public List<object>? Credentials { get; set; }

        [JsonPropertyName("attributes")]
        public Dictionary<string, List<string>>? Attributes { get; set; }
    }

    public class KeycloakCredential
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "password";

        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;

        [JsonPropertyName("temporary")]
        public bool Temporary { get; set; } = false;
    }
}
