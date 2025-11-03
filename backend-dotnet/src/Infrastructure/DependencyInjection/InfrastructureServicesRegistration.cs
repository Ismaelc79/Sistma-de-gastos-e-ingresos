using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using Infrastructure.BackgroundServices;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DependencyInjection
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DapperContext (IConfiguration inyected in DapperContext)
            services.AddSingleton<DapperContext>();

            // Register respositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ITransactionService, TransactionService>();

            // Register services
            services.AddAutoMapper(x =>
            {
                x.AddProfile<MappingProfile>();
            });
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            // Register background services
            services.AddHostedService<TokenCleanupService>();


            // Configurate JWT Middlerware
            var jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key not configured");
            var jwtIssuer = configuration["Jwt:Issuer"] ?? "default_issuer";
            var jwtAudience = configuration["Jwt:Audience"] ?? "default_audience";

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            services.AddAuthorization();

            // Add token services
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
