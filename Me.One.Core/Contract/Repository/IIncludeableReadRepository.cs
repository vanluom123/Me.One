using System.Linq;
using Microsoft.EntityFrameworkCore.Query;

namespace Me.One.Core.Contract.Repository
{
    public interface IIncludeableReadRepository<T, out TPro> : IBaseReadRepository<T> where T : class
    {
    }
}