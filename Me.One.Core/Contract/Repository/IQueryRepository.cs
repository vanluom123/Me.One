using System.Linq;

namespace Me.One.Core.Contract.Repository
{
    public interface IQueryRepository<T> : IBaseReadRepository<T> where T : class
    {
        IQueryable<T> Query { get; }
    }
}