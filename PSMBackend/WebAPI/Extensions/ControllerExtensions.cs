using Microsoft.AspNetCore.Mvc;
using PSMWebAPI.DTOs.Common;

namespace PSMWebAPI.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Returns a standardized API response with success status
        /// </summary>
        public static IActionResult ApiSuccess<T>(this ControllerBase controller, T data, string message = "Operation completed successfully")
        {
            return controller.Ok(ApiResponse<T>.Success(data, message));
        }

        /// <summary>
        /// Returns a standardized API response with success status and no data
        /// </summary>
        public static IActionResult ApiSuccess(this ControllerBase controller, string message = "Operation completed successfully")
        {
            return controller.Ok(ApiResponse<object>.Success(message));
        }

        /// <summary>
        /// Returns a standardized API response with failure status
        /// </summary>
        public static IActionResult ApiFailure<T>(this ControllerBase controller, string message, List<string> errors = null)
        {
            return controller.Ok(ApiResponse<T>.Failure(message, errors));
        }

        /// <summary>
        /// Returns a standardized API response with failure status and no specific type
        /// </summary>
        public static IActionResult ApiFailure(this ControllerBase controller, string message, List<string> errors = null)
        {
            return controller.Ok(ApiResponse<object>.Failure(message, errors));
        }

        /// <summary>
        /// Returns a standardized API response with failure status and single error
        /// </summary>
        public static IActionResult ApiFailure<T>(this ControllerBase controller, string message, string error)
        {
            return controller.Ok(ApiResponse<T>.Failure(message, error));
        }

        /// <summary>
        /// Returns a standardized API response with failure status and single error (no specific type)
        /// </summary>
        public static IActionResult ApiFailure(this ControllerBase controller, string message, string error)
        {
            return controller.Ok(ApiResponse<object>.Failure(message, error));
        }
    }
}
