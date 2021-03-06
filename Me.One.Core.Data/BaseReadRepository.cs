using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Me.One.Core.Contract.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Me.One.Core.Data
{
    public abstract class BaseReadRepository<T> : IBaseReadRepository<T> where T : class
    {
        protected BaseReadRepository(DbContext context)
        {
            Query = context.Set<T>();
        }

        public virtual IQueryable<T> Query { get; protected set; }

        public T GetById(Guid id)
        {
            return GetById(id.ToString());
        }

        public T GetById(string id)
        {
            var propertyInfos = typeof(T).BaseType.GetProperties();
            var propertyInfo = propertyInfos.FirstOrDefault(info => info.Name == "Id");
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "q");
            MemberExpression memberExpression = Expression.Property(parameterExpression, propertyInfo);
            BinaryExpression binaryExpression = Expression.Equal(memberExpression, Expression.Constant(id));
            LambdaExpression lambdaExpression = Expression.Lambda(binaryExpression, parameterExpression);
            MethodCallExpression methodCallExpression = Expression.Call(
                typeof(Queryable),
                "FirstOrDefault",
                new Type[] { typeof(T) },
                Query.Expression,
                Expression.Quote(lambdaExpression));
            var result = Query.Provider.Execute<T>(methodCallExpression);
            return result;
        }

        public IEnumerable<T> GetByIds(params string[] ids)
        {
            return ids.Select(GetById);
        }

        public virtual IEnumerable<T> List()
        {
            return List(Query);
        }

        public virtual IEnumerable<T> List(Expression<Func<T, bool>> predicate)
        {
            return List(Query, predicate);
        }

        public virtual IEnumerable<T> List(
            Expression<Func<T, bool>> predicate,
            int pageIndex,
            int pageSize,
            out long totalRecords)
        {
            return List(Query, predicate, pageIndex, pageSize, out totalRecords);
        }

        public virtual IEnumerable<T> List(
            Expression<Func<T, bool>> predicate,
            int pageIndex,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>> orderBy,
            out long totalRecords)
        {
            return List(Query, predicate, pageIndex, pageSize, orderBy, out totalRecords);
        }

        public bool Any(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? Query.Any() : Query.Any(predicate);
        }

        public IBaseReadRepository<T> Include(string navigationPropertyPath)
        {
            return new QueryableRepoOperator<T>(this).Include(navigationPropertyPath);
        }

        public IIncludeableReadRepository<T, TPro> Include<TPro>(
            Expression<Func<T, TPro>> navigationPropertyPath)
        {
            return new QueryableRepoOperator<T>(this).Include(navigationPropertyPath);
        }

        public IBaseReadRepository<TResult> Join<TJoin, TKey, TResult>(
            IBaseReadRepository<TJoin> joinRepo,
            Expression<Func<T, TKey>> keySelector,
            Expression<Func<TJoin, TKey>> joinSelector,
            Expression<Func<T, TJoin, TResult>> selector)
            where TJoin : class
            where TResult : class
        {
            return new JoinQueryableOperator<T>(this).Join(joinRepo, keySelector, joinSelector, selector);
        }

        public IEnumerable<T> List(
            Expression<Func<T, bool>> predicate,
            List<OrderBy> orderBy)
        {
            return List(Query, predicate, orderBy);
        }

        public IEnumerable<T> List(
            Expression<Func<T, bool>> predicate,
            List<OrderBy> orderBy,
            int pageIndex,
            int pageSize,
            out long totalRecords)
        {
            return List(Query, predicate, orderBy, pageIndex, pageSize, out totalRecords);
        }

        public IBaseReadRepository<T> Where(Expression<Func<T, bool>> predicate)
        {
            Query = Query.Where(predicate);
            return this;
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return Query.FirstOrDefault(predicate);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return Query.FirstOrDefaultAsync(predicate);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return this.GetByIdAsync(id.ToString());
        }

        public Task<T> GetByIdAsync(string id)
        {
            var propertyInfos = typeof(T).BaseType.GetProperties();
            var propertyInfo = propertyInfos.FirstOrDefault(info => info.Name == "Id");
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "q");
            MemberExpression memberExpression = Expression.Property(parameterExpression, propertyInfo);
            BinaryExpression binaryExpression = Expression.Equal(memberExpression, Expression.Constant(id));
            LambdaExpression lambdaExpression = Expression.Lambda(binaryExpression, parameterExpression);
            MethodCallExpression methodCallExpression = Expression.Call(
                typeof(Queryable),
                "FirstOrDefaultAsync",
                new Type[] { typeof(T) },
                Query.Expression,
                Expression.Quote(lambdaExpression));
            var result = Query.Provider.Execute<T>(methodCallExpression);
            return Task.FromResult(result);

        }

        public Task<IEnumerable<T>> GetByIdsAsync(params string[] ids)
        {
            return Task.FromResult(ids.Select(GetById));
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return Query.AnyAsync(predicate);
        }

        private IOrderedQueryable<T> Order(
            IQueryable<T> query,
            string propertyName,
            bool descending,
            bool anotherLevel)
        {
            var parameterExpression = Expression.Parameter(typeof(T), string.Empty);
            var memberExpression = Expression.PropertyOrField(parameterExpression, propertyName);
            var lambdaExpression = Expression.Lambda(memberExpression, parameterExpression);
            var methodCallExpression = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                new Type[2] { typeof(T), memberExpression.Type },
                query.Expression,
                Expression.Quote(lambdaExpression));
            return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(methodCallExpression);
        }

        protected virtual IEnumerable<T> List(
            IQueryable<T> query,
            Expression<Func<T, bool>> predicate,
            List<OrderBy> orderBy)
        {
            var queryable = query.Where(predicate);
            var count = orderBy.Count;
            if (count <= 0)
            {
                return queryable.AsEnumerable();
            }

            for (var index = 0; index < count; ++index)
            {
                var orderBy1 = orderBy[index];
                queryable = index != 0
                    ? Order(queryable, orderBy1.PropertyName, orderBy1.Desc, true)
                    : (IQueryable<T>)Order(queryable, orderBy1.PropertyName, orderBy1.Desc, false);
            }

            return queryable.AsEnumerable();
        }

        protected virtual IEnumerable<T> List(
            IQueryable<T> query,
            Expression<Func<T, bool>> predicate,
            List<OrderBy> orderBy,
            int pageIndex,
            int pageSize,
            out long totalRecords)
        {
            var queryable = query.Where(predicate);
            var count1 = orderBy.Count;
            if (count1 > 0)
                for (var index = 0; index < count1; ++index)
                {
                    var orderBy1 = orderBy[index];
                    queryable = index != 0
                        ? Order(queryable, orderBy1.PropertyName, orderBy1.Desc, true)
                        : (IQueryable<T>)Order(queryable, orderBy1.PropertyName, orderBy1.Desc, false);
                }

            var count2 = (pageIndex - 1) * pageSize;
            totalRecords = Query.Count(predicate);
            return queryable.Skip(count2).Take(pageSize).AsEnumerable();
        }

        protected virtual IEnumerable<T> List(IQueryable<T> query)
        {
            return query.AsNoTracking().AsEnumerable();
        }

        protected virtual IEnumerable<T> List(
            IQueryable<T> query,
            Expression<Func<T, bool>> predicate)
        {
            return query.AsNoTracking().Where(predicate).AsEnumerable();
        }

        protected virtual IEnumerable<T> List(
            IQueryable<T> query,
            Expression<Func<T, bool>> predicate,
            int pageIndex,
            int pageSize,
            out long totalRecords)
        {
            var count = (pageIndex - 1) * pageSize;
            totalRecords = Query.Count(predicate);
            return query.AsNoTracking().Where(predicate).Skip(count).Take(pageSize).AsEnumerable();
        }

        protected virtual IEnumerable<T> List(
            IQueryable<T> source,
            Expression<Func<T, bool>> predicate,
            int pageIndex,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>> orderBy,
            out long totalRecords)
        {
            var count = (pageIndex - 1) * pageSize;
            totalRecords = source.AsNoTracking().Count(predicate);
            var source1 = source.AsNoTracking().Where(predicate);
            if (orderBy != null)
                source1 = orderBy(source1);
            return source1.Skip(count).Take(pageSize).AsEnumerable();
        }


        internal class IncludeQueryableOperator<TEntity, TPro> :
            QueryableRepoOperator<TEntity>,
            IIncludeableReadRepository<TEntity, TPro>
            where TEntity : class
        {
            public IncludeQueryableOperator(
                QueryableRepoOperator<TEntity> repo)
                : base(repo)
            {
            }

            public IncludeQueryableOperator(
                QueryableRepoOperator<TEntity> repo,
                IIncludableQueryable<TEntity, TPro> includableQueryable) : base(repo)
            {
                IncludableQueryable = includableQueryable;
                Query = IncludableQueryable;
            }

            public IIncludeableReadRepository<TEntity, TPro> IncludeInternal(
                Expression<Func<TEntity, TPro>> navigationPropertyPath)
            {
                IncludableQueryable = Query.Include(navigationPropertyPath);
                Query = IncludableQueryable;
                return this;
            }

            public IIncludableQueryable<TEntity, TPro> IncludableQueryable { get; protected set; }

        }

        internal class JoinQueryableOperator<TEntity> : QueryableRepoOperator<TEntity>
            where TEntity : class
        {
            public JoinQueryableOperator(BaseReadRepository<TEntity> repo)
                : base(repo)
            {
            }

            public override IBaseReadRepository<TResult> Join<TJoin, TKey, TResult>(
                IBaseReadRepository<TJoin> repo,
                Expression<Func<TEntity, TKey>> keySelector,
                Expression<Func<TJoin, TKey>> joinSelector,
                Expression<Func<TEntity, TJoin, TResult>> selector)
            {
                var queryable = repo switch
                {
                    BaseReadRepository<TJoin> baseReadRepository => baseReadRepository.Query,
                    IQueryRepository<TJoin> queryRepository => queryRepository.Query,
                    _ => null
                };

                if (queryable == null) throw new ArgumentNullException(nameof(repo));

                return new QueryableOperator<TResult>(Query.Join(queryable, keySelector, joinSelector, selector));
            }
        }

        internal class QueryableOperator<TEntity> : IQueryRepository<TEntity>
            where TEntity : class
        {
            public QueryableOperator(IQueryable<TEntity> query)
            {
                Query = query;
            }

            public IEnumerable<TEntity> List()
            {
                return Query.AsNoTracking().AsEnumerable();
            }

            public IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> predicate)
            {
                return List(Query, predicate);
            }

            public IEnumerable<TEntity> List(
                Expression<Func<TEntity, bool>> predicate,
                int pageIndex,
                int pageSize,
                out long totalRecords)
            {
                return List(Query, predicate, pageIndex, pageSize, out totalRecords);
            }

            public IEnumerable<TEntity> List(
                Expression<Func<TEntity, bool>> predicate,
                int pageIndex,
                int pageSize,
                Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy,
                out long totalRecords)
            {
                return List(Query, predicate, pageIndex, pageSize, orderBy, out totalRecords);
            }

            public bool Any(Expression<Func<TEntity, bool>> predicate = null)
            {
                if (predicate == null) throw new ArgumentNullException(nameof(predicate));
                return Query.Any(predicate);
            }

            public TEntity GetById(Guid id)
            {
                return GetById(id.ToString());
            }

            public TEntity GetById(string id)
            {
                var propertyInfos = typeof(TEntity).BaseType.GetProperties();
                var propertyInfo = propertyInfos.FirstOrDefault(info => info.Name == "Id");
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "q");
                MemberExpression memberExpression = Expression.Property(parameterExpression, propertyInfo);
                BinaryExpression binaryExpression = Expression.Equal(memberExpression, Expression.Constant(id));
                LambdaExpression lambdaExpression = Expression.Lambda(binaryExpression, parameterExpression);
                MethodCallExpression methodCallExpression = Expression.Call(
                    typeof(Queryable),
                    "FirstOrDefault",
                    new Type[] { typeof(TEntity) },
                    Query.Expression,
                    Expression.Quote(lambdaExpression));
                var result = Query.Provider.Execute<TEntity>(methodCallExpression);
                return result;
            }

            public IEnumerable<TEntity> GetByIds(params string[] ids)
            {
                return ids.Select(GetById);
            }

            public IIncludeableReadRepository<TEntity, TPro> Include<TPro>(
                Expression<Func<TEntity, TPro>> navigationPropertyPath)
            {
                throw new NotSupportedException();
            }

            public IBaseReadRepository<TResult> Join<TJoin, TKey, TResult>(
                IBaseReadRepository<TJoin> joinRepo,
                Expression<Func<TEntity, TKey>> keySelector,
                Expression<Func<TJoin, TKey>> joinSelector,
                Expression<Func<TEntity, TJoin, TResult>> selector)
                where TJoin : class
                where TResult : class
            {
                throw new NotSupportedException();
            }

            public IBaseReadRepository<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
            {
                Query = Query.Where(predicate);
                return this;
            }

            public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
            {
                return Query.FirstOrDefault(predicate);
            }

            public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
            {
                return Query.FirstOrDefaultAsync(predicate);
            }

            public Task<TEntity> GetByIdAsync(Guid id)
            {
                return this.GetByIdAsync(id.ToString());
            }

            public Task<TEntity> GetByIdAsync(string id)
            {
                var propertyInfos = typeof(TEntity).BaseType.GetProperties();
                var propertyInfo = propertyInfos.FirstOrDefault(info => info.Name == "Id");
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "q");
                MemberExpression memberExpression = Expression.Property(parameterExpression, propertyInfo);
                BinaryExpression binaryExpression = Expression.Equal(memberExpression, Expression.Constant(id));
                LambdaExpression lambdaExpression = Expression.Lambda(binaryExpression, parameterExpression);
                MethodCallExpression methodCallExpression = Expression.Call(
                    typeof(Queryable),
                    "FirstOrDefaultAsync",
                    new Type[] { typeof(TEntity) },
                    Query.Expression,
                    Expression.Quote(lambdaExpression));
                var result = Query.Provider.Execute<TEntity>(methodCallExpression);
                return Task.FromResult(result);
            }

            public Task<IEnumerable<TEntity>> GetByIdsAsync(params string[] ids)
            {
                return Task.FromResult(ids.Select(GetById));
            }

            public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
            {
                return Query.AnyAsync(predicate);
            }

            public IBaseReadRepository<TEntity> Include(string navigationPropertyPath)
            {
                Query = Query.Include(navigationPropertyPath);
                return this;
            }

            public IEnumerable<TEntity> List(
                Expression<Func<TEntity, bool>> predicate,
                List<OrderBy> orderBy)
            {
                return List(Query, predicate, orderBy);
            }

            public IEnumerable<TEntity> List(
                Expression<Func<TEntity, bool>> predicate,
                List<OrderBy> orderBy,
                int pageIndex,
                int pageSize,
                out long totalRecords)
            {
                return List(Query, predicate, orderBy, pageIndex, pageSize, out totalRecords);
            }

            public IQueryable<TEntity> Query { get; protected set; }

            private IOrderedQueryable<TEntity> Order(
                IQueryable<TEntity> query,
                string propertyName,
                bool descending,
                bool anotherLevel)
            {
                var parameterExpression = Expression.Parameter(typeof(TEntity), string.Empty);
                var memberExpression = Expression.PropertyOrField(parameterExpression, propertyName);
                var lambdaExpression = Expression.Lambda(memberExpression, parameterExpression);
                var methodCallExpression = Expression.Call(
                    typeof(Queryable),
                    (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                    new Type[2] { typeof(TEntity), memberExpression.Type },
                    query.Expression, Expression.Quote(lambdaExpression));
                return (IOrderedQueryable<TEntity>)query.Provider.CreateQuery<TEntity>(methodCallExpression);
            }

            private IEnumerable<TEntity> List(
                IQueryable<TEntity> query,
                Expression<Func<TEntity, bool>> predicate,
                int pageIndex,
                int pageSize,
                out long totalRecords)
            {
                var count = (pageIndex - 1) * pageSize;
                totalRecords = query.Count(predicate);
                return query.AsNoTracking().Where(predicate).Skip(count).Take(pageSize).AsEnumerable();
            }

            private IEnumerable<TEntity> List(
                IQueryable<TEntity> source,
                Expression<Func<TEntity, bool>> predicate,
                int pageIndex,
                int pageSize,
                Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy,
                out long totalRecords)
            {
                var count = (pageIndex - 1) * pageSize;
                totalRecords = source.AsNoTracking().Count(predicate);
                var source1 = source.AsNoTracking().Where(predicate);
                if (orderBy != null)
                    source1 = orderBy(source1);
                return source1.Skip(count).Take(pageSize).AsEnumerable();
            }

            private IEnumerable<TEntity> List(
                IQueryable<TEntity> query,
                Expression<Func<TEntity, bool>> predicate)
            {
                return query.AsNoTracking().Where(predicate).AsEnumerable();
            }

            private IEnumerable<TEntity> List(
                IQueryable<TEntity> query,
                Expression<Func<TEntity, bool>> predicate,
                List<OrderBy> orderBy)
            {
                var queryable = query.AsNoTracking().Where(predicate);
                var count = orderBy.Count;
                if (count > 0)
                    for (var index = 0; index < count; ++index)
                    {
                        var orderBy1 = orderBy[index];
                        queryable = index != 0
                            ? Order(queryable, orderBy1.PropertyName, orderBy1.Desc, true)
                            : (IQueryable<TEntity>)Order(queryable, orderBy1.PropertyName, orderBy1.Desc, false);
                    }

                return queryable.AsEnumerable();
            }

            private IEnumerable<TEntity> List(
                IQueryable<TEntity> query,
                Expression<Func<TEntity, bool>> predicate,
                List<OrderBy> orderBy,
                int pageIndex,
                int pageSize,
                out long totalRecords)
            {
                var queryable = query.Where(predicate);
                var count1 = orderBy.Count;
                if (count1 > 0)
                    for (var index = 0; index < count1; ++index)
                    {
                        var orderBy1 = orderBy[index];
                        queryable = index != 0
                            ? Order(queryable, orderBy1.PropertyName, orderBy1.Desc, true)
                            : (IQueryable<TEntity>)Order(queryable, orderBy1.PropertyName, orderBy1.Desc, false);
                    }

                var count2 = (pageIndex - 1) * pageSize;
                totalRecords = queryable.Count(predicate);
                return queryable.Skip(count2).Take(pageSize).AsEnumerable();
            }

        }

        internal class QueryableRepoOperator<TEntity> : IQueryRepository<TEntity>
            where TEntity : class
        {
            private readonly BaseReadRepository<TEntity> _repo;

            public QueryableRepoOperator(BaseReadRepository<TEntity> repo)
            {
                _repo = repo;
                Query = repo.Query;
            }

            protected QueryableRepoOperator(
                QueryableRepoOperator<TEntity> repo)
            {
                _repo = repo._repo;
                Query = repo.Query;
            }

            public TEntity GetById(Guid id)
            {
                return _repo.GetById(id);
            }

            public TEntity GetById(string id)
            {
                return _repo.GetById(id);
            }

            public IEnumerable<TEntity> GetByIds(params string[] ids)
            {
                return _repo.GetByIds(ids);
            }

            public IEnumerable<TEntity> List()
            {
                return _repo.List(Query);
            }

            public IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> predicate)
            {
                return _repo.List(Query, predicate);
            }

            public IEnumerable<TEntity> List(
                Expression<Func<TEntity, bool>> predicate,
                int pageIndex,
                int pageSize,
                out long totalRecords)
            {
                return _repo.List(Query, predicate, pageIndex, pageSize, out totalRecords);
            }

            public IEnumerable<TEntity> List(
                Expression<Func<TEntity, bool>> predicate,
                int pageIndex,
                int pageSize,
                Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy,
                out long totalRecords)
            {
                return _repo.List(Query, predicate, pageIndex, pageSize, orderBy, out totalRecords);
            }

            public bool Any(Expression<Func<TEntity, bool>> predicate = null)
            {
                return _repo.Any(predicate);
            }

            public IBaseReadRepository<TEntity> Include(string navigationPropertyPath)
            {
                Query = Query.Include(navigationPropertyPath);
                return this;
            }

            public IIncludeableReadRepository<TEntity, TPro> Include<TPro>(
                Expression<Func<TEntity, TPro>> navigationPropertyPath)
            {
                return new IncludeQueryableOperator<TEntity, TPro>(this).IncludeInternal(navigationPropertyPath);
            }

            public virtual IBaseReadRepository<TResult> Join<TJoin, TKey, TResult>(
                IBaseReadRepository<TJoin> joinRepo,
                Expression<Func<TEntity, TKey>> keySelector,
                Expression<Func<TJoin, TKey>> joinSelector,
                Expression<Func<TEntity, TJoin, TResult>> selector)
                where TJoin : class
                where TResult : class
            {
                return _repo.Join(joinRepo, keySelector, joinSelector, selector);
            }

            public IEnumerable<TEntity> List(
                Expression<Func<TEntity, bool>> predicate,
                List<OrderBy> orderBy)
            {
                return _repo.List(predicate, orderBy);
            }

            public IEnumerable<TEntity> List(
                Expression<Func<TEntity, bool>> predicate,
                List<OrderBy> orderBy,
                int pageIndex,
                int pageSize,
                out long totalRecords)
            {
                return _repo.List(predicate, orderBy, pageIndex, pageSize, out totalRecords);
            }

            public IBaseReadRepository<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
            {
                return _repo.Where(predicate);
            }

            public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
            {
                return _repo.FirstOrDefault(predicate);
            }

            public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
            {
                return _repo.FirstOrDefaultAsync(predicate);
            }

            public Task<TEntity> GetByIdAsync(Guid id)
            {
                return _repo.GetByIdAsync(id);
            }

            public Task<TEntity> GetByIdAsync(string id)
            {
                return _repo.GetByIdAsync(id);
            }

            public Task<IEnumerable<TEntity>> GetByIdsAsync(params string[] ids)
            {
                return _repo.GetByIdsAsync(ids);
            }

            public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
            {
                return _repo.AnyAsync(predicate);
            }


            public IQueryable<TEntity> Query
            {
                get => _repo.Query;
                protected set => _repo.Query = value;
            }
        }

    }
}