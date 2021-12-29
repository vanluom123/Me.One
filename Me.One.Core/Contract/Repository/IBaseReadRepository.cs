using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Me.One.Core.Contract.Repository
{
    public interface IBaseReadRepository<T> where T : class
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

        IBaseReadRepository<T> Include(string navigationPropertyPath);
      
        T GetById(Guid id);

        T GetById(string id);

        IEnumerable<T> GetByIds(params string[] ids);

        IIncludeableReadRepository<T, TPro> Include<TPro>(
            Expression<Func<T, TPro>> navigationPropertyPath);

        IBaseReadRepository<TResult> Join<TJoin, TKey, TResult>(
            IBaseReadRepository<TJoin> joinRepo,
            Expression<Func<T, TKey>> keySelector,
            Expression<Func<TJoin, TKey>> joinSelector,
            Expression<Func<T, TJoin, TResult>> selector)
            where TJoin : class
            where TResult : class;

        IBaseReadRepository<T> Where(Expression<Func<T, bool>> predicate);

        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        
        Task<T> GetByIdAsync(Guid id);
        
        Task<T> GetByIdAsync(string id);
        
        Task<IEnumerable<T>> GetByIdsAsync(params string[] ids);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}