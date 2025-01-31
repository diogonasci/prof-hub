using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Teacher.ValueObjects
{
    public sealed record HourlyRate(decimal Value)
    {
        public static Result<HourlyRate> Create(decimal value)
        {
            if (value <= 0)
                return Result.Invalid(new ValidationError("O valor da hora deve ser maior que zero."));

            return Result.Success(new HourlyRate(value));
        }

        public Result<HourlyRate> Increase(decimal amount)
        {
            return new HourlyRate(Value + amount);
        }

        public Result<HourlyRate> Decrease(decimal amount)
        {
            if (amount >= Value)
                return Result.Invalid(new ValidationError("A diminuição não pode ser maior ou igual ao valor atual."));

            return new HourlyRate(Value - amount);
        }

        public override string ToString() => $"R$ {Value:F2}";
    }
}
