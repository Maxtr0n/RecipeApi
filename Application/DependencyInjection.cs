﻿using Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(assembly);

        // For now, only use automapper for the returned DTOS, because I create entities manually, following DDD
        // later could be refactored, but must learn how to integrate automapper to work with DDD entities (constructors with parameters - id)
        services.AddAutoMapper(assembly);

        return services;
    }
}
