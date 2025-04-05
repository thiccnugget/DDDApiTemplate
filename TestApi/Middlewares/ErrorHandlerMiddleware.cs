using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace TestApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var activity = Activity.Current;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, activity);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, Activity? activity)
        {
            var request = context.Request;
            var response = context.Response;
            response.ContentType = "application/json";

            var user = context.User?.Identity?.Name ?? "Anonymous";

            var requestInfo = new
            {
                Path = request.Path,
                Query = request.Query ?? null,
                Headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()) ?? null,
                Method = request.Method,
                Body = await ReadRequestBodyAsync(request) ?? null,
                User = user
            };

            _logger.LogError(exception, "Unhandled exception occurred: {RequestInfo}", JsonSerializer.Deserialize<object>(JsonSerializer.Serialize(requestInfo)));

            activity?.SetTag("error", true);
            activity?.SetTag("error.message", exception.Message);
            activity?.SetTag("error.stack", exception.StackTrace);
            activity?.SetTag("request.info", JsonSerializer.Serialize(requestInfo));

            response.StatusCode = StatusCodes.Status500InternalServerError;
            var errorResponse = new ProblemDetails()
            {
                Title = "Unexpected error",
                Detail = "An unexpected error occurred while processing your request. Please try again later.",
                Extensions = new Dictionary<string, object?>()
                {
                    ["TraceId"] = activity?.Id ?? context.TraceIdentifier,
                }
            };

            await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            if (request.ContentLength == null || request.ContentLength == 0)
                return string.Empty;

            request.EnableBuffering();
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }
    }
}