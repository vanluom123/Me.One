using System.Threading.Tasks;
using Me.One.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Me.One.Core.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly ILogger _logger;
        private readonly ITransaction _transaction;

        public UnitOfWork(DbContext context, ILogger logger, ITransaction transaction)
        {
            _context = context;
            _transaction = transaction;
            _logger = logger;
        }

        public async Task<int> Commit()
        {
            var num = 0;
            try
            {
                num = await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                num = -1;
                _logger.LogError(ex, "Saving changes fail.");
                throw;
            }

            return num;
        }

        public ITransaction CreateTransaction()
        {
            return _transaction == null
                ? new EFCustomTransaction(_context.Database.BeginTransaction())
                : new EmptyTransaction();
        }
    }
}