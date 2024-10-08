using MediatR;
using System.Diagnostics;
using ILogger = Serilog.ILogger;

namespace Prof.Hub.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var stopwatch = Stopwatch.StartNew();

            _logger.Information("Iniciando requisição {RequestName} com dados {@Request}", requestName, request);

            var response = await next();

            stopwatch.Stop();

            _logger.Information("Concluída a requisição {RequestName} em {ElapsedMilliseconds}ms com resposta {@Response}",
                requestName, stopwatch.ElapsedMilliseconds, response);

            return response;
        }
    }
}
