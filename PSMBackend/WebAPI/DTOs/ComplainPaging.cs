using System;

namespace PSMWebAPI.DTOs;

public class  ComplainPaging
{
    public string? Status { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int Duration { get; set; }
    public string? Type { get; set; }
}
