using Application.Interfaces;
using Application.Services;
using Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
<<<<<<< Updated upstream
            policy
                .WithOrigins("http://localhost:5173") // URL del frontend
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
=======
            var raw = corsOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            // Acepta dominios con o sin esquema; si no traen http/https, agrega ambos
            var normalized = raw.SelectMany(o =>
                o.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || o.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
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
            // Fallback: permitir cualquier origen (útil para pruebas/despliegue inicial)
            policy.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod();
        }
    });
>>>>>>> Stashed changes
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
        Description = "Ingrese el token JWT: **Bearer &lt;token&gt;**"
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

app.MapControllers();

app.Run();
