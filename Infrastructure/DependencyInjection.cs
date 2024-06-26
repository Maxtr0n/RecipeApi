﻿using Application.Common.Abstractions.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddDbContext<RecipeDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
        services.AddHealthChecks()
            .AddDbContextCheck<RecipeDbContext>(name: "Database");

        return services;
    }
}
