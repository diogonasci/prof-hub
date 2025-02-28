using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
public record ClassNote(string Content, DateTime CreatedAt)
{
    public static Result<ClassNote> Create(string content, DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Conteúdo da nota não pode estar vazio.");

        return new ClassNote(content, createdAt);
    }
}
