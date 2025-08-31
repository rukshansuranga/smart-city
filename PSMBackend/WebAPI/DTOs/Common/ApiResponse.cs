using System;

namespace PSMWebAPI.DTOs.Common
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public ApiResponse()
        {
        }

        public ApiResponse(bool isSuccess, string message, T data = default(T))
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public ApiResponse(bool isSuccess, string message, List<string> errors)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors = errors ?? new List<string>();
        }

        // Static methods for common responses
        public static ApiResponse<T> Success(T data, string message = "Operation completed successfully")
        {
            return new ApiResponse<T>(true, message, data);
        }

        public static ApiResponse<T> Success(string message = "Operation completed successfully")
        {
            return new ApiResponse<T>(true, message);
        }

        public static ApiResponse<T> Failure(string message, List<string> errors = null)
        {
            return new ApiResponse<T>(false, message, errors ?? new List<string>());
        }

        public static ApiResponse<T> Failure(string message, string error)
        {
            return new ApiResponse<T>(false, message, new List<string> { error });
        }
    }
}
