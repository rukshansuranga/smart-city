using System;
using NodaTime;

namespace PSMModel.Models;

public class Driver: User
{
    public string? LicenseNo { get; set; }
    public LocalDateTime? ExpireDate { get; set; }
}
