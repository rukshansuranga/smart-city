using System;
using NodaTime;

namespace PSMWebAPI.DTOs.Project;

public class ProjectPaging
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string? SelectedType { get; set; }
    public string? SearchText { get; set; }
    public LocalDate? StartDate { get; set; }
    public LocalDate? EndDate { get; set; }

}
