using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ToDoService.DbContextClasses;
using ToDoService.models;

var builder = WebApplication.CreateBuilder(args);

// Add DI -

builder.Services.AddDbContext<TodoDbContext>(opt => opt.UseInMemoryDatabase("TodoList"));

var app = builder.Build();

// Configuring the request pipelines

app.MapGet("/todoitems/{id}", async (int id, TodoDbContext db) => await db.Todos.FindAsync(id));
app.MapGet("/todoitems", async (TodoDbContext db) => await db.Todos.ToListAsync());
app.MapPost(
    "/todoitem",
    async (TodoItem todo, TodoDbContext db) =>
    {
        db.Todos.Add(todo);
        await db.SaveChangesAsync();
        Results.Created($"/todoitems/{todo.Id}", todo);
    }
);
app.MapPut(
    "/todoitem/{id}",
    async (int id, TodoItem inputTodo, TodoDbContext db) =>
    {
        var todo = await db.Todos.FindAsync(id);
        if (todo == null)
            return Results.NotFound();

        todo.Name = inputTodo.Name;
        todo.IsComplete = inputTodo.IsComplete;
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
);

app.MapDelete(
    "/todoitem/{id}",
    async (int id, TodoDbContext db) =>
    {
        var todo = await db.Todos.FindAsync(id);
        if (todo == null)
            return Results.NotFound();

        db.Todos.Remove(todo);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
);

app.Run();
