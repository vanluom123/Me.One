using Me.One.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;

namespace Me.One.Core.Data
{
    public class EFCustomTransaction : ITransaction
    {
        private readonly IDbContextTransaction _transaction;

        public EFCustomTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}