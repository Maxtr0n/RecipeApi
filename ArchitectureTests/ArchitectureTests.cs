using Application;
using Domain.Abstractions;
using FluentAssertions;
using Infrastructure.Persistence.EntityConfigurations;
using NetArchTest.Rules;
using RecipeApi.Controllers;

namespace ArchitectureTests;

public class ArchitectureTests
{
    private const string DomainNamespace = "Domain";
    private const string ApplicationNamespace = "Application";
    private const string InfrastructureNamespace = "Infrastructure";
    private const string WebNamespace = "RecipeApi";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(IAggregateRoot).Assembly;

        var otherProjects = new[] { ApplicationNamespace, InfrastructureNamespace, WebNamespace };

        // Act
        var testResults = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Assert
        testResults.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(DependencyInjection).Assembly;

        var otherProjects = new[] { InfrastructureNamespace, WebNamespace };

        // Act
        var testResults = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Assert
        testResults.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handlers_Should_Have_DependencyOnDomain()
    {
        // Arrange
        var assembly = typeof(DependencyInjection).Assembly;

        // Act
        var result = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();


        //Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(RecipeEntityTypeConfiguration).Assembly;

        var otherProjects = new[] { WebNamespace };

        // Act
        var testResults = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Assert
        testResults.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_Should_HaveDependencyOnMediatr()
    {
        // Arrange
        var assembly = typeof(RecipesController).Assembly;

        // Act
        var testResults = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .And()
            .DoNotHaveNameMatching("HealthController")
            .Should()
            .HaveDependencyOn("MediatR")
            .GetResult();

        //Assert
        testResults.IsSuccessful.Should().BeTrue();
    }
}