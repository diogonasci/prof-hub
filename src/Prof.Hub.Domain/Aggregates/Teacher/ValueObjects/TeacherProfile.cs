using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
public sealed record TeacherProfile
{
    public string Name { get; }
    public string Bio { get; }
    public string AvatarUrl { get; }

    private TeacherProfile(string name, string bio, string avatarUrl)
    {
        Name = name;
        Bio = bio;
        AvatarUrl = avatarUrl;
    }

    public static Result<TeacherProfile> Create(string name, string bio, string avatarUrl)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(new("Nome deve ser informado."));

        if (string.IsNullOrWhiteSpace(bio))
            errors.Add(new("Descrição deve ser informada."));

        if (bio?.Length > 1000)
            errors.Add(new("Descrição não pode exceder 1000 caracteres."));

        if (errors.Any())
            return Result<TeacherProfile>.Invalid(errors);

        return Result<TeacherProfile>.Success(new TeacherProfile(name, bio, avatarUrl));
    }

    public Result<TeacherProfile> UpdateBio(string newBio)
    {
        if (string.IsNullOrWhiteSpace(newBio) || newBio.Length > 1000)
            return this;

        return new TeacherProfile(Name, newBio, AvatarUrl);
    }

    public Result<TeacherProfile> UpdateAvatar(string newAvatarUrl)
    {
        return new TeacherProfile(Name, Bio, newAvatarUrl);
    }
}
