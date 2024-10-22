using System.Net;
using System.Text.Json;

namespace Prof.Hub.WebApi
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, IWebHostEnvironment env)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var request = httpContext.Request;
                var requestId = httpContext.TraceIdentifier;

                var logInfo = new
                {
                    RequestId = requestId,
                    HttpMethod = request.Method,
                    RequestPath = request.Path,
                    QueryString = request.QueryString.ToString(),
                    RemoteIp = httpContext.Connection.RemoteIpAddress?.ToString(),
                    ExceptionMessage = ex.Message
                };

                _logger.LogError(ex, "Ocorreu um erro inesperado. Informações da requisição: {@LogInfo}", logInfo);

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    StatusCode = httpContext.Response.StatusCode,
                    Message = "Ocorreu um erro ao processar sua requisição.",
                    RequestId = requestId,
                };

                var jsonResponse = JsonSerializer.Serialize(response);
                await httpContext.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
