using System.ComponentModel.DataAnnotations;

namespace TaskSystem_Back.DTOs.User;

public class UpdateUserDto
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    [Required]
    public Guid RoleId { get; set; }
    public Guid? ClientCompanyId { get; set; }
}