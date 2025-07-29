using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PSMModel.Models;

public class GCShedule: BaseEntity
{
    public int GCSheduleId { get; set; }
    public string Day { get; set; }
    public string Type { get; set; }
    public string Time { get; set; }
    [ForeignKey("Region")]
    public string RegionNo { get; set; }
    public Region Region { get; set; }
}
