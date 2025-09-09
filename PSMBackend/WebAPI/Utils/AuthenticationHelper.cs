using System;
using System.Security.Claims;
using System.Text.Json;

namespace PSMWebAPI.Utils;

public class AuthenticationHelper
{
    public static bool IsUserInRole(ClaimsPrincipal user, string role)
    {
        if (user == null || !user.Identity.IsAuthenticated)
        {
            return false;
        }

        var json = user.FindFirstValue("realm_access");
        var doc = JsonDocument.Parse(json);
        var roles = doc.RootElement.GetProperty("roles").EnumerateArray().Select(x => x.GetString()).ToList();

        return roles.Contains(role);

        //return user.IsInRole(role);
    }
}
