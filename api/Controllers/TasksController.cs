using Microsoft.AspNetCore.Mvc;
using ProductivityApi.Services;
using TodoTask = ProductivityApi.Models.TodoTask;

namespace ProductivityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskService _taskService;

    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoTask>>> Get()
    {
        var tasks = await _taskService.GetAllAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoTask>> Get(int id)
    {
        var task = await _taskService.GetByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TodoTask>> Post(TodoTask task)
    {
        var created = await _taskService.AddAsync(task);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoTask>> Put(int id, TodoTask task)
    {
        if (id != task.Id) return BadRequest();

        var updated = await _taskService.UpdateAsync(task);
        if (updated == null) return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _taskService.DeleteAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}
