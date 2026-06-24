using Microsoft.EntityFrameworkCore;
using TaskSystem_Back.Data;
using TaskSystem_Back.DTOs.Common;
using TaskSystem_Back.DTOs.SubProject;

namespace TaskSystem_Back.Services;

public class SubProjectService
{
    private readonly AppDbContext _db;

    public SubProjectService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResultDto<SubProjectDto>> GetAllAsync(
        int page, int pageSize,
        string? titulo, Guid? projectId, Guid? departmentId,
        string? status, Guid? assignedUserId)
    {
        var query = _db.SubProjects
            .Include(sp => sp.Project)
            .Include(sp => sp.Department)
            .Include(sp => sp.AssignedUser)
            .Where(sp => sp.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(titulo))
            query = query.Where(sp => sp.Title.ToLower().Contains(titulo.ToLower()));

        if (projectId.HasValue)
            query = query.Where(sp => sp.ProjectId == projectId.Value);

        if (departmentId.HasValue)
            query = query.Where(sp => sp.DepartmentId == departmentId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(sp => sp.Status == status);

        if (assignedUserId.HasValue)
            query = query.Where(sp => sp.AssignedUserId == assignedUserId.Value);

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(sp => new SubProjectDto
            {
                Id = sp.Id,
                Title = sp.Title,
                Detail = sp.Detail,
                StartDate = sp.StartDate,
                EndDate = sp.EndDate,
                Status = sp.Status,
                ProjectId = sp.ProjectId,
                ProjectTitle = sp.Project.Title,
                DepartmentId = sp.DepartmentId,
                DepartmentName = sp.Department.Name,
                AssignedUserId = sp.AssignedUserId,
                AssignedUserFirstName = sp.AssignedUser != null ? sp.AssignedUser.FirstName : null,
                AssignedUserLastName = sp.AssignedUser != null ? sp.AssignedUser.LastName : null
            })
            .ToListAsync();

        return new PagedResultDto<SubProjectDto>
        {
            TotalItems = total,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<SubProjectDto?> GetByIdAsync(Guid id)
    {
        var sp = await _db.SubProjects
            .Include(s => s.Project)
            .Include(s => s.Department)
            .Include(s => s.AssignedUser)
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (sp == null) return null;

        return new SubProjectDto
        {
            Id = sp.Id,
            Title = sp.Title,
            Detail = sp.Detail,
            StartDate = sp.StartDate,
            EndDate = sp.EndDate,
            Status = sp.Status,
            ProjectId = sp.ProjectId,
            ProjectTitle = sp.Project.Title,
            DepartmentId = sp.DepartmentId,
            DepartmentName = sp.Department.Name,
            AssignedUserId = sp.AssignedUserId,
            AssignedUserFirstName = sp.AssignedUser?.FirstName,
            AssignedUserLastName = sp.AssignedUser?.LastName
        };
    }

    public async Task<SubProjectDto> CreateAsync(CreateSubProjectDto dto)
    {
        var sp = new Models.SubProject
        {
            Title = dto.Title,
            Detail = dto.Detail,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status,
            ProjectId = dto.ProjectId,
            DepartmentId = dto.DepartmentId,
            AssignedUserId = dto.AssignedUserId
        };

        _db.SubProjects.Add(sp);
        await _db.SaveChangesAsync();

        await _db.Entry(sp).Reference(s => s.Project).LoadAsync();
        await _db.Entry(sp).Reference(s => s.Department).LoadAsync();
        await _db.Entry(sp).Reference(s => s.AssignedUser).LoadAsync();

        return new SubProjectDto
        {
            Id = sp.Id,
            Title = sp.Title,
            Detail = sp.Detail,
            StartDate = sp.StartDate,
            EndDate = sp.EndDate,
            Status = sp.Status,
            ProjectId = sp.ProjectId,
            ProjectTitle = sp.Project.Title,
            DepartmentId = sp.DepartmentId,
            DepartmentName = sp.Department.Name,
            AssignedUserId = sp.AssignedUserId,
            AssignedUserFirstName = sp.AssignedUser?.FirstName,
            AssignedUserLastName = sp.AssignedUser?.LastName
        };
    }

    public async Task<SubProjectDto?> UpdateAsync(Guid id, UpdateSubProjectDto dto)
    {
        var sp = await _db.SubProjects
            .Include(s => s.Project)
            .Include(s => s.Department)
            .Include(s => s.AssignedUser)
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (sp == null) return null;

        sp.Title = dto.Title;
        sp.Detail = dto.Detail;
        sp.StartDate = dto.StartDate;
        sp.EndDate = dto.EndDate;
        sp.Status = dto.Status;
        sp.ProjectId = dto.ProjectId;
        sp.DepartmentId = dto.DepartmentId;
        sp.AssignedUserId = dto.AssignedUserId;
        sp.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        await _db.Entry(sp).Reference(s => s.Project).LoadAsync();
        await _db.Entry(sp).Reference(s => s.Department).LoadAsync();
        await _db.Entry(sp).Reference(s => s.AssignedUser).LoadAsync();

        return new SubProjectDto
        {
            Id = sp.Id,
            Title = sp.Title,
            Detail = sp.Detail,
            StartDate = sp.StartDate,
            EndDate = sp.EndDate,
            Status = sp.Status,
            ProjectId = sp.ProjectId,
            ProjectTitle = sp.Project.Title,
            DepartmentId = sp.DepartmentId,
            DepartmentName = sp.Department.Name,
            AssignedUserId = sp.AssignedUserId,
            AssignedUserFirstName = sp.AssignedUser?.FirstName,
            AssignedUserLastName = sp.AssignedUser?.LastName
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var sp = await _db.SubProjects
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (sp == null) return false;

        sp.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return true;
    }
}