using Kledex.Queries;

namespace Me.One.Core.CQRS.Models
{
    public class BaseQuery<T> : BaseQuery, IQuery<T>
    {
    }
}