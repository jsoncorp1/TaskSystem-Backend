using System.ComponentModel.DataAnnotations;

namespace TaskSystem_Back.DTOs.DepartmentUser;

public class UpdateDepartmentUserDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid DepartmentId { get; set; }
}