using System.ComponentModel.DataAnnotations;

namespace TaskSystem_Back.DTOs.Project;

public class UpdateProjectDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Detail { get; set; }
    [Required]
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    [Required]
    public Guid ProjectTypeId { get; set; }
    public Guid? ClientUserId { get; set; }
}