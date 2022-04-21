using Microsoft.EntityFrameworkCore;
using WebApiTodoes.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet(
    "/todoitems",
    async (TodoContext db) => await db.TodoItems.ToListAsync());

app.MapGet(
    "/todoitems/complete",
    async (TodoContext db) => await db
        .TodoItems
        .Where(x => x.IsComplete)
        .ToListAsync());

app.MapGet(
    "/todoitems/{id}",
    async (int id, TodoContext db) => await db.TodoItems.FindAsync(id)
        is TodoItem todoItem
        ? Results.Ok(todoItem)
        : Results.NotFound());

app.MapPost(
    "/todoitems",
    async (TodoItem item, TodoContext db) =>
    {
        db.TodoItems.Add(item);

        await db.SaveChangesAsync();

        return Results.Created($"/todoitems/{item.Id}", item);
    });

app.MapPut(
    "/todoitems/{id}",
    async (int id, TodoItem item, TodoContext db) =>
    {
        var existingItem = await db.TodoItems.FindAsync(id);

        if (existingItem is null) return Results.NotFound();

        existingItem.Name = item.Name;
        existingItem.IsComplete = item.IsComplete;

        await db.SaveChangesAsync();

        return Results.NoContent();
    });

app.MapDelete(
    "/todoitems/{id}",
    async (int id, TodoContext db) =>
    {
        if (await db.TodoItems.FindAsync(id) is TodoItem item)
        {
            db.TodoItems.Remove(item);
            await db.SaveChangesAsync();
            return Results.Ok(item);
        }

        return Results.NotFound();
    });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
