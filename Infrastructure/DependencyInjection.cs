﻿using Application.Abstractions;
using Domain.Abstractions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepository<IAggregateRoot>, GenericRepository<IAggregateRoot>>();
            services.AddDbContext<RecipeDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
            return services;
        }
    }
}
