using System;
using PSMModel.Enums;

namespace PSMWebAPI.DTOs;

public class ComplainRequest
{
    public string Subject { get; set; }
    public string Detail { get; set; }
    public ComplainStatus? Status { get; set; }
}
