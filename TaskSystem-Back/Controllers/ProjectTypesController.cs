using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskSystem_Back.DTOs.ProjectType;
using TaskSystem_Back.Services;

namespace TaskSystem_Back.Controllers;

[Authorize]
[ApiController]
[Route("api/project-types")]
public class ProjectTypesController(ProjectTypeService projectTypeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? nombre = null)
    {
        var result = await projectTypeService.GetAllAsync(page, pageSize, nombre);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await projectTypeService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectTypeDto dto)
    {
        var result = await projectTypeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectTypeDto dto)
    {
        var result = await projectTypeService.UpdateAsync(id, dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await projectTypeService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}