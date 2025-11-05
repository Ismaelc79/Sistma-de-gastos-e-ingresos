using System;
using System.Linq;
using Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// CORS: lee CORS_ORIGINS y además permite *.vercel.app para pruebas
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        var corsOrigins = builder.Configuration["CORS_ORIGINS"]; // coma-separado

        string[] normalized = Array.Empty<string>();
        if (!string.IsNullOrWhiteSpace(corsOrigins))
        {
            var raw = corsOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            normalized = raw
                .SelectMany(o =>
                    o.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                    o.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                        ? new[] { o.TrimEnd('/') }
                        : new[] { $"https://{o.TrimEnd('/')}", $"http://{o.TrimEnd('/')}" }
                )
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        bool AllowAnyVercel(string origin)
        {
            try { return new Uri(origin).Host.EndsWith(".vercel.app", StringComparison.OrdinalIgnoreCase); }
            catch { return false; }
        }

        if (normalized.Length > 0)
        {
            policy
                .SetIsOriginAllowed(origin =>
                    normalized.Contains(origin.TrimEnd('/'), StringComparer.OrdinalIgnoreCase) ||
                    AllowAnyVercel(origin)
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            // Fallback: permitir todo (útil para despliegue inicial)
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

// IoC
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

// Health endpoint (Render)
app.MapGet("/health", () => Results.Ok("OK"));

app.MapControllers();

app.Run();

