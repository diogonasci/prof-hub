using Prof.Hub.Domain.Enums;

namespace Prof.Hub.Domain.Aggregates.Common.ValueObjects;
public class ClassIssue
{
    public string Description { get; }
    public ClassIssueType Type { get; }
    public DateTime ReportedAt { get; }
    public bool IsResolved { get; private set; }
    public DateTime? ResolvedAt { get; private set; }
    public string? ResolutionNotes { get; private set; }

    public ClassIssue(string description, ClassIssueType type, DateTime reportedAt)
    {
        Description = description;
        Type = type;
        ReportedAt = reportedAt;
        IsResolved = false;
    }

    public void Resolve(string notes, DateTime resolvedAt)
    {
        IsResolved = true;
        ResolvedAt = resolvedAt;
        ResolutionNotes = notes;
    }
}
