using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


//using Microsoft.EntityFrameworkCore;
//using PingAPI.Repositories;

//var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlite("Data Source=playerdata.db"));//uso la base de datos

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer(); // Para Swagger
//builder.Services.AddSwaggerGen();           // Para Swagger UI
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policy =>
//    {
//        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//    });
//});

//var app = builder.Build();

//// Middleware para desarrollo
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseCors();
//app.UseAuthorization();
//app.MapControllers();
//app.Run();
