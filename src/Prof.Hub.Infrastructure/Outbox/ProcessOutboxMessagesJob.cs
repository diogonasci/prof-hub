using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prof.Hub.Infrastructure.PostgresSql;
using Prof.Hub.SharedKernel;
using Quartz;
using Microsoft.EntityFrameworkCore;

namespace Prof.Hub.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob : IJob
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers = { static ti => ti.PolymorphismOptions = new() { TypeDiscriminatorPropertyName = "$type" } }
        }
    };

    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly OutboxOptions _outboxOptions;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        IUnitOfWork unitOfWork,
        IPublisher publisher,
        IDateTimeProvider dateTimeProvider,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
        _outboxOptions = outboxOptions.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Iniciando o processamento das mensagens da outbox");

        var dbContext = (ApplicationDbContext)_unitOfWork;
        var transaction = await dbContext.Database.BeginTransactionAsync(context.CancellationToken);

        try
        {
            var outboxMessages = await GetOutboxMessagesAsync(dbContext, context.CancellationToken);

            foreach (var outboxMessage in outboxMessages)
            {
                Exception? exception = null;

                try
                {
                    var domainEvent = JsonSerializer.Deserialize<IDomainEvent>(
                        outboxMessage.Content,
                        JsonSerializerOptions)!;

                    await _publisher.Publish(domainEvent, context.CancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Exceção ao processar a mensagem da outbox {MessageId}",
                        outboxMessage.Id);

                    exception = ex;
                }

                await UpdateOutboxMessageAsync(dbContext, outboxMessage, exception, context.CancellationToken);
            }

            await transaction.CommitAsync(context.CancellationToken);
            _logger.LogInformation("Processamento das mensagens da outbox concluído");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao processar as mensagens da outbox");
            await transaction.RollbackAsync(context.CancellationToken);
        }
    }

    private async Task<List<OutboxMessage>> GetOutboxMessagesAsync(
        ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        return await dbContext.OutboxMessages
            .Where(msg => msg.ProcessedOnUtc == null)
            .OrderBy(msg => msg.OccurredOnUtc)
            .Take(_outboxOptions.BatchSize)
            .ToListAsync(cancellationToken);
    }

    private async Task UpdateOutboxMessageAsync(
        ApplicationDbContext dbContext, OutboxMessage outboxMessage,
        Exception? exception, CancellationToken cancellationToken)
    {
        var updatedMessage = new OutboxMessage(
            outboxMessage.Id,
            outboxMessage.OccurredOnUtc,
            outboxMessage.Type,
            outboxMessage.Content)
        {
            ProcessedOnUtc = _dateTimeProvider.UtcNow,
            Error = exception?.ToString()
        };

        dbContext.OutboxMessages.Update(updatedMessage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
