using System.ComponentModel.DataAnnotations;

namespace TaskSystem_Back.DTOs.Role;

public class CreateRoleDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}