﻿using System.Text.Json.Serialization;

namespace Prof.Hub.Application.Results
{
    public class PagedResult<T>(PagedInfo pagedInfo, T value) : Result<T>(value)
    {
        [JsonInclude]
        public PagedInfo PagedInfo { get; init; } = pagedInfo;
    }
}
