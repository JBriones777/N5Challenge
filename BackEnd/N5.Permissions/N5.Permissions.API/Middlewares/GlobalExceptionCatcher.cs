using FluentValidation;
using N5.Permissions.Application.Dtos;

namespace N5.Permissions.API.Middlewares;

public class GlobalExceptionCatcher(RequestDelegate next, ILogger<GlobalExceptionCatcher> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionCatcher> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await WriteExceptionResponse(context, ex);
        }
    }
    private async Task WriteExceptionResponse(HttpContext context, Exception ex)
    {

        LogLevel logLevel = LogLevel.Critical;
        var response = new ErrorResponseDto();
        switch (ex)
        {
            case KeyNotFoundException:
                logLevel = LogLevel.Warning;
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = ex.Message;
                break;
            case ValidationException:
                logLevel = LogLevel.Warning;
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "Error de validación.";
                response.Details = ((ValidationException)ex).Errors.Select(x => new ErrorModel { Code = x.ErrorCode, Message = x.ErrorMessage, PropertyName = x.PropertyName });
                break;
            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Message = "Ocurrió un error inesperado.";
                break;
        }
        _logger.Log(logLevel, ex, "{Message}", ex.Message);

        await context.Response.WriteAsJsonAsync(response);
    }

}
