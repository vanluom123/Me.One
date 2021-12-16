using System.Linq;
using Microsoft.EntityFrameworkCore.Query;

namespace Me.One.Core.Contract.Repository
{
    public interface IIncludeableReadRepository<T, TPro> : IIncludableQueryable<T, TPro>, IBaseReadRepository<T> where T : class
    {
    }
}