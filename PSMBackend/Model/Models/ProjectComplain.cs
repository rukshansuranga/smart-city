using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSMModel.Models;

public class ProjectComplain : Complain
{
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    public Project Project { get; set; }
}
