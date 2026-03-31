using Microsoft.EntityFrameworkCore;
using ProductivityApi.Data;
using ProductivityApi.Models;

namespace ProductivityApi.Services;

public class TaskService
{
    private readonly AppDbContext _db;

    public TaskService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<TodoTask>> GetAllAsync() => await _db.TodoTask.ToListAsync();
    public async Task<TodoTask?> GetByIdAsync(int id) => await _db.TodoTask.FindAsync(id);
    public async Task<TodoTask> AddAsync(TodoTask task)
    {
        _db.TodoTask.Add(task);
        await _db.SaveChangesAsync();
        return task;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _db.TodoTask.FindAsync(id);
        if (task == null) return false;
        _db.TodoTask.Remove(task);
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<TodoTask?> UpdateAsync(TodoTask task)
    {
        var existing = await _db.TodoTask.FindAsync(task.Id);
        if (existing == null) return null;
        existing.Title = task.Title;
        existing.Completed = task.Completed;
        existing.Priority = task.Priority;
        await _db.SaveChangesAsync();
        return existing;
    }
}
