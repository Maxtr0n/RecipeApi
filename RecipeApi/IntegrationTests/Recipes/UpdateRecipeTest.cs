﻿using Application.Common.Dtos;
using Application.Recipes.Update;
using FluentAssertions;

namespace IntegrationTests.Recipes;

public class UpdateRecipeTest(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Delete_Should_Remove_Recipe_From_Database()
    {
        // Arrange
        var recipeId = await CreateUserAndRecipeForTesting();

        var updateRecipeDto = new RecipeUpdateDto
        {
            Title = "My New Title",
            Description = "My New Description",
            Ingredients = ["Ingredient 4, Ingredient 5, Ingredient 6"],
            Images = ["Images 1, Images 2, Images 3"]
        };

        var updateCommand = new UpdateRecipeCommand(recipeId, updateRecipeDto);

        // Act
        var result = await Sender.Send(updateCommand);

        // Assert
        result.IsSuccess.Should().BeTrue();
        DbContext.Recipes.Should().HaveCount(1);
    }
}