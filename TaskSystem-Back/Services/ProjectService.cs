using Microsoft.EntityFrameworkCore;
using TaskSystem_Back.Data;
using TaskSystem_Back.DTOs.Common;
using TaskSystem_Back.DTOs.Project;

namespace TaskSystem_Back.Services;

public class ProjectService(AppDbContext db)
{
    public async Task<PagedResultDto<ProjectDto>> GetAllAsync(
        int page, int pageSize,
        string? titulo, Guid? projectTypeId, Guid? clientUserId,
        DateOnly? startDateFrom, DateOnly? startDateTo)
    {
        var query = db.Projects
            .Include(p => p.ProjectType)
            .Include(p => p.ClientUser)
            .Where(p => p.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(titulo))
            query = query.Where(p => p.Title.ToLower().Contains(titulo.ToLower()));

        if (projectTypeId.HasValue)
            query = query.Where(p => p.ProjectTypeId == projectTypeId.Value);

        if (clientUserId.HasValue)
            query = query.Where(p => p.ClientUserId == clientUserId.Value);

        if (startDateFrom.HasValue)
            query = query.Where(p => p.StartDate >= startDateFrom.Value);

        if (startDateTo.HasValue)
            query = query.Where(p => p.StartDate <= startDateTo.Value);

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Detail = p.Detail,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ProjectTypeId = p.ProjectTypeId,
                ProjectTypeName = p.ProjectType.Name,
                ClientUserId = p.ClientUserId,
                ClientUserFirstName = p.ClientUser != null ? p.ClientUser.FirstName : null,
                ClientUserLastName = p.ClientUser != null ? p.ClientUser.LastName : null
            })
            .ToListAsync();

        return new PagedResultDto<ProjectDto>
        {
            TotalItems = total,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<ProjectDto?> GetByIdAsync(Guid id)
    {
        var project = await db.Projects
            .Include(p => p.ProjectType)
            .Include(p => p.ClientUser)
            .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);

        if (project == null) return null;

        return new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Detail = project.Detail,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            ProjectTypeId = project.ProjectTypeId,
            ProjectTypeName = project.ProjectType.Name,
            ClientUserId = project.ClientUserId,
            ClientUserFirstName = project.ClientUser?.FirstName,
            ClientUserLastName = project.ClientUser?.LastName
        };
    }

    public async Task<ProjectFullDto?> GetFullAsync(Guid id)
    {
        var project = await db.Projects
            .Include(p => p.ProjectType)
            .Include(p => p.ClientUser)
            .Include(p => p.SubProjects.Where(sp => sp.DeletedAt == null))
                .ThenInclude(sp => sp.Department)
            .Include(p => p.SubProjects.Where(sp => sp.DeletedAt == null))
                .ThenInclude(sp => sp.AssignedUser)
            .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);

        if (project == null) return null;

        return new ProjectFullDto
        {
            Id = project.Id,
            Title = project.Title,
            Detail = project.Detail,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            ProjectTypeId = project.ProjectTypeId,
            ProjectTypeName = project.ProjectType.Name,
            ClientUserId = project.ClientUserId,
            ClientUserFirstName = project.ClientUser?.FirstName,
            ClientUserLastName = project.ClientUser?.LastName,
            SubProjects = project.SubProjects.Select(sp => new SubProjectSummaryDto
            {
                Id = sp.Id,
                Title = sp.Title,
                Detail = sp.Detail,
                StartDate = sp.StartDate,
                EndDate = sp.EndDate,
                Status = sp.Status,
                DepartmentName = sp.Department.Name,
                AssignedUserFirstName = sp.AssignedUser?.FirstName,
                AssignedUserLastName = sp.AssignedUser?.LastName
            }).ToList()
        };
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
    {
        var project = new Models.Project
        {
            Title = dto.Title,
            Detail = dto.Detail,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            ProjectTypeId = dto.ProjectTypeId,
            ClientUserId = dto.ClientUserId
        };

        db.Projects.Add(project);
        await db.SaveChangesAsync();

        await db.Entry(project).Reference(p => p.ProjectType).LoadAsync();
        await db.Entry(project).Reference(p => p.ClientUser).LoadAsync();

        return new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Detail = project.Detail,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            ProjectTypeId = project.ProjectTypeId,
            ProjectTypeName = project.ProjectType.Name,
            ClientUserId = project.ClientUserId,
            ClientUserFirstName = project.ClientUser?.FirstName,
            ClientUserLastName = project.ClientUser?.LastName
        };
    }

    public async Task<ProjectDto?> UpdateAsync(Guid id, UpdateProjectDto dto)
    {
        var project = await db.Projects
            .Include(p => p.ProjectType)
            .Include(p => p.ClientUser)
            .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);

        if (project == null) return null;

        project.Title = dto.Title;
        project.Detail = dto.Detail;
        project.StartDate = dto.StartDate;
        project.EndDate = dto.EndDate;
        project.ProjectTypeId = dto.ProjectTypeId;
        project.ClientUserId = dto.ClientUserId;
        project.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();

        await db.Entry(project).Reference(p => p.ProjectType).LoadAsync();
        await db.Entry(project).Reference(p => p.ClientUser).LoadAsync();

        return new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Detail = project.Detail,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            ProjectTypeId = project.ProjectTypeId,
            ProjectTypeName = project.ProjectType.Name,
            ClientUserId = project.ClientUserId,
            ClientUserFirstName = project.ClientUser?.FirstName,
            ClientUserLastName = project.ClientUser?.LastName
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var project = await db.Projects
            .FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == null);

        if (project == null) return false;

        project.DeletedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();

        return true;
    }
}