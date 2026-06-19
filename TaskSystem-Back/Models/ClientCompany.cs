using TaskSystem_Back.Models.Common;

namespace TaskSystem_Back.Models;

public class ClientCompany : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}