using Microsoft.EntityFrameworkCore;
using ProductivityApi.Models;

namespace ProductivityApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<TodoTask> TodoTask => Set<TodoTask>();
}
