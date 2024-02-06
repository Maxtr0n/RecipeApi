﻿using Domain.Abstractions;

namespace Domain.Entities
{
    public class Recipe : BaseEntity, IAggregateRoot
    {
        public string Title { get; set; } = default!;

        public IEnumerable<string> Ingredients { get; set; } = Enumerable.Empty<string>();

        public string Description { get; set; } = default!;

        public IEnumerable<string> Images { get; set; } = Enumerable.Empty<string>();

        public string Author { get; set; } = default!;
    }
}
