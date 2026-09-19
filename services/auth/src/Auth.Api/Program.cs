using Auth.Api.Servicio;
using Auth.Application;
using Auth.Application.IServicios;
using Auth.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Falta Jwt:Key");

var minutos = builder.Configuration.GetValue("Jwt:AccessTokenMinutos", 15.0);

builder.Services.AddSingleton<IJwtTokenServicio>(
    _ => new JwtTokenServicio(jwtKey, minutos));
    
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
