﻿namespace Prof.Hub.SharedKernel.Results
{
    public interface IResult
    {
        ResultStatus Status { get; }
        IEnumerable<string> Errors { get; }
        IEnumerable<ValidationError> ValidationErrors { get; }
        Type ValueType { get; }
        object GetValue();
        string Location { get; }
    }
}
