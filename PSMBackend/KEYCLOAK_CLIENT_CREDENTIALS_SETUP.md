# Keycloak Client Credentials Setup Guide

This guide explains how to set up Client Credentials flow in Keycloak for service-to-service authentication without username/password.

## Step 1: Create Client in Keycloak Admin Console

### 1.1 Access Keycloak Admin Console

- Navigate to: `https://smartcity-identity-ecesd5fya0buajfs.southeastasia-01.azurewebsites.net/admin`
- Login with your admin credentials

### 1.2 Create New Client

1. Select your realm: `smartcity`
2. Go to **Clients** → **Create Client**
3. Fill in the details:
   - **Client type**: `OpenID Connect`
   - **Client ID**: `psm-backend-service`
   - Click **Next**

### 1.3 Configure Client Settings

1. **Client authentication**: `ON` (enables confidential client)
2. **Authorization**: `OFF` (unless you need fine-grained authorization)
3. **Standard flow**: `OFF` (disable authorization code flow)
4. **Direct access grants**: `OFF` (disable password flow)
5. **Service accounts roles**: `ON` (enables client credentials flow)
6. Click **Next** → **Save**

### 1.4 Get Client Credentials

1. Go to the **Credentials** tab
2. Copy the **Client secret**
3. Update your `appsettings.json` and `appsettings.Development.json`:

```json
"Keycloak": {
  "ClientCredentials": {
    "TokenUrl": "https://your-keycloak-url/realms/smartcity/protocol/openid-connect/token",
    "ClientId": "psm-backend-service",
    "ClientSecret": "paste-your-secret-here"
  }
}
```

## Step 2: Configure Service Account Roles (Optional)

1. In your client settings, go to **Service accounts roles** tab
2. Click **Assign role**
3. Add any realm or client roles your service needs
4. Common roles to assign:
   - `view-users` (if you need to query user information)
   - `manage-users` (if you need to manage users)
   - Custom roles specific to your application

## Step 3: Test the Implementation

### 3.1 Using the API Endpoint

```http
POST http://localhost:5000/api/token/client-credentials
Content-Type: application/json
```

### 3.2 Direct Keycloak Request

```http
POST https://smartcity-identity-ecesd5fya0buajfs.southeastasia-01.azurewebsites.net/realms/smartcity/protocol/openid-connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=client_credentials&client_id=psm-backend-service&client_secret=your-secret
```

### 3.3 Expected Response

```json
{
  "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...",
  "token_type": "Bearer",
  "expires_in": 300,
  "scope": "profile email"
}
```

## Step 4: Using the Token

Once you get the token, use it in your API requests:

```http
GET http://localhost:5000/api/your-protected-endpoint
Authorization: Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...
```

## Key Differences: Client Credentials vs Password Flow

| Aspect            | Client Credentials  | Password Flow       |
| ----------------- | ------------------- | ------------------- |
| **Use Case**      | Service-to-service  | User authentication |
| **Credentials**   | Client ID + Secret  | Username + Password |
| **Token Subject** | Service account     | Actual user         |
| **User Context**  | No user context     | Full user context   |
| **Security**      | High (for services) | Varies by user      |

## Security Best Practices

1. **Store secrets securely**: Use Azure Key Vault or similar services
2. **Rotate secrets regularly**: Set up automatic rotation
3. **Limit scope**: Only assign necessary roles to the service account
4. **Monitor usage**: Keep track of token usage and requests
5. **Use HTTPS**: Always use secure connections in production

## Troubleshooting

### Common Issues:

1. **Invalid client credentials**: Verify client ID and secret
2. **Client not configured**: Ensure "Service accounts roles" is enabled
3. **Missing roles**: Service account needs appropriate roles
4. **Wrong token URL**: Verify the realm and Keycloak URL

### Debug Steps:

1. Check Keycloak logs
2. Verify client configuration
3. Test with direct HTTP requests first
4. Check network connectivity
5. Validate JWT token content

## Implementation Files Created

1. **`DTOs/KeycloakTokenResponse.cs`** - Token response model
2. **`Services/KeycloakClientCredentialsOptions.cs`** - Configuration model
3. **`Services/KeycloakTokenService.cs`** - Token service implementation
4. **`Controllers/TokenController.cs`** - API controller for token requests
5. **Updated `appsettings.json`** - Configuration settings
6. **Updated `PSMBackend.http`** - Test requests

## Next Steps

1. Replace `"your-client-secret-here"` with the actual secret from Keycloak
2. Test the implementation using the provided HTTP requests
3. Integrate token retrieval into your services that need authentication
4. Set up proper secret management for production deployment
