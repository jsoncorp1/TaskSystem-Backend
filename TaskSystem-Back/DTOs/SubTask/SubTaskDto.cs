namespace TaskSystem_Back.DTOs.SubTask;

public class SubTaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Detail { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;

    public Guid TaskItemId { get; set; }
    public string TaskItemTitle { get; set; } = string.Empty;

    public Guid? AssignedUserId { get; set; }
    public string? AssignedUserFirstName { get; set; }
    public string? AssignedUserLastName { get; set; }
}