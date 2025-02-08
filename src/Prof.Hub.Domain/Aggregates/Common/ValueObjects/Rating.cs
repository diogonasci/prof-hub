using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.ValueObjects;
public sealed record Rating
{
    private const int MIN_RATING = 1;
    private const int MAX_RATING = 5;
    private readonly List<int> _ratings = [];

    public decimal AverageScore => _ratings.Count != 0 ? _ratings.Select(r => (decimal)r).Average() : 0;
    public int TotalRatings => _ratings.Count;
    public int Value { get; private set; }

    private Rating()
    {
    }

    public static Rating Create()
    {
        return new Rating();
    }

    public static Result<Rating> Create(int value)
    {
        if (value < MIN_RATING || value > MAX_RATING)
            return Result.Invalid(new ValidationError($"Avaliação deve estar entre {MIN_RATING} e {MAX_RATING}"));

        var rating = new Rating
        {
            Value = value
        };

        rating._ratings.Add(value);

        return rating;
    }

    public Result AddRating(Rating rating)
    {
        if (rating == null)
            return Result.Invalid(new ValidationError("Avaliação é obrigatória"));

        if (rating.Value < MIN_RATING || rating.Value > MAX_RATING)
            return Result.Invalid(new ValidationError($"Avaliação deve estar entre {MIN_RATING} e {MAX_RATING}"));

        _ratings.Add(rating.Value);
        Value = rating.Value;

        return Result.Success();
    }

    public bool HasRatings() => _ratings.Count != 0;

    public decimal GetAverageForLastNRatings(int n)
    {
        if (n <= 0 || _ratings.Count == 0)
            return 0;

        return _ratings
            .TakeLast(Math.Min(n, _ratings.Count))
            .Select(r => (decimal)r)
            .Average();
    }
}
