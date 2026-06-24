using Microsoft.EntityFrameworkCore;
using TaskSystem_Back.Data;
using TaskSystem_Back.DTOs.Common;
using TaskSystem_Back.DTOs.DepartmentUser;

namespace TaskSystem_Back.Services;

public class DepartmentUserService(AppDbContext db)
{
    public async Task<PagedResultDto<DepartmentUserDto>> GetAllAsync(
        int page, int pageSize,
        Guid? userId, Guid? departmentId)
    {
        var query = db.DepartmentUsers
            .Include(du => du.User)
            .Include(du => du.Department)
            .Where(du => du.DeletedAt == null);

        if (userId.HasValue)
            query = query.Where(du => du.UserId == userId.Value);

        if (departmentId.HasValue)
            query = query.Where(du => du.DepartmentId == departmentId.Value);

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(du => new DepartmentUserDto
            {
                Id = du.Id,
                UserId = du.UserId,
                UserFirstName = du.User.FirstName,
                UserLastName = du.User.LastName,
                DepartmentId = du.DepartmentId,
                DepartmentName = du.Department.Name
            })
            .ToListAsync();

        return new PagedResultDto<DepartmentUserDto>
        {
            TotalItems = total,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<DepartmentUserDto?> GetByIdAsync(Guid id)
    {
        var du = await db.DepartmentUsers
            .Include(du => du.User)
            .Include(du => du.Department)
            .FirstOrDefaultAsync(du => du.Id == id && du.DeletedAt == null);

        if (du == null) return null;

        return new DepartmentUserDto
        {
            Id = du.Id,
            UserId = du.UserId,
            UserFirstName = du.User.FirstName,
            UserLastName = du.User.LastName,
            DepartmentId = du.DepartmentId,
            DepartmentName = du.Department.Name
        };
    }

    public async Task<(DepartmentUserDto? result, bool conflict)> CreateAsync(CreateDepartmentUserDto dto)
    {
        var exists = await db.DepartmentUsers
            .AnyAsync(du => du.UserId == dto.UserId && du.DepartmentId == dto.DepartmentId && du.DeletedAt == null);

        if (exists) return (null, true);

        var du = new Models.DepartmentUser
        {
            UserId = dto.UserId,
            DepartmentId = dto.DepartmentId
        };

        db.DepartmentUsers.Add(du);
        await db.SaveChangesAsync();

        await db.Entry(du).Reference(d => d.User).LoadAsync();
        await db.Entry(du).Reference(d => d.Department).LoadAsync();

        return (new DepartmentUserDto
        {
            Id = du.Id,
            UserId = du.UserId,
            UserFirstName = du.User.FirstName,
            UserLastName = du.User.LastName,
            DepartmentId = du.DepartmentId,
            DepartmentName = du.Department.Name
        }, false);
    }

    public async Task<DepartmentUserDto?> UpdateAsync(Guid id, UpdateDepartmentUserDto dto)
    {
        var du = await db.DepartmentUsers
            .Include(d => d.User)
            .Include(d => d.Department)
            .FirstOrDefaultAsync(d => d.Id == id && d.DeletedAt == null);

        if (du == null) return null;

        du.UserId = dto.UserId;
        du.DepartmentId = dto.DepartmentId;
        du.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();

        await db.Entry(du).Reference(d => d.User).LoadAsync();
        await db.Entry(du).Reference(d => d.Department).LoadAsync();

        return new DepartmentUserDto
        {
            Id = du.Id,
            UserId = du.UserId,
            UserFirstName = du.User.FirstName,
            UserLastName = du.User.LastName,
            DepartmentId = du.DepartmentId,
            DepartmentName = du.Department.Name
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var du = await db.DepartmentUsers
            .FirstOrDefaultAsync(d => d.Id == id && d.DeletedAt == null);

        if (du == null) return false;

        du.DeletedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();

        return true;
    }
}