using System.ComponentModel.DataAnnotations;

namespace TaskSystem_Back.DTOs.DepartmentUser;

public class CreateDepartmentUserDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid DepartmentId { get; set; }
}