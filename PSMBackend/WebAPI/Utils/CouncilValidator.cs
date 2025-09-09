namespace PSMWebAPI.Utils
{
    public static class CouncilValidator
    {
        /// <summary>
        /// Valid council values as defined in Keycloak user profile
        /// </summary>
        public static readonly string[] AllowedCouncils = { "Mahara", "Gampaha", "Biyagama" };

        /// <summary>
        /// Validates if a council value is allowed
        /// </summary>
        /// <param name="council">Council value to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidCouncil(string? council)
        {
            if (string.IsNullOrEmpty(council))
                return false;
                
            return AllowedCouncils.Contains(council, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets a formatted error message for invalid council
        /// </summary>
        /// <returns>Error message string</returns>
        public static string GetValidCouncilsMessage()
        {
            return $"Council must be one of: {string.Join(", ", AllowedCouncils)}";
        }
    }
}
