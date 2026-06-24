using Microsoft.AspNetCore.Mvc;
using TaskSystem_Back.DTOs.TaskItem;
using TaskSystem_Back.Services;

namespace TaskSystem_Back.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskItemsController : ControllerBase
{
    private readonly TaskItemService _taskItemService;

    public TaskItemsController(TaskItemService taskItemService)
    {
        _taskItemService = taskItemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? titulo = null,
        [FromQuery] Guid? subProjectId = null,
        [FromQuery] string? status = null,
        [FromQuery] Guid? assignedUserId = null)
    {
        var result = await _taskItemService.GetAllAsync(page, pageSize, titulo, subProjectId, status, assignedUserId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _taskItemService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskItemDto dto)
    {
        var result = await _taskItemService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskItemDto dto)
    {
        var result = await _taskItemService.UpdateAsync(id, dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _taskItemService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}