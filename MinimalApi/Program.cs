using MinimalApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MinimalApidb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

static async Task<List<SuperHero>> GetAllHeroes(DataContext context) => 
    await context.SuperHeroes.ToListAsync();

app.MapGet("/FirstPage", () => "Welcome to WebApiDB!");

app.MapGet("/superhero", async (DataContext context) => 
    await context.SuperHeroes.ToListAsync());

app.MapGet("/seperhero/{id}", async (DataContext context, int id) =>
    await context.SuperHeroes.FindAsync(id) is SuperHero hero ? 
    Results.Ok(hero): 
    Results.NotFound("Sorry, Hero not found. "));


app.MapPost("/superhero", async (DataContext context, SuperHero hero) =>
{
    context.SuperHeroes.Add(hero);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllHeroes(context));
});

app.MapPut("/superhero/{id}", async (DataContext context, SuperHero hero, int id) =>
{
    var dbHero = await context.SuperHeroes.FindAsync(id);
    if (dbHero == null) return Results.NotFound("No Hero Found");

    dbHero.FirstName = hero.FirstName;
    dbHero.LastName = hero.LastName;
    dbHero.HeroName = hero.HeroName;
    await context.SaveChangesAsync();

    return Results.Ok(await GetAllHeroes(context));
});

app.MapDelete("/superhero/{id}", async (DataContext context, int id) =>
{
    var dbHero = await context.SuperHeroes.FindAsync(id);

    if (dbHero == null) return Results.NotFound("No Hero Found");

    context.SuperHeroes.Remove(dbHero);

    await context.SaveChangesAsync();

    return Results.Ok(await GetAllHeroes(context));
});
app.Run();
