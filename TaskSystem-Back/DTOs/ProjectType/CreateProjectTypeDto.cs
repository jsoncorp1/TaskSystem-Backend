using System.ComponentModel.DataAnnotations;

namespace TaskSystem_Back.DTOs.ProjectType;

public class CreateProjectTypeDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}