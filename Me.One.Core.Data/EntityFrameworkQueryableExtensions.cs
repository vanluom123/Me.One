using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Me.One.Core.Contract.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Me.One.Core.Data
{
    public static class EntityFrameworkQueryableExtensions
    {
        //#region FunctionExtension

        //public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //    [NotNull] this IIncludeableReadRepository<TEntity, IEnumerable<TPreviousProperty>> source,
        //    [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        //) where TEntity : class
        //{
        //    var queryable = source as BaseReadRepository<TEntity>.QueryableRepoOperator<TEntity>;
        //    var includeQuery =
        //        source as BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, IEnumerable<TPreviousProperty>>;
        //    return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
        //        queryable,
        //        includeQuery?.IncludeQuery.ThenInclude(navigationPropertyPath));
        //}

        //public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //    [NotNull] this IIncludeableReadRepository<TEntity, ICollection<TPreviousProperty>> source,
        //    [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        //) where TEntity : class
        //{
        //    var queryable = source as BaseReadRepository<TEntity>.QueryableRepoOperator<TEntity>;
        //    var includeQuery =
        //        source as BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, ICollection<TPreviousProperty>>;
        //    return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
        //        queryable,
        //        includeQuery?.IncludeQuery.ThenInclude(navigationPropertyPath));
        //}

        //public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //    [NotNull] this IIncludeableReadRepository<TEntity, IList<TPreviousProperty>> source,
        //    [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        //) where TEntity : class
        //{
        //    var queryable = source as BaseReadRepository<TEntity>.QueryableRepoOperator<TEntity>;
        //    var includeQuery =
        //        source as BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, IList<TPreviousProperty>>;
        //    return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
        //        queryable,
        //        includeQuery?.IncludeQuery.ThenInclude(navigationPropertyPath));
        //}

        //public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //    [NotNull] this IIncludeableReadRepository<TEntity, List<TPreviousProperty>> source,
        //    [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        //) where TEntity : class
        //{
        //    var queryable = source as BaseReadRepository<TEntity>.QueryableRepoOperator<TEntity>;
        //    var includeQuery =
        //        source as BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, List<TPreviousProperty>>;
        //    return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
        //        queryable,
        //        includeQuery?.IncludeQuery.ThenInclude(navigationPropertyPath));
        //}

        //public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //    [NotNull] this IIncludeableReadRepository<TEntity, HashSet<TPreviousProperty>> source,
        //    [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        //) where TEntity : class
        //{
        //    var queryable = source as BaseReadRepository<TEntity>.QueryableRepoOperator<TEntity>;
        //    var includeQuery =
        //        source as BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, HashSet<TPreviousProperty>>;
        //    return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
        //        queryable,
        //        includeQuery?.IncludeQuery.ThenInclude(navigationPropertyPath));
        //}

        //public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //    [NotNull] this IIncludeableReadRepository<TEntity, Stack<TPreviousProperty>> source,
        //    [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        //) where TEntity : class
        //{
        //    var queryable = source as BaseReadRepository<TEntity>.QueryableRepoOperator<TEntity>;
        //    var includeQuery =
        //        source as BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, Stack<TPreviousProperty>>;
        //    return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
        //        queryable,
        //        includeQuery?.IncludeQuery.ThenInclude(navigationPropertyPath));
        //}

        //public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //    [NotNull] this IIncludeableReadRepository<TEntity, SortedSet<TPreviousProperty>> source,
        //    [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        //) where TEntity : class
        //{
        //    var queryable = source as BaseReadRepository<TEntity>.QueryableRepoOperator<TEntity>;
        //    var includeQuery =
        //        source as BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, SortedSet<TPreviousProperty>>;
        //    return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
        //        queryable,
        //        includeQuery?.IncludeQuery.ThenInclude(navigationPropertyPath));
        //}

        //public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //    [NotNull] this IIncludeableReadRepository<TEntity, LinkedList<TPreviousProperty>> source,
        //    [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        //) where TEntity : class
        //{
        //    var queryable = source as BaseReadRepository<TEntity>.QueryableRepoOperator<TEntity>;
        //    var includeQuery =
        //        source as BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, LinkedList<TPreviousProperty>>;
        //    return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
        //        queryable,
        //        includeQuery?.IncludeQuery.ThenInclude(navigationPropertyPath));
        //}

        //public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //    [NotNull] this IIncludeableReadRepository<TEntity, Queue<TPreviousProperty>> source,
        //    [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        //) where TEntity : class
        //{
        //    var queryable = source as BaseReadRepository<TEntity>.QueryableRepoOperator<TEntity>;
        //    var includeQuery =
        //        source as BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, Queue<TPreviousProperty>>;
        //    return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
        //        queryable,
        //        includeQuery?.IncludeQuery.ThenInclude(navigationPropertyPath));
        //}

        //public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //    [NotNull] this IIncludeableReadRepository<TEntity, TPreviousProperty> source,
        //    [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
        //    where TEntity : class
        //{
        //    var queryable = source as BaseReadRepository<TEntity>.QueryableRepoOperator<TEntity>;
        //    var includeQuery =
        //        source as BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TPreviousProperty>;
        //    return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(
        //        queryable,
        //        includeQuery?.IncludeQuery.ThenInclude(navigationPropertyPath));
        //}

        //#endregion

        #region ThenInclude

        //internal static readonly MethodInfo ThenIncludeAfterEnumerableMethodInfo
        //    = typeof(Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions)
        //        .GetTypeInfo().GetDeclaredMethods(nameof(ThenInclude))
        //        .Where(mi => mi.GetGenericArguments().Count() == 3)
        //        .Single(
        //            mi =>
        //            {
        //                var typeInfo = mi.GetParameters()[0].ParameterType.GenericTypeArguments[1];
        //                return typeInfo.IsGenericType
        //                       && typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        //            });


        //public static IIncludeableReadRepository<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //    [NotNull] this IIncludeableReadRepository<TEntity, HashSet<TPreviousProperty>> source,
        //    [NotNull] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        //) where TEntity : class
        //{
        //    var provider = source.Provider as EntityQueryProvider;
        //    var queryableOperator = (BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, HashSet<TPreviousProperty>>)source;
        //    if (provider == null)
        //    {
        //        return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(source, queryableOperator.QueryableRepoOperator);
        //    }

        //    return new BaseReadRepository<TEntity>.IncludeQueryableOperator<TEntity, TProperty>(provider.CreateQuery<TEntity>(
        //            Expression.Call(
        //                instance: null,
        //                method: ThenIncludeAfterEnumerableMethodInfo.MakeGenericMethod(
        //                    typeof(TEntity), typeof(TPreviousProperty), typeof(TProperty)),
        //                arguments: new[] { source.Expression, Expression.Quote(navigationPropertyPath) })),
        //        queryableOperator.QueryableRepoOperator);
        //}

        #endregion

    }
}