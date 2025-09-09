using System.ComponentModel.DataAnnotations;
using PSMWebAPI.Utils;

namespace PSMWebAPI.Attributes
{
    /// <summary>
    /// Validation attribute for council values based on Keycloak user profile
    /// </summary>
    public class ValidCouncilAttribute : ValidationAttribute
    {
        public ValidCouncilAttribute()
        {
            ErrorMessage = CouncilValidator.GetValidCouncilsMessage();
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;

            return CouncilValidator.IsValidCouncil(value.ToString());
        }
    }
}
