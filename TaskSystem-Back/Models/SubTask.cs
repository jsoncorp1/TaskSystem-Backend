using TaskSystem_Back.Models.Common;

namespace TaskSystem_Back.Models;

public class SubTask : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Detail { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public string Status { get; set; } = "Pendiente";

    // Tarea a la que pertenece: obligatorio
    public Guid TaskItemId { get; set; }
    public TaskItem TaskItem { get; set; } = null!;

    // Usuario responsable: opcional
    public Guid? AssignedUserId { get; set; }
    public User? AssignedUser { get; set; }
}