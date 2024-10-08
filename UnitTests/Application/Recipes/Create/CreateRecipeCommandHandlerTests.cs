﻿using Application.Common.Abstractions.Repositories;
using Application.Common.Dtos;
using Application.Common.Extensions;
using Application.Recipes.Create;
using Ardalis.Result;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace UnitTests.Application.Recipes.Create;

public class CreateRecipeCommandHandlerTests
{
    private readonly Mock<IRepository<Recipe>> _recipeRepositoryMock;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly ApplicationUser user;

    public CreateRecipeCommandHandlerTests()
    {
        _mapper = new();
        _recipeRepositoryMock = new();

        user = new ApplicationUser();

        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            new Mock<IUserStore<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .Returns(Task.FromResult(IdentityResult.Success));
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenRecipeCannotBeCreated()
    {
        // Arrange
        var dto = new RecipeCreateDto
        {
            Title = "Recipe Title",
            Ingredients = ["Ingredient1", "Ingredient2"],
            Images = [],
            Description = "Recipe Description",
            AuthorId = Guid.NewGuid(),

        };

        var recipe = new Recipe(Guid.NewGuid(), dto.Title, dto.Ingredients.JoinStrings(), dto.Description, dto.Images.JoinStrings(), user);

        _mapper.Setup(x => x.Map<Recipe>(It.IsAny<RecipeCreateDto>())).Returns(recipe);
        _recipeRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Recipe>(), It.IsAny<CancellationToken>())).ReturnsAsync((Recipe)null!);

        var command = new CreateRecipeCommand(dto);
        var handler = new CreateRecipeCommandHandler(_mapper.Object, _recipeRepositoryMock.Object, _userManagerMock.Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Status.Should().Be(ResultStatus.CriticalError);
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFoundResult_WhenUserIsNotRegistered()
    {
        // Arrange
        var dto = new RecipeCreateDto
        {
            Title = "Recipe Title",
            Ingredients = ["Ingredient1", "Ingredient2"],
            Images = [],
            Description = "Recipe Description",
            AuthorId = Guid.NewGuid(),
        };

        var recipe = new Recipe(Guid.NewGuid(), dto.Title, dto.Ingredients.JoinStrings(), dto.Description, dto.Images.JoinStrings(), user);

        _mapper.Setup(x => x.Map<Recipe>(It.IsAny<RecipeCreateDto>())).Returns(recipe);
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null!);

        var command = new CreateRecipeCommand(dto);
        var handler = new CreateRecipeCommandHandler(_mapper.Object, _recipeRepositoryMock.Object, _userManagerMock.Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Status.Should().Be(ResultStatus.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenRecipeWasCreated()
    {
        // Arrange
        var dto = new RecipeCreateDto
        {
            Title = "Recipe Title",
            Ingredients = ["Ingredient1", "Ingredient2"],
            Images = [],
            Description = "Recipe Description",
            AuthorId = Guid.NewGuid(),
        };

        var readDto = new RecipeReadDto
        {
            Id = Guid.NewGuid(),
            Title = "Recipe Title",
            Author = "Author",
            Ingredients = ["Ingredient1", "Ingredient2"],
            Images = [],
            Description = "Recipe Description"
        };

        var user = new ApplicationUser();
        var recipe = new Recipe(Guid.NewGuid(), dto.Title, dto.Ingredients.JoinStrings(), dto.Description, dto.Images.JoinStrings(), user);

        _mapper.Setup(x => x.Map<Recipe>(It.IsAny<RecipeCreateDto>())).Returns(recipe);
        _mapper.Setup(x => x.Map<RecipeReadDto>(It.IsAny<Recipe>())).Returns(readDto);
        _recipeRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Recipe>(), It.IsAny<CancellationToken>())).ReturnsAsync(recipe);

        var command = new CreateRecipeCommand(dto);
        var handler = new CreateRecipeCommandHandler(_mapper.Object, _recipeRepositoryMock.Object, _userManagerMock.Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().NotBeEmpty();
        result.Value.Title.Should().Be(dto.Title);
        result.Value.Ingredients.Should().Contain(dto.Ingredients);
        result.Value.Description.Should().Be(dto.Description);
    }
}
