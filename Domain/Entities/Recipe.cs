﻿using Ardalis.GuardClauses;
using Domain.Abstractions;
using SharedKernel;

namespace Domain.Entities;

public class Recipe : Entity, IAggregateRoot
{
    public Recipe(
        Guid id,
        string title,
        string ingredients,
        string description,
        string? images,
        string author
        ) : base(id)
    {
        GuardAgainstInvalidInput(title, ingredients, description, author);

        Title = title;
        Ingredients = ingredients;
        Description = description;
        Images = images;
        Author = author;

    }

    private static void GuardAgainstInvalidInput(string title, string ingredients, string description, string author)
    {
        Guard.Against.NullOrEmpty(title);
        Guard.Against.StringTooShort(title, 3);
        Guard.Against.StringTooLong(title, 100);

        Guard.Against.NullOrEmpty(ingredients);

        Guard.Against.NullOrEmpty(description);
        Guard.Against.StringTooShort(description, 3);
        Guard.Against.StringTooLong(description, 5000);

        Guard.Against.NullOrEmpty(author);
        Guard.Against.StringTooShort(author, 3);
        Guard.Against.StringTooLong(author, 100);
    }

    public string Title { get; private set; }

    public string Ingredients { get; private set; }

    public string Description { get; private set; }

    public string? Images { get; private set; }

    public string Author { get; private set; }

    public void Update(
        string title,
        string ingredients,
        string description,
        string? images,
        string author)
    {
        Guard.Against.NullOrEmpty(title);
        Guard.Against.NullOrEmpty(ingredients);
        Guard.Against.NullOrEmpty(description);
        Guard.Against.NullOrEmpty(author);

        Title = title;
        Ingredients = ingredients;
        Description = description;
        Images = images;
        Author = author;
    }
}
