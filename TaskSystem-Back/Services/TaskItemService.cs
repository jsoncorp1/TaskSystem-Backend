using Microsoft.EntityFrameworkCore;
using TaskSystem_Back.Data;
using TaskSystem_Back.DTOs.Common;
using TaskSystem_Back.DTOs.TaskItem;

namespace TaskSystem_Back.Services;

public class TaskItemService
{
    private readonly AppDbContext _db;

    public TaskItemService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResultDto<TaskItemDto>> GetAllAsync(
        int page, int pageSize,
        string? titulo, Guid? subProjectId,
        string? status, Guid? assignedUserId)
    {
        var query = _db.TaskItems
            .Include(t => t.SubProject)
            .Include(t => t.AssignedUser)
            .Where(t => t.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(titulo))
            query = query.Where(t => t.Title.ToLower().Contains(titulo.ToLower()));

        if (subProjectId.HasValue)
            query = query.Where(t => t.SubProjectId == subProjectId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(t => t.Status == status);

        if (assignedUserId.HasValue)
            query = query.Where(t => t.AssignedUserId == assignedUserId.Value);

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TaskItemDto
            {
                Id = t.Id,
                Title = t.Title,
                Detail = t.Detail,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Status = t.Status,
                SubProjectId = t.SubProjectId,
                SubProjectTitle = t.SubProject.Title,
                AssignedUserId = t.AssignedUserId,
                AssignedUserFirstName = t.AssignedUser != null ? t.AssignedUser.FirstName : null,
                AssignedUserLastName = t.AssignedUser != null ? t.AssignedUser.LastName : null
            })
            .ToListAsync();

        return new PagedResultDto<TaskItemDto>
        {
            TotalItems = total,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<TaskItemDto?> GetByIdAsync(Guid id)
    {
        var task = await _db.TaskItems
            .Include(t => t.SubProject)
            .Include(t => t.AssignedUser)
            .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null);

        if (task == null) return null;

        return new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Detail = task.Detail,
            StartDate = task.StartDate,
            EndDate = task.EndDate,
            Status = task.Status,
            SubProjectId = task.SubProjectId,
            SubProjectTitle = task.SubProject.Title,
            AssignedUserId = task.AssignedUserId,
            AssignedUserFirstName = task.AssignedUser?.FirstName,
            AssignedUserLastName = task.AssignedUser?.LastName
        };
    }

    public async Task<TaskItemDto> CreateAsync(CreateTaskItemDto dto)
    {
        var task = new Models.TaskItem
        {
            Title = dto.Title,
            Detail = dto.Detail,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status,
            SubProjectId = dto.SubProjectId,
            AssignedUserId = dto.AssignedUserId
        };

        _db.TaskItems.Add(task);
        await _db.SaveChangesAsync();

        await _db.Entry(task).Reference(t => t.SubProject).LoadAsync();
        await _db.Entry(task).Reference(t => t.AssignedUser).LoadAsync();

        return new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Detail = task.Detail,
            StartDate = task.StartDate,
            EndDate = task.EndDate,
            Status = task.Status,
            SubProjectId = task.SubProjectId,
            SubProjectTitle = task.SubProject.Title,
            AssignedUserId = task.AssignedUserId,
            AssignedUserFirstName = task.AssignedUser?.FirstName,
            AssignedUserLastName = task.AssignedUser?.LastName
        };
    }

    public async Task<TaskItemDto?> UpdateAsync(Guid id, UpdateTaskItemDto dto)
    {
        var task = await _db.TaskItems
            .Include(t => t.SubProject)
            .Include(t => t.AssignedUser)
            .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null);

        if (task == null) return null;

        task.Title = dto.Title;
        task.Detail = dto.Detail;
        task.StartDate = dto.StartDate;
        task.EndDate = dto.EndDate;
        task.Status = dto.Status;
        task.SubProjectId = dto.SubProjectId;
        task.AssignedUserId = dto.AssignedUserId;
        task.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        await _db.Entry(task).Reference(t => t.SubProject).LoadAsync();
        await _db.Entry(task).Reference(t => t.AssignedUser).LoadAsync();

        return new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Detail = task.Detail,
            StartDate = task.StartDate,
            EndDate = task.EndDate,
            Status = task.Status,
            SubProjectId = task.SubProjectId,
            SubProjectTitle = task.SubProject.Title,
            AssignedUserId = task.AssignedUserId,
            AssignedUserFirstName = task.AssignedUser?.FirstName,
            AssignedUserLastName = task.AssignedUser?.LastName
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var task = await _db.TaskItems
            .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null);

        if (task == null) return false;

        task.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return true;
    }
}