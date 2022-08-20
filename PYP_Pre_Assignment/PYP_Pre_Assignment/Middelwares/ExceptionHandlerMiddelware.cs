using Business.DTOs.Common;
using System.Net;
using System.Text.Json;

namespace PYP_Pre_Assignment.API.Middelwares
{
    public class ExceptionHandlerMiddelware
    {
        private readonly RequestDelegate _request;
        private readonly ILogger _logger;

        public ExceptionHandlerMiddelware(RequestDelegate request, ILogger<ExceptionHandlerMiddelware> logger)
        {
            _request = request;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (Exception ex)
            {
                ErrorResponse error = await HandleExceptionAsync(context, ex);
                _logger.LogError(ex, $"Request {context.Request?.Method}: {context.Request?.Path.Value} failed Error: {@error}", error);
            }
        }
        private async Task<ErrorResponse> HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ErrorResponse errorResponse = new()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
            };
            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
            return errorResponse;
        }
    }
}
