using System.ComponentModel.DataAnnotations;

namespace TaskSystem_Back.DTOs.SubProject;

public class UpdateSubProjectDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Detail { get; set; }
    [Required]
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    [Required]
    public string Status { get; set; } = string.Empty;
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public Guid DepartmentId { get; set; }
    public Guid? AssignedUserId { get; set; }
}