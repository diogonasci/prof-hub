namespace Prof.Hub.SharedKernel;
public sealed class ConcurrencyException(string message, Exception innerException) : Exception(message, innerException)
{
}
