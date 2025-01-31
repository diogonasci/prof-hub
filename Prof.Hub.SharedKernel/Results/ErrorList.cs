namespace Prof.Hub.SharedKernel.Results
{
    /// <summary>
    /// Uma classe wrapper para uma lista de mensagens de erro e um CorrelationId opcional.
    /// </summary>
    /// <param name="ErrorMessages"></param>
    /// <param name="CorrelationId"></param>
    public record ErrorList(IEnumerable<string> ErrorMessages, string? CorrelationId = null);
}
