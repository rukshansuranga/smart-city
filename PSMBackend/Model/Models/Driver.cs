using System;
using NodaTime;

namespace PSMModel.Models;

public class Driver: User
{
    public string? LicenNo { get; set; }
    public LocalDateTime? ExpireDate { get; set; }
}
