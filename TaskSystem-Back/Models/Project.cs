using TaskSystem_Back.Models.Common;

namespace TaskSystem_Back.Models;

public class Project : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Detail { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    // Tipo de proyecto: obligatorio
    public Guid ProjectTypeId { get; set; }
    public ProjectType ProjectType { get; set; } = null!;

    // Usuario cliente que solicita: opcional
    public Guid? ClientUserId { get; set; }
    public User? ClientUser { get; set; }
}