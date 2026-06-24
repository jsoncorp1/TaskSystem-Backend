using Microsoft.EntityFrameworkCore;
using TaskSystem_Back.Data;
using TaskSystem_Back.DTOs.Common;
using TaskSystem_Back.DTOs.SubTask;

namespace TaskSystem_Back.Services;

public class SubTaskService
{
    private readonly AppDbContext _db;

    public SubTaskService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResultDto<SubTaskDto>> GetAllAsync(
        int page, int pageSize,
        string? titulo, Guid? taskItemId,
        string? status, Guid? assignedUserId)
    {
        var query = _db.SubTasks
            .Include(st => st.TaskItem)
            .Include(st => st.AssignedUser)
            .Where(st => st.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(titulo))
            query = query.Where(st => st.Title.ToLower().Contains(titulo.ToLower()));

        if (taskItemId.HasValue)
            query = query.Where(st => st.TaskItemId == taskItemId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(st => st.Status == status);

        if (assignedUserId.HasValue)
            query = query.Where(st => st.AssignedUserId == assignedUserId.Value);

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(st => new SubTaskDto
            {
                Id = st.Id,
                Title = st.Title,
                Detail = st.Detail,
                StartDate = st.StartDate,
                EndDate = st.EndDate,
                Status = st.Status,
                TaskItemId = st.TaskItemId,
                TaskItemTitle = st.TaskItem.Title,
                AssignedUserId = st.AssignedUserId,
                AssignedUserFirstName = st.AssignedUser != null ? st.AssignedUser.FirstName : null,
                AssignedUserLastName = st.AssignedUser != null ? st.AssignedUser.LastName : null
            })
            .ToListAsync();

        return new PagedResultDto<SubTaskDto>
        {
            TotalItems = total,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<SubTaskDto?> GetByIdAsync(Guid id)
    {
        var st = await _db.SubTasks
            .Include(s => s.TaskItem)
            .Include(s => s.AssignedUser)
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (st == null) return null;

        return new SubTaskDto
        {
            Id = st.Id,
            Title = st.Title,
            Detail = st.Detail,
            StartDate = st.StartDate,
            EndDate = st.EndDate,
            Status = st.Status,
            TaskItemId = st.TaskItemId,
            TaskItemTitle = st.TaskItem.Title,
            AssignedUserId = st.AssignedUserId,
            AssignedUserFirstName = st.AssignedUser?.FirstName,
            AssignedUserLastName = st.AssignedUser?.LastName
        };
    }

    public async Task<SubTaskDto> CreateAsync(CreateSubTaskDto dto)
    {
        var st = new Models.SubTask
        {
            Title = dto.Title,
            Detail = dto.Detail,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status,
            TaskItemId = dto.TaskItemId,
            AssignedUserId = dto.AssignedUserId
        };

        _db.SubTasks.Add(st);
        await _db.SaveChangesAsync();

        await _db.Entry(st).Reference(s => s.TaskItem).LoadAsync();
        await _db.Entry(st).Reference(s => s.AssignedUser).LoadAsync();

        return new SubTaskDto
        {
            Id = st.Id,
            Title = st.Title,
            Detail = st.Detail,
            StartDate = st.StartDate,
            EndDate = st.EndDate,
            Status = st.Status,
            TaskItemId = st.TaskItemId,
            TaskItemTitle = st.TaskItem.Title,
            AssignedUserId = st.AssignedUserId,
            AssignedUserFirstName = st.AssignedUser?.FirstName,
            AssignedUserLastName = st.AssignedUser?.LastName
        };
    }

    public async Task<SubTaskDto?> UpdateAsync(Guid id, UpdateSubTaskDto dto)
    {
        var st = await _db.SubTasks
            .Include(s => s.TaskItem)
            .Include(s => s.AssignedUser)
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (st == null) return null;

        st.Title = dto.Title;
        st.Detail = dto.Detail;
        st.StartDate = dto.StartDate;
        st.EndDate = dto.EndDate;
        st.Status = dto.Status;
        st.TaskItemId = dto.TaskItemId;
        st.AssignedUserId = dto.AssignedUserId;
        st.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        await _db.Entry(st).Reference(s => s.TaskItem).LoadAsync();
        await _db.Entry(st).Reference(s => s.AssignedUser).LoadAsync();

        return new SubTaskDto
        {
            Id = st.Id,
            Title = st.Title,
            Detail = st.Detail,
            StartDate = st.StartDate,
            EndDate = st.EndDate,
            Status = st.Status,
            TaskItemId = st.TaskItemId,
            TaskItemTitle = st.TaskItem.Title,
            AssignedUserId = st.AssignedUserId,
            AssignedUserFirstName = st.AssignedUser?.FirstName,
            AssignedUserLastName = st.AssignedUser?.LastName
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var st = await _db.SubTasks
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (st == null) return false;

        st.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return true;
    }
}