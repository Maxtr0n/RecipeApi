﻿using Ardalis.Specification;
using Domain.Abstractions;

namespace Application.Abstractions;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{

}
