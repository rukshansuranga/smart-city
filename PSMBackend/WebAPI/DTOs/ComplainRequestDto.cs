using System;

namespace PSMBackend.DTOs;

public class ComplainRequestDto
{
    public int? Id { get; set; }
    public string LightPostNumber { get; set; }
    public int UserId { get; set; }
    public DateTime? ComplainDate { get; set; }
    public DateTime? ResolvedDate { get; set; }
    public string Name { get; set; }
    public string? Comment { get; set; }
    public string? Status { get; set; }
}
