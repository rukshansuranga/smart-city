# User Management with Keycloak Integration

This guide explains how to use the Keycloak-integrated user management system in the PSM Backend API.

## Overview

The system creates users in both the local PostgreSQL database and Keycloak server simultaneously, ensuring data consistency and proper authentication integration.

## Features

- **Dual User Creation**: Users are created in both local database and Keycloak
- **Transaction Management**: Database rollback if Keycloak user creation fails
- **Data Validation**: Comprehensive validation using data annotations
- **Custom Attributes**: Support for council and mobile attributes in Keycloak
- **Password Management**: Optional password specification (Keycloak generates if not provided)

## Keycloak User Profile Configuration

The system complies with the following Keycloak user profile:

### Required Attributes

- `username` (unique identifier)
- `email` (valid email format)
- `firstName`
- `lastName`

### Custom Attributes

- `mobile` (phone number)
- `council` (restricted to: "Mahara", "Gampaha", "Biyagama")

### Optional Attributes

- `password` (if not provided, Keycloak will generate one)
- `addressLine1`, `addressLine2`, `city`, `designation`

## API Endpoint

```
POST /api/User
Authorization: Bearer {keycloak_token}
Content-Type: application/json
```

## Request Body Schema

```json
{
  "userId": "string (required)",
  "mobile": "string (optional)",
  "firstName": "string (required)",
  "lastName": "string (required)",
  "email": "string (required, valid email)",
  "addressLine1": "string (optional)",
  "addressLine2": "string (optional)",
  "city": "string (optional)",
  "designation": "string (optional)",
  "authType": "string (optional, values: 'Admin', 'Staff', 'Contractor', 'Councillor')",
  "council": "string (optional, must be one of: 'Mahara', 'Gampaha', 'Biyagama')",
  "password": "string (optional)"
}
```

## Testing with UserManagement.http

1. **Get Authentication Token**:

   - Use the first request to get a Bearer token from Keycloak
   - Copy the `access_token` from the response

2. **Replace Token Variable**:

   - In VS Code, you can set the `access_token` variable by adding this line after getting the token:

   ```
   @access_token = YOUR_ACTUAL_TOKEN_HERE
   ```

3. **Test Valid Scenarios**:

   - Create user with all required fields
   - Create user without password (Keycloak generates)
   - Create user for different councils

4. **Test Validation Scenarios**:
   - Missing required fields (should return 400 Bad Request)
   - Invalid council value (should return 400 Bad Request)
   - Invalid email format (should return 400 Bad Request)

## Response Formats

### Success Response (201 Created)

```json
{
  "success": true,
  "message": "User created successfully in both local database and Keycloak",
  "data": {
    "id": 123,
    "userId": "test.user.001",
    "firstName": "Test",
    "lastName": "User",
    "email": "test.user@smartcity.lk"
    // ... other user properties
  }
}
```

### Validation Error Response (400 Bad Request)

```json
{
  "success": false,
  "message": "Validation failed",
  "errors": {
    "FirstName": ["First name is required"],
    "Email": ["Invalid email format"],
    "Council": ["Council must be one of: Mahara, Gampaha, Biyagama"]
  }
}
```

### Server Error Response (500 Internal Server Error)

```json
{
  "success": false,
  "message": "An error occurred while creating the user",
  "error": "Detailed error message"
}
```

## Implementation Details

### Transaction Management

- Database transaction starts before user creation
- If Keycloak user creation fails, database transaction is rolled back
- If database save fails after Keycloak creation, the Keycloak user is deleted

### Keycloak Attribute Mapping

- Local `userId` → Keycloak `username`
- Local `mobile` → Keycloak custom attribute `mobile`
- Local `council` → Keycloak custom attribute `council`
- Standard attributes (firstName, lastName, email) map directly

### Error Handling

- Comprehensive logging for debugging
- Graceful rollback on failures
- User-friendly error messages
- Detailed validation error responses

## Configuration

### appsettings.json

```json
{
  "Keycloak": {
    "Authority": "https://your-keycloak-server/realms/smartcity",
    "AdminApiUrl": "https://your-keycloak-server",
    "Realm": "smartcity",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret"
  }
}
```

## Troubleshooting

### Common Issues

1. **401 Unauthorized**: Check Bearer token validity
2. **400 Bad Request**: Verify request body validation
3. **500 Internal Server Error**: Check Keycloak connectivity and configuration
4. **User already exists**: Use unique userId values

### Logs

Check application logs for detailed error information and transaction flow.

## Security Considerations

- Always use HTTPS in production
- Protect client credentials
- Validate all input data
- Use proper authentication tokens
- Monitor failed creation attempts
