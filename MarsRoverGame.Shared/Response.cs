using System.Text.Json.Serialization;

namespace MarsRoverGame.Shared
{
    public class Response<T>
    {
        public T Data { get; set; }

        public int StatusCode { get; set; }

        public bool IsSuccessful { get; set; }

        public List<string> Errors { get; set; }

        public string Message { get; set; }

        public static Response<T> Success(T data,string message, int statusCode)
        {
            return new Response<T> { 
                Data = data, 
                StatusCode = statusCode, 
                Message = message,
                IsSuccessful = true 
            };
        }

        public static Response<T> Success(int statusCode,string message)
        {
            return new Response<T> { 
                Data = default(T), 
                StatusCode = statusCode,
                Message = message,
                IsSuccessful = true 
            };
        }

        public static Response<T> Fail(List<string> errors, int statusCode)

        {
            return new Response<T>
            {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T> { 
                Errors = new List<string>() { error }, 
                StatusCode = statusCode, 
                IsSuccessful = false 
            };
        }
    }
}