using System;
using Me.One.Core.UnitOfWork;

namespace Me.One.Core.Data
{
    public class EmptyTransaction : ITransaction, IDisposable
    {
        public void Commit()
        {
        }

        public void Dispose()
        {
        }
    }
}