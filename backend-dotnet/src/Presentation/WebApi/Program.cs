using System;
using System.Linq;
using Application.Interfaces;
using Application.Services;
using Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// CORS: permite orígenes definidos en CORS_ORIGINS (separados por coma).
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        var corsOrigins = builder.Configuration["CORS_ORIGINS"]; // coma-separado
        if (!string.IsNullOrWhiteSpace(corsOrigins))
        {
            var raw = corsOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            // Acepta dominios con o sin esquema; si no traen http/https, agrega ambos
            var normalized = raw.SelectMany(o =>
                o.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                o.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                    ? new[] { o }
                    : new[] { $"https://{o}", $"http://{o}" }
            ).ToArray();

            if (normalized.Length > 0)
            {
                policy.WithOrigins(normalized).AllowAnyHeader().AllowAnyMethod();
            }
            else
            {
                policy.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod();
            }
        }
        else
        {
            // Fallback: permitir cualquier origen (útil para el despliegue inicial)
            policy.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod();
        }
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gastos e ingresos API",
        Version = "v1",
        Description = "API del sistema de gastos e ingresos personales"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT: **Bearer <token>**"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Pass Configuration to Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

// Health endpoint (para Render)
app.MapGet("/health", () => Results.Ok("OK"));

app.MapControllers();

app.Run();