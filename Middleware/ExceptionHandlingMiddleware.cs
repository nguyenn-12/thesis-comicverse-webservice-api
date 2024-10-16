using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using thesis_comicverse_webservice_api.DTOs.ExceptionDTO;
using thesis_comicverse_webservice_api.DTOs.ResponseDTO;

namespace thesis_comicverse_webservice_api.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _logger.LogInformation("Middleware Start");
                await _next(context);
                _logger.LogInformation("Middleware end");

            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred.");

            ExceptionResponse response = exception switch
            {
                ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "Application exception occurred."),
                KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "The request key not found."),
                UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
                ArgumentNullException _ => new ExceptionResponse(HttpStatusCode.BadRequest, exception.Message),
                Exception _ => new ExceptionResponse(HttpStatusCode.BadRequest, exception.Message),


                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, exception.Message)
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            _logger.LogError(JsonConvert.SerializeObject(response));
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
