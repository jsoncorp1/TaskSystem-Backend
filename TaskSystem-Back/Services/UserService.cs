using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskSystem_Back.Data;
using TaskSystem_Back.DTOs.Common;
using TaskSystem_Back.DTOs.User;

namespace TaskSystem_Back.Services;

public class UserService(AppDbContext db, IConfiguration config)
{
    public async Task<PagedResultDto<UserDto>> GetAllAsync(
        int page, int pageSize,
        string? nombre, string? apellido, string? email,
        Guid? roleId, Guid? clientCompanyId)
    {
        var query = db.Users
            .Include(u => u.Role)
            .Include(u => u.ClientCompany)
            .Where(u => u.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(u => u.FirstName.ToLower().Contains(nombre.ToLower()));

        if (!string.IsNullOrWhiteSpace(apellido))
            query = query.Where(u => u.LastName.ToLower().Contains(apellido.ToLower()));

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(u => u.Email != null && u.Email.ToLower().Contains(email.ToLower()));

        if (roleId.HasValue)
            query = query.Where(u => u.RoleId == roleId.Value);

        if (clientCompanyId.HasValue)
            query = query.Where(u => u.ClientCompanyId == clientCompanyId.Value);

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Phone = u.Phone,
                Email = u.Email,
                RoleId = u.RoleId,
                RoleName = u.Role.Name,
                ClientCompanyId = u.ClientCompanyId,
                ClientCompanyName = u.ClientCompany != null ? u.ClientCompany.Name : null
            })
            .ToListAsync();

        return new PagedResultDto<UserDto>
        {
            TotalItems = total,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<PagedResultDto<UserDto>> GetClientsAsync(
        int page, int pageSize,
        string? nombre, string? apellido, Guid? clientCompanyId)
    {
        var clientRoleId = await db.Roles
            .Where(r => r.Name == "Cliente" && r.DeletedAt == null)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        var query = db.Users
            .Include(u => u.Role)
            .Include(u => u.ClientCompany)
            .Where(u => u.DeletedAt == null && u.RoleId == clientRoleId);

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(u => u.FirstName.ToLower().Contains(nombre.ToLower()));

        if (!string.IsNullOrWhiteSpace(apellido))
            query = query.Where(u => u.LastName.ToLower().Contains(apellido.ToLower()));

        if (clientCompanyId.HasValue)
            query = query.Where(u => u.ClientCompanyId == clientCompanyId.Value);

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Phone = u.Phone,
                Email = u.Email,
                RoleId = u.RoleId,
                RoleName = u.Role.Name,
                ClientCompanyId = u.ClientCompanyId,
                ClientCompanyName = u.ClientCompany != null ? u.ClientCompany.Name : null
            })
            .ToListAsync();

        return new PagedResultDto<UserDto>
        {
            TotalItems = total,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await db.Users
            .Include(u => u.Role)
            .Include(u => u.ClientCompany)
            .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);

        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Email = user.Email,
            RoleId = user.RoleId,
            RoleName = user.Role.Name,
            ClientCompanyId = user.ClientCompanyId,
            ClientCompanyName = user.ClientCompany?.Name
        };
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var user = new Models.User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            Email = dto.Email,
            Password = dto.Password != null ? BCrypt.Net.BCrypt.HashPassword(dto.Password) : null,
            RoleId = dto.RoleId,
            ClientCompanyId = dto.ClientCompanyId
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        await db.Entry(user).Reference(u => u.Role).LoadAsync();
        await db.Entry(user).Reference(u => u.ClientCompany).LoadAsync();

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Email = user.Email,
            RoleId = user.RoleId,
            RoleName = user.Role.Name,
            ClientCompanyId = user.ClientCompanyId,
            ClientCompanyName = user.ClientCompany?.Name
        };
    }

    public async Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await db.Users
            .Include(u => u.Role)
            .Include(u => u.ClientCompany)
            .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);

        if (user == null) return null;

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Phone = dto.Phone;
        user.Email = dto.Email;
        user.RoleId = dto.RoleId;
        user.ClientCompanyId = dto.ClientCompanyId;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        await db.SaveChangesAsync();

        await db.Entry(user).Reference(u => u.Role).LoadAsync();
        await db.Entry(user).Reference(u => u.ClientCompany).LoadAsync();

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Email = user.Email,
            RoleId = user.RoleId,
            RoleName = user.Role.Name,
            ClientCompanyId = user.ClientCompanyId,
            ClientCompanyName = user.ClientCompany?.Name
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await db.Users
            .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);

        if (user == null) return false;

        user.DeletedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();

        return true;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await db.Users
            .Include(u => u.Role)
            .Include(u => u.ClientCompany)
            .Include(u => u.DepartmentUsers.Where(du => du.DeletedAt == null))
                .ThenInclude(du => du.Department)
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.DeletedAt == null);

        if (user == null || user.Password == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) return null;

        var departments = user.DepartmentUsers
            .Select(du => du.Department.Name)
            .ToList();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim("departments", string.Join(",", departments))
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginResponseDto
        {
            Token = tokenString,
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            RoleName = user.Role.Name,
            Departments = departments
        };
    }
}