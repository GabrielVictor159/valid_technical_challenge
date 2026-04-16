using FluentValidation;
using Store.Api.DTOs;
using System.Net;
using System.Text.Json;

namespace Store.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro não tratado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            ValidationException => (int)HttpStatusCode.BadRequest,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var message = statusCode == (int)HttpStatusCode.InternalServerError
            ? "Ocorreu um erro interno no servidor."
            : exception.Message;

        IEnumerable<string>? errors = null;

        if (exception is ValidationException validationException)
        {
            errors = validationException.Errors.Select(x => x.ErrorMessage);
        }

        var response = new ErrorResponse(statusCode, message, errors);

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);

        return context.Response.WriteAsync(json);
    }
}
