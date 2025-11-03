using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Infrastructure.Persistence.Repositories;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Services;
using AutoMapper;
using Application.Mappings;

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

            // Register services
            services.AddAutoMapper(x =>
            {
                x.AddProfile<MappingProfile>();
            });
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
