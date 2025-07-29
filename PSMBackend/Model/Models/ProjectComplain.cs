using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSMModel.Models;

public class ProjectComplain : Workpackage
{
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    public Project Project { get; set; }
}
