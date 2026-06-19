using TaskSystem_Back.Models.Common;

namespace TaskSystem_Back.Models;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }

    // Rol: obligatorio
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    // Empresa cliente: opcional
    public Guid? ClientCompanyId { get; set; }
    public ClientCompany? ClientCompany { get; set; }

    public ICollection<DepartmentUser> DepartmentUsers { get; set; } = new List<DepartmentUser>();
}