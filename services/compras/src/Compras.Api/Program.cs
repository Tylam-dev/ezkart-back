using Compras.Api.Servicio;
using Compras.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Falta Jwt:Key");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Falta Jwt:Issuer");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Falta Jwt:Audience");
var clockSkew = builder.Configuration.GetValue("Jwt:ClockSkewSegundos", 30);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(jwtKey)),
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            ClockSkew = TimeSpan.FromSeconds(clockSkew)
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("access_token_ezkart", out var token))
                    context.Token = token;

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICarritoServicio, CarritoServicio>();
builder.Services.AddScoped<IOrdenesServicio, OrdenesServicio>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ReenviarTokenHandler>();
builder.Services
    .AddHttpClient<IClienteHttpInventario, ClienteHttpInventario>(cliente =>
    {
        var url = builder.Configuration["Inventario:BaseUrl"]
            ?? throw new InvalidOperationException("Falta Inventario:BaseUrl");
        cliente.BaseAddress = new Uri(url);
    })
    .AddHttpMessageHandler<ReenviarTokenHandler>();

var app = builder.Build();

await app.Services.InicializarBaseDatosAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
