# API Response Pattern Implementation Guide

## Summary

I have successfully implemented a structured API response pattern for your PSM Backend project. This approach provides consistent, user-friendly responses across all your controllers instead of throwing exceptions or returning raw HTTP status codes.

## What Was Created

### 1. ApiResponse<T> Class

**Location**: `d:\PSM\PSMBackend\WebAPI\DTOs\Common\ApiResponse.cs`

A generic response wrapper that includes:

- `IsSuccess`: Boolean indicating success/failure
- `Message`: User-friendly message
- `Data`: The actual response data
- `Errors`: List of error messages
- Static helper methods for common responses

### 2. Controller Extension Methods

**Location**: `d:\PSM\PSMBackend\WebAPI\Extensions\ControllerExtensions.cs`

Convenient extension methods that make it easy to return standardized responses:

- `this.ApiSuccess(data, message)` - Success with data
- `this.ApiSuccess(message)` - Success without data
- `this.ApiFailure<T>(message, errors)` - Failure response
- `this.ApiFailure(message, error)` - Failure with single error

### 3. Updated Controllers

I have fully implemented the pattern in:

- **TenderController** - Complete implementation showing all patterns
- **ProjectController** - Partially updated (GetById, Get methods)
- **UserController** - Updated GetUsers method
- **ComplainController** - Added using statements
- **ClientController** - Added using statements
- **TicketController** - Added using statements

## Implementation Pattern

### Before (Old Way):

```csharp
[HttpPost]
public async Task<IActionResult> Add(TenderPostRequest request)
{
    if (request == null)
    {
        return BadRequest();
    }

    var existingTender = await _tenderRepository.GetTenderByProjectIdAndContractorIdAsync(request.ProjectId, request.ContractorId);
    if (existingTender != null)
    {
        return Conflict("Tender already exists");
    }

    var createdTender = await _tenderRepository.AddAsync(tender);
    return CreatedAtAction(nameof(GetById), new { id = createdTender.TenderId }, createdTender);
}
```

### After (New Way):

```csharp
[HttpPost]
public async Task<IActionResult> Add(TenderPostRequest request)
{
    if (request == null)
    {
        return this.ApiFailure<Tender>("Invalid request data");
    }

    var existingTender = await _tenderRepository.GetTenderByProjectIdAndContractorIdAsync(request.ProjectId, request.ContractorId);
    if (existingTender != null)
    {
        return this.ApiFailure<Tender>("Tender for this project and contractor already exists");
    }

    try
    {
        var createdTender = await _tenderRepository.AddAsync(tender);
        return this.ApiSuccess(createdTender, "Tender created successfully");
    }
    catch (Exception ex)
    {
        return this.ApiFailure<Tender>("An error occurred while creating the tender", ex.Message);
    }
}
```

## To Complete the Implementation

For each remaining controller, follow these steps:

### 1. Add Required Using Statements:

```csharp
using PSMWebAPI.DTOs.Common;
using PSMWebAPI.Extensions;
```

### 2. Update Method Patterns:

**GET Methods (Single Item):**

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    try
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
        {
            return this.ApiFailure<YourModel>("Item not found");
        }
        return this.ApiSuccess(item, "Item retrieved successfully");
    }
    catch (Exception ex)
    {
        return this.ApiFailure<YourModel>("Error retrieving item", ex.Message);
    }
}
```

**GET Methods (List):**

```csharp
[HttpGet]
public async Task<IActionResult> GetAll()
{
    var items = await _repository.GetAllAsync();
    return this.ApiSuccess(items, "Items retrieved successfully");
}
```

**POST Methods:**

```csharp
[HttpPost]
public async Task<IActionResult> Create(CreateRequest request)
{
    if (request == null)
    {
        return this.ApiFailure<YourModel>("Invalid request data");
    }

    // Business validation
    var existingItem = await _repository.CheckExists(request.SomeId);
    if (existingItem != null)
    {
        return this.ApiFailure<YourModel>("Item already exists");
    }

    try
    {
        var newItem = await _repository.CreateAsync(request);
        return this.ApiSuccess(newItem, "Item created successfully");
    }
    catch (Exception ex)
    {
        return this.ApiFailure<YourModel>("Error creating item", ex.Message);
    }
}
```

**PUT Methods:**

```csharp
[HttpPut("{id}")]
public async Task<IActionResult> Update(int id, UpdateRequest request)
{
    if (request == null || id <= 0)
    {
        return this.ApiFailure<YourModel>("Invalid request data or ID");
    }

    var existingItem = await _repository.GetByIdAsync(id);
    if (existingItem == null)
    {
        return this.ApiFailure<YourModel>("Item not found");
    }

    try
    {
        var updatedItem = await _repository.UpdateAsync(existingItem);
        return this.ApiSuccess(updatedItem, "Item updated successfully");
    }
    catch (Exception ex)
    {
        return this.ApiFailure<YourModel>("Error updating item", ex.Message);
    }
}
```

**DELETE Methods:**

```csharp
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
{
    if (id <= 0)
    {
        return this.ApiFailure("Invalid ID");
    }

    var existingItem = await _repository.GetByIdAsync(id);
    if (existingItem == null)
    {
        return this.ApiFailure("Item not found");
    }

    try
    {
        await _repository.DeleteAsync(id);
        return this.ApiSuccess("Item deleted successfully");
    }
    catch (Exception ex)
    {
        return this.ApiFailure("Error deleting item", ex.Message);
    }
}
```

## Frontend Benefits

With this implementation, your frontend will always receive a consistent response structure:

```json
{
  "isSuccess": true,
  "message": "Tender created successfully",
  "data": {
    "tenderId": 123,
    "projectId": 456
    // ... tender data
  },
  "errors": []
}
```

Or for failures:

```json
{
  "isSuccess": false,
  "message": "Tender for this project and contractor already exists",
  "data": null,
  "errors": []
}
```

## Frontend Handling Pattern:

```javascript
const response = await fetch('/api/tender', { method: 'POST', ... });
const result = await response.json();

if (result.isSuccess) {
    // Handle success
    console.log(result.message); // Show success message
    console.log(result.data);    // Use the data
} else {
    // Handle business logic failure
    console.error(result.message); // Show error message to user
    if (result.errors.length > 0) {
        console.error('Errors:', result.errors);
    }
}
```

## Next Steps

1. Apply this pattern to the remaining controllers (ProjectController, ComplainController, etc.)
2. Update any existing frontend code to handle the new response structure
3. Consider adding validation attributes to your DTOs for more comprehensive error handling
4. Add logging for better debugging and monitoring

The TenderController is now fully implemented and serves as a complete example of the pattern!
