using GameStore.Data;
using GameStore.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Register services.
var connectionString = builder.Configuration.GetConnectionString("GameStore");

// Uses dependency injection. 
builder.Services.AddSqlite<GameStoreContext>(connectionString);

// Build an instance: logs Kestrel, and loads configurations.
var app = builder.Build();

app.MapGamesEndpoints();

app.MapGet("/", () => "You are " + System.Environment.MachineName + " and it is " + DateTime.Now);

app.MigrateDb();

app.Run();