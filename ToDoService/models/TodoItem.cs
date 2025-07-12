namespace ToDoService.models;

public class TodoItem
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsComplete { get; set; }
}
