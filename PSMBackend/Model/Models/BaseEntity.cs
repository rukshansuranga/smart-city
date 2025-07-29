using System;
using NodaTime;

namespace PSMModel.Models;

public class BaseEntity
{
    public LocalDateTime CreatedDate { get; set; }
    public LocalDateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;
}
