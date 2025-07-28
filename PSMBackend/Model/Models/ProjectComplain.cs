using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSMModel.Models;

public class ProjectComplain : WorkPackage
{
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    public Project Project { get; set; }
}
