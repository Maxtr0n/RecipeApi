﻿using Application.Common.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipeReadDto>();
            CreateMap<RecipeCreateDto, Recipe>();
        }
    }
}
