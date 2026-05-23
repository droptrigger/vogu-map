using Microsoft.EntityFrameworkCore;
using VoguMap.Infrastructure.Data.Context;

var builder = WebApplication.CreateBuilder(args);

#region Переменные окружения

var dbHost = builder.Configuration["DB_HOST"]
    ?? throw new InvalidOperationException("Missing environment variable DB_HOST.");
var dbPort = builder.Configuration["DB_PORT"]
    ?? throw new InvalidOperationException("Missing environment variable DB_PORT.");
var dbName = builder.Configuration["DB_NAME"]
    ?? throw new InvalidOperationException("Missing environment variable DB_NAME.");
var dbUser = builder.Configuration["DB_USER"]
    ?? throw new InvalidOperationException("Missing environment variable DB_USER.");
var dbPassword = builder.Configuration["DB_PASSWORD"]
    ?? throw new InvalidOperationException("Missing environment variable DB_PASSWORD.");

var connectionString =
    $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

#endregion

#region База данных

// Настройка подключения к базе данных PostgreSQL
builder.Services.AddDbContext<VoguMapContext>(options =>
    options.UseNpgsql(connectionString));

#endregion

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

#region Миграции

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<VoguMapContext>();
    context.Database.Migrate();

    // Configure the HTTP request pipeline.
    app.MapOpenApi();
}

#endregion

app.UseHttpsRedirection();

app.Run();
