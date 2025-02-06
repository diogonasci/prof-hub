using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Events;
public record ClassIssueReportedEvent(string ClassId, ClassIssue Issue) : IDomainEvent;
