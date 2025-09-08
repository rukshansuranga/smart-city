using System;

namespace PSMWebAPI.DTOs;

public class PagingRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
