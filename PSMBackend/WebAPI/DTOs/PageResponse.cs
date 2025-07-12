using System;

namespace PSMWebAPI.DTOs;

public class PageResponse<T>
{
    public int TotalItems { get; set; }
    public IEnumerable<T> Records { get; set; }
}
