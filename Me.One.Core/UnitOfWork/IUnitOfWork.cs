using System.Threading.Tasks;

namespace Me.One.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        ITransaction CreateTransaction();

        Task<int> Commit();
    }
}