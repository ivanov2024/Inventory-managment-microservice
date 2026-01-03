using System.Net;
using System.Text.Json;

namespace InventoryManagment.Middleware
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiExceptionMiddleware(RequestDelegate next)
            => _next = next;
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Proceed to the next middleware/component
                await _next(context);
            }
            catch (Exception ex)
            {
                // Handle unhandled exceptions
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Writes a JSON-formatted error response to the HTTP context for an unhandled exception.
        /// </summary>
        /// <remarks>The response includes the exception message and stack trace in JSON format, and sets
        /// the HTTP status code to 500 (Internal Server Error).</remarks>
        /// <param name="context">The HTTP context for the current request. Used to write the error response.</param>
        /// <param name="exception">The exception that occurred and will be reported in the response.</param>
        /// <returns>A task that represents the asynchronous operation of writing the error response.</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new
            {
                status = 500,
                error = exception.Message,
                stackTrace = exception.StackTrace
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}