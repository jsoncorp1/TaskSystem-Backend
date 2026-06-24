using Microsoft.EntityFrameworkCore;
using TaskSystem_Back.Data;
using TaskSystem_Back.DTOs.ClientCompany;
using TaskSystem_Back.DTOs.Common;

namespace TaskSystem_Back.Services;

public class ClientCompanyService(AppDbContext db)
{
    public async Task<PagedResultDto<ClientCompanyDto>> GetAllAsync(int page, int pageSize, string? nombre, string? email)
    {
        var query = db.ClientCompanies.Where(c => c.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(c => c.Name.ToLower().Contains(nombre.ToLower()));

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(c => c.Email != null && c.Email.ToLower().Contains(email.ToLower()));

        var total = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ClientCompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Email = c.Email,
                Phone = c.Phone
            })
            .ToListAsync();

        return new PagedResultDto<ClientCompanyDto>
        {
            TotalItems = total,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<ClientCompanyDto?> GetByIdAsync(Guid id)
    {
        var company = await db.ClientCompanies
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (company == null) return null;

        return new ClientCompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            Description = company.Description,
            Email = company.Email,
            Phone = company.Phone
        };
    }

    public async Task<ClientCompanyDto> CreateAsync(CreateClientCompanyDto dto)
    {
        var company = new Models.ClientCompany
        {
            Name = dto.Name,
            Description = dto.Description,
            Email = dto.Email,
            Phone = dto.Phone
        };

        db.ClientCompanies.Add(company);
        await db.SaveChangesAsync();

        return new ClientCompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            Description = company.Description,
            Email = company.Email,
            Phone = company.Phone
        };
    }

    public async Task<ClientCompanyDto?> UpdateAsync(Guid id, UpdateClientCompanyDto dto)
    {
        var company = await db.ClientCompanies
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (company == null) return null;

        company.Name = dto.Name;
        company.Description = dto.Description;
        company.Email = dto.Email;
        company.Phone = dto.Phone;
        company.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync();

        return new ClientCompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            Description = company.Description,
            Email = company.Email,
            Phone = company.Phone
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var company = await db.ClientCompanies
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (company == null) return false;

        company.DeletedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();

        return true;
    }
}