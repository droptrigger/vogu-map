using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using VoguMap.Application.Mappings;
using VoguMap.Application.Services.Implementations;
using VoguMap.Application.Services.Interfaces;
using VoguMap.Domain.Interfaces.Repositories;
using VoguMap.Infrastructure.Persistence.Context;
using VoguMap.Infrastructure.Persistence.Repositories;
using VoguMap.Infrastructure.Persistence.Seed;
using VoguMap.Web.Middlewares;

// Поиск .env файла
Env.TraversePath().Load();

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

#region AutoMapper

builder.Services.AddAutoMapper(
    cfg => { },
    typeof(RoomProfile).Assembly,
    typeof(BuildingProfile).Assembly
);

#endregion

#region Репозитории

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();

#endregion

#region Сервисы

builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBuildingService, BuildingService>();

#endregion

// Добавление контроллеров
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        {
            // Игнорирование циклических ссылок при сериализации JSON
            options.JsonSerializerOptions.ReferenceHandler =
                System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            // Игнорирование null значений
            options.JsonSerializerOptions.DefaultIgnoreCondition =
                System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        }
    );

#region Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VoguMap API",
        Version = "v1",
        Description = "API для управления корпусами и помещениями",
        Contact = new OpenApiContact { Name = "Support", Email = "vasya.domakov@mail.ru" }
    });
});

#endregion

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Глобальная обработка исключений
app.UseMiddleware<ExceptionHandlerMiddleware>();

#region Миграции

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<VoguMapContext>();

// Занесение справочных данных
ApplicationDbSeeder.SeedBuildings(context);

if (app.Environment.IsDevelopment())
{
    context.Database.Migrate();
    
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VoguMap API v1"));

    // Configure the HTTP request pipeline.
    app.MapOpenApi();
}

#endregion

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
