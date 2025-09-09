using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSMModel.Models;

public class ProjectStandard: BaseEntity
{
    public int ProjectStandardId { get; set; }
    public string Standard { get; set; }
    public string? Note { get; set; }
    public bool IsMandatory { get; set; }
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    public Project Project { get; set; }

}
