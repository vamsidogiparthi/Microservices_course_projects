namespace ToDoService.DbContextClasses;

using Microsoft.EntityFrameworkCore;
using ToDoService.models;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> Todos { get; set; }
}
