using TaskSystem_Back.Models.Common;

namespace TaskSystem_Back.Models;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}