using Microsoft.EntityFrameworkCore;
using APIRESTUnityWeb.Data;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------
// Configuración de la conexión a PostgreSQL
// ------------------------------------
var connectionString = builder.Configuration.GetConnectionString("PostgreConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ------------------------------------
// Configuración de controladores y JSON en camelCase
// ------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// ------------------------------------
// Swagger (documentación de la API)
// ------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------------------------------------
// Política CORS (para permitir requests desde frontend local u otros)
// ------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// ------------------------------------
// Middleware
// ------------------------------------
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
