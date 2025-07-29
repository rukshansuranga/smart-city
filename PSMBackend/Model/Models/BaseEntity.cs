using System;
using NodaTime;

namespace PSMModel.Models;

public class BaseEntity
{
    public LocalDateTime CreatedAt { get; set; }
    public LocalDateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;
}
