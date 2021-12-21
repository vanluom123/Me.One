using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
            return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
                queryableOperator,
                source.IncludableQueryable.ThenInclude(navigationPropertyPath));
        }  

        public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            [NotNull] this IIncludeableReadRepository<TEntity, TPreviousProperty> source,
            [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class
        {
            var queryableOperator = (BaseReadRepository<TEntity>.QueryableRepoOperator<TEntity>)source;
            return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
                queryableOperator,
                source.IncludableQueryable.ThenInclude(navigationPropertyPath));
        }
    }
}