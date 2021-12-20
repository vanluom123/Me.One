using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Me.One.Core.Contract.Repository
{
    public interface IBaseQueryableOperator<T> where T : class
    {
        IEnumerable<T> List();

        IEnumerable<T> List(Expression<Func<T, bool>> predicate);

        IEnumerable<T> List(
            Expression<Func<T, bool>> predicate,
            int pageIndex,
            int pageSize,
            out long totalRecords);

        IEnumerable<T> List(
            Expression<Func<T, bool>> predicate,
            int pageIndex,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>> orderBy,
            out long totalRecords);

        IEnumerable<T> List(Expression<Func<T, bool>> predicate, List<OrderBy> orderBy);

        IEnumerable<T> List(
            Expression<Func<T, bool>> predicate,
            List<OrderBy> orderBy,
            int pageIndex,
            int pageSize,
            out long totalRecords);

        bool Any(Expression<Func<T, bool>> predicate = null);

        IBaseQueryableOperator<T> Include(string navigationPropertyPath);

    }

    public interface IBaseReadRepository<T> : IBaseQueryableOperator<T>
        where T : class
    {
        T GetById(Guid id);

        T GetById(string id);

        IEnumerable<T> GetByIds(params string[] ids);

        IIncludeableReadRepository<T, TPro> Include<TPro>(
            Expression<Func<T, TPro>> navigationPropertyPath);

        IBaseQueryableOperator<TResult> Join<TJoin, TKey, TResult>(
            IBaseQueryableOperator<TJoin> joinRepo,
            Expression<Func<T, TKey>> keySelector,
            Expression<Func<TJoin, TKey>> joinSelector,
            Expression<Func<T, TJoin, TResult>> selector)
            where TJoin : class
            where TResult : class;

        IBaseQueryableOperator<T> Where(Expression<Func<T, bool>> predicate);
    }
}