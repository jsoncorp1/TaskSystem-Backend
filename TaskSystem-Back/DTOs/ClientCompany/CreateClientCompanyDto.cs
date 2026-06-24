using System.ComponentModel.DataAnnotations;

namespace TaskSystem_Back.DTOs.ClientCompany;

public class CreateClientCompanyDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}