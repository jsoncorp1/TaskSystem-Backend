using TaskSystem_Back.Models.Common;

namespace TaskSystem_Back.Models;

public class DepartmentUser : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public string? Email { get; set; }
    public string? Password { get; set; }
}