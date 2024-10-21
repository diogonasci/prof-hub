using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Student.ValueObjects;
public sealed record ClassHours(int Value)
{
    public static Result<ClassHours> Create(int value)
    {
        if (value < 0)
            return Result.Invalid(new ValidationError("A quantidade de horas disponíveis não pode ser negativa."));

        return Result.Success(new ClassHours(value));
    }

    public Result<ClassHours> Add(int hours)
    {
        if (hours <= 0)
            return Result.Invalid(new ValidationError("Não é possível adicionar horas zero ou negativas."));

        return Result.Success(new ClassHours(Value + hours));
    }

    public Result<ClassHours> Subtract(int hours)
    {
        if (hours <= 0)
            return Result.Invalid(new ValidationError("Não é possível subtrair horas zero ou negativas."));

        if (Value - hours < 0)
            return Result.Invalid(new ValidationError("Horas disponíveis insuficientes."));

        return Result.Success(new ClassHours(Value - hours));
    }
}
