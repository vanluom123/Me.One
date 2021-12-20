using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Me.One.Core.Contract.Repository;
using Microsoft.EntityFrameworkCore;

namespace Me.One.Core.Data
{
    public static class EntityFrameworkQueryableExtensions
    {
        public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            [NotNull] this IIncludeableReadRepository<TEntity, IEnumerable<TPreviousProperty>> source,
            [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        ) where TEntity : class
        {
            var queryableOperator = (BaseReadRepository<TEntity>.QueryableRepoOperator<TEntity>)source;
            var includeQueryableOperator =
                (BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, IEnumerable<TPreviousProperty>>)source;
            return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
                queryableOperator,
                includeQueryableOperator.IncludableQueryable.ThenInclude(navigationPropertyPath));
        }  

        public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            [NotNull] this IIncludeableReadRepository<TEntity, TPreviousProperty> source,
            [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class
        {
            var includeQueryableOperator = (BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TPreviousProperty>)source;
            var queryableOperator = (BaseReadRepository<TEntity>.QueryableRepoOperator<TEntity>)source;
            return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
                queryableOperator,
                includeQueryableOperator.IncludableQueryable.ThenInclude(navigationPropertyPath));
        }
    }
}