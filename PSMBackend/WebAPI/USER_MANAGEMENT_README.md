# User Management with Keycloak Integration

## Overview

This implementation adds user management functionality that integrates with Keycloak for identity and access management. When a user is created, it will be added to both the local database and the Keycloak server.

## Features

- **Dual User Creation**: Creates users in both local database and Keycloak server
- **Transaction Management**: Ensures data consistency with rollback support
- **Password Management**: Supports both provided and auto-generated passwords
- **Comprehensive Error Handling**: Proper error handling and cleanup
- **User Attributes**: Maps all user properties to Keycloak custom attributes

## Architecture

### Components Added

1. **KeycloakUserService** (`Services/KeycloakUserService.cs`):

   - Manages all Keycloak user operations
   - Handles authentication with Keycloak admin API
   - Provides CRUD operations for Keycloak users

2. **UserRepository Enhancement** (`Repositories/UserRepository.cs`):

   - Added `UserAddAsync` method that creates users in both systems
   - Implements transaction management for data consistency
   - Provides rollback functionality if Keycloak creation fails

3. **DTOs**:

   - `KeycloakUserRequest.cs`: Request model for Keycloak user creation
   - `KeycloakUserResponse.cs`: Response models from Keycloak API
   - `CreateUserRequest.cs`: API request model for user creation
   - `CreateUserResponse.cs`: API response model

4. **Controller Enhancement** (`Controllers/UserController.cs`):
   - Added POST endpoint for user creation
   - Comprehensive error handling and validation

## Configuration

The system uses the existing Keycloak configuration from your `appsettings.json`:

```json
{
  "keycloak": {
    "ClientCredentials": {
      "TokenUrl": "https://smartcity-identity-ecesd5fya0buajfs.southeastasia-01.azurewebsites.net/realms/smartcity/protocol/openid-connect/token",
      "ClientId": "smartcity-test",
      "ClientSecret": "Sy7SFs54eJR2SsZbukkeyvcveUkXntYX"
    }
  }
}
```

## Service Registration

The following services have been registered in `Program.cs`:

```csharp
// Register Keycloak User Service
builder.Services.AddHttpClient<IKeycloakUserService, KeycloakUserService>();
```

## API Endpoints

### Create User

**POST** `/api/User`

Creates a new user in both local database and Keycloak.

**Request Body:**

```json
{
  "userId": "test.user.001",
  "mobile": "+94771234567",
  "firstName": "Test",
  "lastName": "User",
  "email": "test.user@smartcity.lk",
  "addressLine1": "123 Main Street",
  "addressLine2": "Apartment 4B",
  "city": "Colombo",
  "designation": "Municipal Officer",
  "authType": "Admin",
  "council": "Colombo Municipal Council",
  "password": "TempPassword123!" // Optional
}
```

**Response:**

```json
{
  "isSuccess": true,
  "message": "User created successfully",
  "data": {
    "success": true,
    "message": "User created successfully in both local database and Keycloak",
    "userId": "test.user.001"
  }
}
```

### Get Users

**GET** `/api/User`

Retrieves all users from the local database.

## User Attributes Mapping

The following user properties are mapped to Keycloak custom attributes:

- `mobile` → Keycloak attribute
- `city` → Keycloak attribute
- `council` → Keycloak attribute
- `designation` → Keycloak attribute
- `authType` → Keycloak attribute (values: Admin, Staff, Contractor, Councillor)

## Transaction Management

The `UserAddAsync` method implements transaction management:

1. **Begin Transaction**: Starts a database transaction
2. **Create in Keycloak**: Attempts to create user in Keycloak first
3. **Create in Database**: If Keycloak succeeds, creates user in local database
4. **Commit**: If both succeed, commits the transaction
5. **Rollback**: If either fails, rolls back database and attempts to cleanup Keycloak

## Error Handling

The system provides comprehensive error handling:

- **InvalidOperationException**: Thrown when Keycloak user creation fails
- **Generic Exception**: Handles database and other unexpected errors
- **Cleanup Logic**: Attempts to remove Keycloak user if database creation fails

## Testing

Use the provided `UserManagement.http` file to test the endpoints:

1. First, get an authentication token using the client credentials flow
2. Use the token to call the user creation endpoint
3. Verify the user exists in both systems

## Keycloak Admin API Requirements

Ensure your Keycloak client has the following permissions:

- `manage-users`: To create, update, and delete users
- `view-users`: To query user information
- `manage-realm`: For broader realm management (if needed)

## Security Considerations

1. **Client Secret**: Store client secrets securely in production
2. **HTTPS**: Always use HTTPS in production environments
3. **Token Management**: The service handles token refresh automatically
4. **Password Policy**: Ensure strong password policies in Keycloak

## Logging

The implementation includes comprehensive logging:

- Successful operations are logged at Information level
- Errors are logged at Error level with full exception details
- Cleanup operations are logged for troubleshooting

## Future Enhancements

Potential improvements for the future:

1. **User Update**: Implement user update synchronization
2. **User Delete**: Add soft delete with Keycloak synchronization
3. **Bulk Operations**: Support for bulk user creation
4. **Password Reset**: Integrate with Keycloak password reset functionality
5. **User Roles**: Map user roles between systems
6. **Audit Trail**: Add comprehensive audit logging

## Troubleshooting

### Common Issues

1. **401 Unauthorized**: Check client credentials and permissions
2. **User Creation Failed**: Verify Keycloak server is accessible
3. **Database Rollback**: Check database connection and transaction support

### Logs to Check

- Application logs for detailed error information
- Keycloak admin console for user creation status
- Database logs for transaction issues
