﻿using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Infrastructure;
using Scalar.AspNetCore;
using System.Net;

namespace RecipeApi.Extensions;

public static class ApiExtensions
{
    public static IServiceCollection SetupApi(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddControllers(mvcOptions => mvcOptions
            .AddResultConvention(resultStatusMap => resultStatusMap
                .AddDefaultMap()
                .For(ResultStatus.Ok, HttpStatusCode.OK, resultStatusOptions => resultStatusOptions
                    .For("POST", HttpStatusCode.Created)
                    .For("DELETE", HttpStatusCode.NoContent))
            ));

        services.AddAuthorization();

        services.AddIdentityApiEndpoints<ApplicationUser>(options =>
            {
            })
            .AddEntityFrameworkStores<RecipeDbContext>();

        services.AddHealthChecks();

        services.AddOpenApi();

        return services;
    }

    public static WebApplication UseApi(this WebApplication app)
    {
        app.MapOpenApi();

        if (app.Environment.IsDevelopment())
        {
            app.MapScalarApiReference();
        }

        //app.UseHttpsRedirection();

        app.MapIdentityApi<ApplicationUser>();

        app.UseExceptionHandler();

        app.MapControllers();

        return app;
    }

    public static void ApplyMigrations(this WebApplication app, bool exit = false)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<RecipeDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<RecipeDbContext>>();

        // Check and apply pending migrations
        var pendingMigrations = dbContext.Database.GetPendingMigrations();

        var migrations = pendingMigrations.ToList();

        if (migrations.Count == 0)
        {
            logger.LogInformation("No pending migrations found.");
            if (exit)
            {
                Environment.Exit(0);
            }
        }

        logger.LogInformation("Applying {MigrationsCount} migrations to  database...", migrations.Count);

        try
        {
            dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Applying database migrations failed.");
            Environment.Exit(1);
        }

        logger.LogInformation("Applying database migrations succeeded.");

        if (exit)
        {
            Environment.Exit(0);
        }
    }
}