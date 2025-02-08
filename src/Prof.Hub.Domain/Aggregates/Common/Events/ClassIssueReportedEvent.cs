using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Events;
public record ClassIssueReportedEvent(
    string ClassId,
    string Description,
    string IssueType,
    DateTime ReportedAt) : IDomainEvent;
