using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace PSMModel.Models;

public class WorkPackage
{
    [Key]
    public int WorkPackageId { get; set; }
    public string Name { get; set; }
    public string Detail { get; set; }
    public LocalDateTime CreatedDate { get; set; }
    public LocalDateTime UpdatedDate { get; set; }
    public string? Status { get; set; }
    [ForeignKey("Client")]
    public int? ClientId { get; set; }
    public Client? Client { get; set; }
    public string WorkPackageType { get; set; }

    public List<TicketPackage>? TicketPackages { get; set; }
    
    public List<Comment>? Comments { get; set; }
}
