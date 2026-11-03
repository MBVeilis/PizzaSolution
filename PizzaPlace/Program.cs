using Microsoft.EntityFrameworkCore;
using PizzaPlace;
using PizzaPlace.Data;
using PizzaPlace.Factories;
using PizzaPlace.Models;
using PizzaPlace.Repositories;
using PizzaPlace.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o =>
{
    o.AddPolicy("allowAll",
        policy =>
        {
            policy.WithOrigins(
            // "whatEver.homepage.com"
            );
            policy.AllowAnyHeader();
            policy.AllowCredentials();
            policy.AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument(d =>
{
    d.Title = "Pizza Place";
    d.Version = "v1";
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("WebApiDatabase")
    )
);

// Register services:
var services = builder.Services;
services.AddSingleton(TimeProvider.System);

services.AddTransient<IStockRepository, StockRepository>();
services.AddTransient<IRecipeRepository, RecipeRepository>();

services.AddTransient<IPizzaOven, NormalPizzaOven>();
services.AddTransient<IPizzaOven, AssemblyLinePizzaOven>();
services.AddTransient<IPizzaOven, GiantRevolvingPizzaOven>();

services.AddTransient<IStockService, StockService>();
services.AddTransient<IRecipeService, RecipeService>();
services.AddTransient<IOrderingService, OrderingService>();
services.AddTransient<IMenuService, MenuService>();
services.AddScoped(_ => new PizzaOrder([]));
services.AddScoped(_ => new ComparableList<PizzaRecipeDto>());

var app = builder.Build();

/*using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DatabaseSeeder.SeedAsync(dbContext);
}*/

app.UseOpenApi();
app.UseSwaggerUi();

app.UseCors();

app.MapControllers();

app.Run();
