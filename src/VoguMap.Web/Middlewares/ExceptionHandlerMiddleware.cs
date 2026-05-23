using System.Net;
using System.Text.Json;
using VoguMap.Domain.Exceptions.Domain;
using VoguMap.Domain.Exceptions.Infrastructure;

namespace VoguMap.Web.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        // Общий делегат для Warning
        private static readonly Action<ILogger, string, string, Exception?> _logWarning =
            LoggerMessage.Define<string, string>(
                LogLevel.Warning,
                new EventId(1, "Warning"),
                "{ErrorType}: {Message}");

        // Общий делегат для Error
        private static readonly Action<ILogger, string, Exception?> _logError =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId(2, "Error"),
                "Необработанное исключение: {Message}");

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse();

            switch (exception)
            {
                case NotFoundException notFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = notFoundException.Message;
                    response.ErrorStringCode = notFoundException.ErrorCode;
                    response.ErrorCode = notFoundException.HttpStatusCode;
                    _logWarning(_logger, "NOT_FOUND", notFoundException.Message, notFoundException);
                    break;

                case ValidationException validationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = validationException.Message;
                    response.ErrorStringCode = validationException.ErrorCode;
                    response.ErrorCode = validationException.HttpStatusCode;
                    _logWarning(_logger, "VALIDATION_ERROR", validationException.Message, validationException);
                    break;

                case DataConsistencyException dataConsistencyException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = dataConsistencyException.Message;
                    response.ErrorStringCode = dataConsistencyException.ErrorCode;
                    response.ErrorCode = dataConsistencyException.HttpStatusCode;
                    _logWarning(_logger, "VALIDATION_ERROR", dataConsistencyException.Message, dataConsistencyException);
                    break;

                case ArgumentNullException argumentNullException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = argumentNullException.Message;
                    response.ErrorStringCode = "ARGUMENT_NULL";
                    response.ErrorCode = 400;
                    _logWarning(_logger, "ARGUMENT_NULL", argumentNullException.Message, argumentNullException);
                    break;

                case InvalidOperationException invalidOperationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = invalidOperationException.Message;
                    response.ErrorStringCode = "INVALID_OPERATION";
                    response.ErrorCode = 400;
                    _logWarning(_logger, "INVALID_OPERATION", invalidOperationException.Message, invalidOperationException);
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "Произошла внутренняя ошибка сервера";
                    response.ErrorStringCode = "INTERNAL_SERVER_ERROR";
                    response.ErrorCode = 500;
                    _logError(_logger, exception.Message, exception);
                    break;
            }

            var result = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(result);
        }

        private class ErrorResponse
        {
            public string Message { get; set; } = string.Empty;
            public string ErrorStringCode { get; set; } = string.Empty;
            public int ErrorCode { get; set; }
        }
    }
}