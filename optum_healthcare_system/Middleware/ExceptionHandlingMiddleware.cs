using System.Net;
using System.Text.Json;
namespace optum_healthcare_system.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        /// <summary>
        /// Invokes the middleware to handle exceptions that occur during the processing of HTTP requests.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>
        /// The task representing the asynchronous operation.
        /// </returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);

            }
        }

        /// <summary>
        /// Handles the exception and returns a JSON response with a generic error message and status code.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="ex"></param>
        /// <returns>
        /// the task representing the asynchronous operation of writing the JSON response to the HTTP context.
        /// </returns>
        /// 
        //As of now we are returning a generic error message and status code. In the future, we can enhance this method to return more specific error messages based on the type of exception.
        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = new { message = "An error occurred while processing your request.", statusCode = 500 };
            var jsonResponse = JsonSerializer.Serialize(response);
            await httpContext.Response.WriteAsync(jsonResponse);
        }
    }
}
