using Microsoft.EntityFrameworkCore;
using TurismoApp.Application.Implementations;
using TurismoApp.Application.Interfaces;
using TurismoApp.Infraestructure;
using TurismoApp.Infraestructure.Repositories.Implementations;
using TurismoApp.Infraestructure.Repositories.Interfaces;
using TurismoApp.Services.Implementations;
using TurismoApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

builder.Services.AddScoped<IRepositoryDepartamento, RepositoryDepartamento>();
builder.Services.AddScoped<IApplicationDepartamento, ApplicationDepartamento>();

builder.Services.AddScoped<IRepositoryCiudad, RepositoryCiudad>();
builder.Services.AddScoped<IApplicationCiudad, ApplicationCiudad>();

builder.Services.AddScoped<IRepositoryRecorrido, RepositoryRecorrido>();
builder.Services.AddScoped<IApplicationRecorrido, ApplicationRecorrido>();

builder.Services.AddScoped<IRepositoryCliente, RepositoryCliente>();
builder.Services.AddScoped<IApplicationCliente, ApplicationCliente>();

builder.Services.AddScoped<IServiceEmail, ServiceEmail>();
builder.Services.AddScoped<IApplicationEmail, ApplicationEmail>();

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
