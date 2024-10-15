using System.Diagnostics;
using MediatR;
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

            if (_logger.IsEnabled(Serilog.Events.LogEventLevel.Information))
            {
                _logger.Information("Iniciando requisição {RequestName} com dados {@Request}", requestName, request);
            }

            var stopwatch = Stopwatch.StartNew();

            var response = await next();

            _logger.Information("Concluída a requisição {RequestName} em {ElapsedMilliseconds}ms com resposta {@Response}",
                requestName, stopwatch.ElapsedMilliseconds, response);

            stopwatch.Stop();

            return response;
        }
    }
}
