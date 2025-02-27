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
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}