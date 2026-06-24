using System.ComponentModel.DataAnnotations;

namespace TaskSystem_Back.DTOs.Department;

public class CreateDepartmentDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}