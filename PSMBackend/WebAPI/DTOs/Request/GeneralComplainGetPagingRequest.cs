using System;

namespace PSMWebAPI.DTOs.Request;

public class GeneralComplainGetPagingRequest
{
    public bool IsPrivate { get; set; }
    public int PageNumber { get; set; }
}
