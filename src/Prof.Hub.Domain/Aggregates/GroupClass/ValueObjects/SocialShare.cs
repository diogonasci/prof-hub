using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
public record SocialShare
{
    public StudentId SharedBy { get; }
    public SocialNetwork Network { get; }
    public DateTime SharedAt { get; }
    public Uri ShareUrl { get; }

    private SocialShare(StudentId sharedBy, SocialNetwork network, Uri shareUrl)
    {
        SharedBy = sharedBy;
        Network = network;
        SharedAt = DateTime.UtcNow;
        ShareUrl = shareUrl;
    }

    public static Result<SocialShare> Create(StudentId sharedBy, SocialNetwork network, Uri shareUrl)
    {
        if (shareUrl == null || !shareUrl.IsAbsoluteUri)
            return Result.Invalid(new ValidationError("URL de compartilhamento inválida"));

        return new SocialShare(sharedBy, network, shareUrl);
    }
}
